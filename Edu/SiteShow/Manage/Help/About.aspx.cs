using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SiteShow.Manage.Help
{
    public partial class About : Extend.CustomPage
    {
        //��Ȩ��Ϣ
        protected Common.Copyright<string, string> copyright = Common.Request.Copyright;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
