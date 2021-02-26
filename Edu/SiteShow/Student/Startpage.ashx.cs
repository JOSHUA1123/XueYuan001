using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using EntitiesInfo;
using VTemplate.Engine;
namespace SiteShow.Student
{
    /// <summary>
    /// Startpage 的摘要说明
    /// </summary>
    public class Startpage : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin || this.Account == null) return;
            //资金流水记录
            Tag maTag = this.Document.GetChildTagById("moneyAccount");
            if (maTag != null)
            {
                int naCount = int.Parse(maTag.Attributes.GetValue("count", "5"));
                EntitiesInfo.MoneyAccount[] moneyAccount = null;
                moneyAccount = Business.Do<IAccounts>().MoneyCount(this.Organ.Org_ID, this.Account.Ac_ID, -1, true, naCount);
                this.Document.SetValue("moneyAccount", moneyAccount);
            }
            //当前学生的课程
            List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseForStudent(this.Account.Ac_ID, null, -1, null, 0);
            foreach (EntitiesInfo.Course c in cous)
            {
                //课程图片
                if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
                {
                    c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                    c.Cou_IsStudy = true;
                }
            }
            this.Document.SetValue("Course", cous);
            
        }
    }
}