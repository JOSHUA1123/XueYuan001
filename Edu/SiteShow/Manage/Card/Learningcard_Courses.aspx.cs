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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SiteShow.Manage.Card
{
    public partial class Learningcard_Courses : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;       
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
    }
}
