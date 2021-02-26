using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
namespace SiteShow.Mobile
{
    /// <summary>
    /// 我的课程
    /// </summary>
    public class SelfCourse : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前课程
            EntitiesInfo.Course currCou = Extend.LoginState.Accounts.Course();
            if (currCou != null) this.Document.SetValue("currCou", currCou);
        }
    }
}