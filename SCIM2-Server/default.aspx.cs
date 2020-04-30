using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCIM2_Server
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            llog.Text = string.Format("{0}", Application["log"]);
        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            Application.Lock();
            Application["log"] = "";
            Application.UnLock();
            llog.Text = "";
        }
    }
}