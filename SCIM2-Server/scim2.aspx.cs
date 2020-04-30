using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DiligenteSCIM2;

namespace SCIM2_Server
{
    public partial class Scim2 : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Application["db"] == null)
            {
                DataTable db = new DataTable("worstdbever");
                db.Columns.Add("Id", typeof(int));
                db.Columns.Add("Username");
                db.Columns.Add("Smurf");

                db.Rows.Add(1, "Jane.Doe@some.thing", "red");
                db.Rows.Add(2, "John.Doe@some.thing", "blue");

                Application.Lock();
                Application["db"] = db;
                Application.UnLock();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SCIM2();
        }

        [WebMethod(MessageName = "Groups")]
        public static void Groups()
        {
            SCIM2();
        }

        [WebMethod(MessageName = "Users")]
        public static void SCIM2()
        {

            HttpContext.Current.Application.Lock();
            string log = string.Format("<p>{0} {1}</p><hr />{2}", HttpContext.Current.Request.HttpMethod, HttpContext.Current.Request.RawUrl, HttpContext.Current.Application["log"]);
            HttpContext.Current.Application["log"] = log;


            try
            {
                // Authentication.HTTPHeader httpHeader = new Authentication.HTTPHeader() { Token="geheim"};
                Authentication.BasicAuth authentication = new Authentication.BasicAuth() { Username = "gebruikersnaam", Password = "ditisgeheim" };
                SCIM2 scim2 = new SCIM2(HttpContext.Current.Request, HttpContext.Current.Response);
                scim2.Authenticate(authentication);
                scim2.NewUser += NewUser;
                scim2.GetUsers += GetUsers;
                scim2.GetUsersFilter += GetUsersFilter;
                scim2.Process();
            }
            catch (Exception ex)
            {
                //Ok, admit this is the worst logger ever. Don't use for production!!
                HttpContext.Current.Application.Lock();
                log = string.Format("<p>{0}</p><hr />{1}", ex.Message, HttpContext.Current.Application["log"]);
                HttpContext.Current.Application["log"] = log;
                HttpContext.Current.Application.UnLock();
            }

        }


        public class MyUser : CResource
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Smurf { get; set; }

            public MyUser()
            {
                Schemas = SchemasHelper.Get(SchemasHelper.Schema.User);
            }

            public MyUser(DataRow r)
            {
                Schemas = SchemasHelper.Get(SchemasHelper.Schema.User);
                Id = (int)r["id"];
                Username = r["Username"].ToString();
                Smurf = r["Smurf"].ToString();
            }

        }


        public static void NewUser(JsonDocument data)
        {
            var username = data.RootElement.GetProperty("userName").ToString();
            HttpContext.Current.Application.Lock();
            DataTable db = ((DataTable)HttpContext.Current.Application["db"]);
            DataRow dr = db.NewRow();
            dr["id"] = db.Rows.Count + 1;
            dr["userName"] = username;
            dr["smurf"] = "";
            db.Rows.Add(dr);
            HttpContext.Current.Application["db"] = db;
            HttpContext.Current.Application.UnLock();

            HttpContext.Current.Application.Lock();
            string log = string.Format("<p>AddUser: {0}</p><hr />{1}", username, HttpContext.Current.Application["log"]);
            HttpContext.Current.Application["log"] = log;
            HttpContext.Current.Application.UnLock();
        }

        public static Result GetUsers(int startIndex, int count)
        {
            Result result = new Result(SchemasHelper.Schema.ListResponse);
            List<MyUser> users = new List<MyUser>();
            foreach (DataRow r in ((DataTable)HttpContext.Current.Application["db"]).Rows) users.Add(new MyUser(r));
            result.Resources = users.ToArray();
            return result;
        }

        public static Result GetUsersFilter(Filter filter, int startIndex, int count)
        {
            Result result = new Result(SchemasHelper.Schema.ListResponse);
            if (filter.Operator == "eq")
            {
                DataRow[] drs = ((DataTable)HttpContext.Current.Application["db"]).Select(string.Format("{0} = '{1}'", filter.Value1, filter.Value2));
                List<MyUser> users = new List<MyUser>();
                foreach (DataRow r in drs) users.Add(new MyUser(r));
                result.Resources = users.ToArray();
            }
            return result;
        }

    }
}