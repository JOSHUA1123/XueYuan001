using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using EntitiesInfo;

namespace SiteShow.Teacher
{
    /// <summary>
    /// Startpage 的摘要说明
    /// </summary>
    public class Startpage : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //所属教师
            EntitiesInfo.Teacher th = Extend.LoginState.Accounts.Teacher;
            if (th == null) return;
            //所属机构
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //当前教师负责的课程
            List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseAll(org.Org_ID, -1, th.Th_ID, null);
            this.Document.SetValue("courses", cous);
            this.Document.SetValue("path", Upload.Get["Course"].Virtual);
        }
    }
}