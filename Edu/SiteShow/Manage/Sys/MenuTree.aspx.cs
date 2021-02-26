﻿using System;
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

namespace SiteShow.Manage.Sys
{
    public partial class MenuTree : Extend.CustomPage
    {
        //要操作的对象主键
        protected int id = Common.Request.QueryString["id"].Int32 ?? 0;
        //根菜单项，用于“移动”或“复制到”
        protected EntitiesInfo.ManageMenu[] root = Business.Do<IManageMenu>().GetRoot("func");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                EntitiesInfo.ManageMenu m = Business.Do<IManageMenu>().GetSingle(id);
                lbName.Text = m.MM_Name;
            }
        }
    }
}
