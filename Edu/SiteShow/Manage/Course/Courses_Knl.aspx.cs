using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;

namespace SiteShow.Manage.Course
{
    public partial class Courses_Knl : Extend.CustomPage
    {
        //课程id
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //隐藏确定按钮
            EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
            btnEnter.Visible = false;  
        }
    }
}