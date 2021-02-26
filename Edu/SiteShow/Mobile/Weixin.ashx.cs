using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 在微信中使用时的直接登录页
    /// </summary>
    public class Weixin : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //设置主域，用于js跨根域
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (multi == 0 && !Common.Server.IsLocalIP)
                this.Document.Variables.SetValue("domain", Common.Server.MainName);
            //微信登录
            this.Document.SetValue("WeixinLoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false);
            this.Document.SetValue("WeixinpubAPPID", Business.Do<ISystemPara>()["WeixinpubAPPID"].String);
            this.Document.SetValue("WeixinpubReturl", Business.Do<ISystemPara>()["WeixinpubReturl"].Value ?? Common.Server.MainName);
        }
    }
}