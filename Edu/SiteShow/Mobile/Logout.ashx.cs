using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extend;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 退出登录
    /// </summary>
    public class Logout : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            LoginState.Accounts.Logout();           
        }
    }
}