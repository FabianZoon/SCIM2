using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DiligenteSCIM2;

namespace SCIM2_Server
{


    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Authentication.HTTPHeader httpHeader = new Authentication.HTTPHeader() { Token="geheim"};
                Authentication.BasicAuth authentication = new Authentication.BasicAuth() { Username = "gebruikersnaam", Password = "ditisgeheim" };
                SCIM2 scim2 = new SCIM2(Request, Response);
                scim2.Authenticate(authentication);
                scim2.GetUsers += GetUsers;
                scim2.Process();
            }
            catch (Exception ex)
            {
                Application.Lock();
                string log = string.Format("<p>{0}</p><hr />{1}",ex.Message, Application["log"]);
                Application["log"] = log;
                Application.UnLock();
               
            }
        }

        Result GetUsers(int startIndex, int count)
        {
            Result result = new Result(SchemasHelper.Schema.ListResponse);
            List<Resources> resources = new List<Resources>();
            resources.Add(new Resources(SchemasHelper.Schema.User));
            result.Resources = resources.ToArray();
            return result;
        }

    }
}