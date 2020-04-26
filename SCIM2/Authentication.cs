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
        private readonly AuthenticationMode authenticationMode;
        public AuthenticationMode GetAuthenticationMode
        {
            get
            {
                return authenticationMode;
            }
        }

        #endregion 


        public Authentication(HttpRequest httpRequest, IAuthenticationModes iAuthenticationMode)
        {

            if (iAuthenticationMode is BasicAuth basicAuth)
            {
                string basicrheader = httpRequest.Headers["Authorization"];
                if (basicrheader == null) throw new HttpException(401, "no authorization");
                if (!basicrheader.StartsWith("Basic ")) throw new HttpException(401, "not a basic authorization");

                var encoding = Encoding.GetEncoding("iso-8859-1");
                string credentials = encoding.GetString(Convert.FromBase64String(basicrheader.Substring(6)));
                if (credentials != string.Format("{0}:{1}", basicAuth.Username, basicAuth.Password)) throw new HttpException(401, "invalid token");
                authenticationMode = AuthenticationMode.BasicAuth;
                return; // call is authenticated
            }
            if (iAuthenticationMode is HTTPHeader httpHeader )
            {
                string bearerheader = httpRequest.Headers["Authorization"];
                if (bearerheader == null) throw new HttpException(401,"no authorization");
                if (!bearerheader.StartsWith("Bearer ")) throw new HttpException(401,"not a bearer authorization");
                if (bearerheader != "Bearer " + httpHeader.Token) throw new HttpException(401,"invalid token");
                authenticationMode = AuthenticationMode.HTTPHeader;
                return; // call is authenticated
            }
            if (iAuthenticationMode is OAUth2)
            {
                //_authenticationMode = AuthenticationMode.OAuth2;
                throw new Exception("OAuth authentication is not yet implemented");
            }

        }


        public interface IAuthenticationModes { }

        public class BasicAuth : IAuthenticationModes
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class HTTPHeader : IAuthenticationModes
        {
            public string Token { get; set; }
        }

        public class OAUth2 : IAuthenticationModes
        {
            public string Access_token_endpoint_URI { get; set; }
            public string Authorization_endpoint_URI { get; set; }
            public string Client_ID { get; set; }
            public string Client_Secret { get; set; }
        }
    }
}
