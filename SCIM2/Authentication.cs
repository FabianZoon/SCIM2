using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DiligenteSCIM2
{

    public class Authentication
    {

        #region enums
        public enum AuthenticationMode { BasicAuth, HTTPHeader, OAuth2 }

        #endregion

        #region Properties
        private AuthenticationMode _authenticationMode;
        public AuthenticationMode getAuthenticationMode
        {
            get
            {
                return _authenticationMode;
            }
        }

        #endregion 


        public Authentication(HttpRequest httpRequest, iAuthenticationModes authenticationMode)
        {
            
            if (authenticationMode is BasicAuth)
            {
                BasicAuth basicAuth = (BasicAuth)authenticationMode;
                string basicrheader = httpRequest.Headers["Authorization"];
                if (basicrheader == null) throw new HttpException(401,"no authorization");
                if (!basicrheader.StartsWith("Basic ")) throw new HttpException(401,"not a basic authorization");

                var encoding = Encoding.GetEncoding("iso-8859-1");
                string credentials = encoding.GetString(Convert.FromBase64String(basicrheader.Substring(6)));
                if (credentials != string.Format("{0}:{1}", basicAuth.Username, basicAuth.Password)) throw new HttpException(401,"invalid token");
                _authenticationMode = AuthenticationMode.BasicAuth;
                return; // call is authenticated
            }
            if (authenticationMode is HTTPHeader)
            {
                string bearerheader = httpRequest.Headers["Authorization"];
                if (bearerheader == null) throw new HttpException(401,"no authorization");
                if (!bearerheader.StartsWith("Bearer ")) throw new HttpException(401,"not a bearer authorization");
                if (bearerheader != "Bearer " + ((HTTPHeader)authenticationMode).Token) throw new HttpException(401,"invalid token");
                _authenticationMode = AuthenticationMode.HTTPHeader;
                return; // call is authenticated
            }
            if (authenticationMode is OAUth2)
            {
                _authenticationMode = AuthenticationMode.OAuth2;
                throw new Exception("OAuth authentication is not yet implemented");
            }

        }


        public interface iAuthenticationModes { }

        public class BasicAuth : iAuthenticationModes
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class HTTPHeader : iAuthenticationModes
        {
            public string Token { get; set; }
        }

        public class OAUth2 : iAuthenticationModes
        {
            public string Access_token_endpoint_URI { get; set; }
            public string Authorization_endpoint_URI { get; set; }
            public string Client_ID { get; set; }
            public string Client_Secret { get; set; }
        }
    }
}
