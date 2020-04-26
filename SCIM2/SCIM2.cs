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
        public int? startIndex = null;
        public int? count = null;
        public SCIM2(HttpRequest httpRequest, HttpResponse httpResponse, iAuthenticationModes authenticationMode)
        {


            Authentication authenticate = new Authentication(httpRequest, authenticationMode);
            if (!Enum.TryParse(httpRequest.HttpMethod, out HttpMethod httpMethod)) throw new Exception("unknown HTTP Method");
            string endPoint = httpRequest.PathInfo;

            startIndex = IQString(httpRequest, "startIndex");
            count = IQString(httpRequest, "count");

            Result result = null;

            switch (endPoint.ToUpper())
            {
                case "/USERS":
                    switch (httpMethod)
                    {
                        case HttpMethod.GET:
                            result = Users.Get(startIndex, count);
                            break;
                    }
                    break;

            }

            if (result != null)
            {
                string json = JsonSerializer.Serialize<Result>(result);
                httpResponse.Clear();
                httpResponse.ContentType = "application/json";
                httpResponse.Write(json);
                httpResponse.End();
            }

            throw new Exception(string.Format("{0} {1}", httpRequest.RawUrl, authenticate.getAuthenticationMode));
        }

        private int? IQString(HttpRequest httpRequest, string key)
        {
            int? retval = null;
            if (httpRequest.QueryString[key] != null)
            {
                if (!int.TryParse(httpRequest.QueryString[key], out int ivalue)) retval = ivalue;
            }
            return retval;
        }

    }
}
