using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extend;
using Common;
using ServiceInterfaces;

namespace SiteShow.Admin
{
    /// <summary>
    /// 机构管理员的界面
    /// </summary>
    public class Panel : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前版本
            string version = Common.License.Value.VersionName;
            Common.License lic = Common.License.Value;
            this.Document.Variables.SetValue("lic", lic);
            //
            EntitiesInfo.EmpAccount acc = this.Admin;
            if (acc == null)
            {
                context.Response.Redirect("index.ashx");
                return;
            }
            this.Document.Variables.SetValue("CurrUser", acc);
        }        
    }
}