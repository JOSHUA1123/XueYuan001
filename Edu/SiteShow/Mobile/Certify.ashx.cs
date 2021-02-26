using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
using System.Data;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 学员学习证明二维码扫描后的显示
    /// 
    /// </summary>
    public class Certify : BasePage
    {
        //学员ID
        private int accid = Common.Request.QueryString["acid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前学员
            EntitiesInfo.Accounts acc = Business.Do<IAccounts>().AccountsSingle(accid);
            //学员的学习情况记录
            DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(acc.Ac_ID);
            //
            this.Document.Variables.SetValue("acc", acc);
            this.Document.Variables.SetValue("logs", dt);
        } 
    }
}