using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using Extend;
using System.Text.RegularExpressions;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 学员注册协议
    /// </summary>
    public class RegisterAgreement : BasePage
    {       
        protected override void InitPageTemplate(HttpContext context)
        {
            //注册协议
            string agreement = Business.Do<IAccounts>().RegAgreement();
            this.Document.SetValue("Agreement", agreement);
            
        }
    }
}