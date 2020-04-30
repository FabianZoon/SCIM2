using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static DiligenteSCIM2.Authentication;

namespace DiligenteSCIM2
{
    public partial class SCIM2
    {
        //https://tools.ietf.org/html/rfc7644
        //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
        public enum HttpMethod { GET, POST, DELETE, PATCH, PUT }

        private readonly HttpRequest _httpRequest;
        private readonly HttpResponse _httpResponse;
        private Authentication authenticate;
        private bool Authenticated;


        #region delegates
        public delegate Result getUser(object id);
        public delegate Result getUsers(int startIndex, int count);
        public delegate Result getUsersFilter(Filter filter, int startIndex, int count);
        public delegate void newUser(JsonDocument json);

        public delegate Result getGroup(object id);
        public delegate Result getGroups(int startIndex, int count);

        public event getUser GetUser;
        public event getUsers GetUsers;
        public event getUsersFilter GetUsersFilter;
        public event newUser NewUser;

        public event getGroup GetGroup;
        public event getGroups GetGroups;
        #endregion



        #region Constructor
        public SCIM2(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            _httpRequest = httpRequest;
            _httpResponse = httpResponse;
            Authenticated = false;
        }
        #endregion

        public void Authenticate(IAuthenticationModes authenticationMode)
        {
            authenticate = new Authentication(_httpRequest, authenticationMode);
            Authenticated = true;
        }

        public void Process()
        {
            if (!Authenticated) throw new HttpException(401, "not authenticated");
            if (!Enum.TryParse(_httpRequest.HttpMethod, out HttpMethod httpMethod)) throw new Exception("unknown HTTP Method");
            string body = string.Empty;
            JsonDocument json= null;
            if (httpMethod == HttpMethod.POST)
            {
                _httpRequest.InputStream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(_httpRequest.InputStream);
                body = reader.ReadToEnd();
                json = JsonDocument.Parse(body);
              

            }
            string endPoint = _httpRequest.PathInfo;

            Result result = null;

            switch (endPoint.ToUpper())
            {
                case "/USERS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            int startIndex = IQString(_httpRequest, "startIndex");
                            int count = IQString(_httpRequest, "count");
                            string sfilter = QString(_httpRequest, "filter");
                            if (sfilter == string.Empty)
                            {
                                if (GetUsers != null) result = GetUsers(startIndex, count);
                            }
                            else
                            {
                                Filter filter = new Filter(sfilter);
                                if (GetUsersFilter != null) result = GetUsersFilter(filter, startIndex, count);

                            }
                            break;
                        case HttpMethod.POST:
                            NewUser?.Invoke(json);
                            break;
                    }
                    break;
                case "/GROUPS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            int startIndex = IQString(_httpRequest, "startIndex");
                            int count = IQString(_httpRequest, "count");
                            if (GetUsers != null) result = GetGroups(startIndex, count);
                            break;
                    }
                    break;

            }

            if (result != null)
            {
                string resultjson = JsonSerializer.Serialize<Result>(result);
                _httpResponse.Clear();
                _httpResponse.ContentType = "application/json";
                _httpResponse.Write(resultjson);
                _httpResponse.End();
            }

            throw new Exception(string.Format("{0} {1} {2}", _httpRequest.RawUrl, authenticate.GetAuthenticationMode, body));


        }


        private int IQString(HttpRequest httpRequest, string key, int defaultValue = 1)
        {
            int retval = defaultValue;
            if (httpRequest.QueryString[key] != null)
            {
                if (!int.TryParse(httpRequest.QueryString[key], out int ivalue)) retval = ivalue;
            }
            return retval;
        }
        private string QString(HttpRequest httpRequest, string key, string defaultValue = "")
        {
            return (httpRequest.QueryString[key] ?? defaultValue);
        }
    }
}
