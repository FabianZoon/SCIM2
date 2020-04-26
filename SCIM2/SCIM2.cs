using System;
using System.Collections.Generic;
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
        public enum HttpMethod { GET, PUSH, DELETE, PATCH, PUT }
       
        private readonly HttpRequest _httpRequest;
        private readonly HttpResponse _httpResponse;
        private Authentication authenticate;
        private bool Authenticated;


        #region delegates
        public delegate Result getUser(object id);
        public delegate Result getUsers(int startIndex, int count);

        public event getUser GetUser;
        public event getUsers GetUsers;
        #endregion



        #region Constructor
        public SCIM2(HttpRequest httpRequest, HttpResponse httpResponse)
        {
            _httpRequest = httpRequest;
            _httpResponse = httpResponse;
            Authenticated = false;
        }
        #endregion

        public void Authenticate (IAuthenticationModes authenticationMode)
        {
           authenticate= new Authentication(_httpRequest, authenticationMode);
           Authenticated = true;
        }

        public void Process()
        {
            if (!Authenticated) throw new HttpException(401, "not authenticated");
            if (!Enum.TryParse(_httpRequest.HttpMethod, out HttpMethod httpMethod)) throw new Exception("unknown HTTP Method");
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
                            if (GetUsers!=null) result = GetUsers(startIndex, count);
                            break;
                    }
                    break;

            }

            if (result != null)
            {
                string json = JsonSerializer.Serialize<Result>(result);
                _httpResponse.Clear();
                _httpResponse.ContentType = "application/json";
                _httpResponse.Write(json);
                _httpResponse.End();
            }

            throw new Exception(string.Format("{0} {1}", _httpRequest.RawUrl, authenticate.GetAuthenticationMode));


        }

      
        private int IQString(HttpRequest httpRequest, string key, int defaultValue=1)
        {
            int retval = defaultValue;
            if (httpRequest.QueryString[key] != null)
            {
                if (!int.TryParse(httpRequest.QueryString[key], out int ivalue)) retval = ivalue;
            }
            return retval;
        }

    }
}
