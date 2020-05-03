# SCIM2
SCIM 2.0 .Net Library. Especially designed for OKTA provisioning.

see also https://sites.google.com/diligente.nl/oktakladblok/scim-app-wizard
or watch the demo at https://youtu.be/6XSloVbt15A

Early Access feature of Okta is to provision any application with SCIM (2.0)
It's called SCIM App wizard and information can be found at https://help.okta.com/en/prod/Content/Topics/Apps/Apps_App_Integration_Wizard.htm

This project containts the SCIM2 Library and an Example SCIM2-server.
The library is also available as nuget package at https://www.nuget.org/packages/Diligente_SCIM2/



Create a ashx page and implement the functions you need.

        public void ProcessRequest(HttpContext context)
        {
            // Authentication.HTTPHeader httpHeader = new Authentication.HTTPHeader() { Token="secrettoken"};
            Authentication.BasicAuth authentication = new Authentication.BasicAuth() { Username = "username", Password = "password" };
            SCIM2 scim2 = new SCIM2(context.Request, context.Response);
            scim2.Authenticate(authentication);
            scim2.NewUser += Scim2_NewUser;
            scim2.GetUser += Scim2_GetUser;
            scim2.GetUsers += Scim2_GetUsers;
            scim2.GetUserKey += Scim2_GetUserKey;
            scim2.DelUser += Scim2_DelUser;
            scim2.UpdateUser += Scim2_UpdateUser;

            scim2.Log += Scim2_Log;

            scim2.Process();
        }
