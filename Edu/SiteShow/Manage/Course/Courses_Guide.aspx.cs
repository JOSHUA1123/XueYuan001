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


using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;

namespace SiteShow.Manage.Course
{
    public partial class Courses_Guide : Extend.CustomPage
    {
        //�γ�id
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //����ȷ����ť
            EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
            btnEnter.Visible = false;          
           
        }

    }
}
