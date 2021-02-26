using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;

namespace SiteShow.Teacher
{
    /// <summary>
    /// 教师的信息展示
    /// </summary>
    public class Detail : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //教师
            int id = Common.Request.QueryString["id"].Int32 ?? 0;
            EntitiesInfo.Teacher th = Business.Do<ITeacher>().TeacherSingle(id);
            if (th == null) return;
            if ((Common.Request.Cookies["Teacher_" + th.Th_ID].Int32 ?? 0) == 0)
            {
                th.Th_ViewNum++;
                Business.Do<ITeacher>().TeacherSave(th);
                context.Response.Cookies["Teacher_" + th.Th_ID].Value = th.Th_ID.ToString();
            }
            th.Th_Photo = Upload.Get["Teacher"].Virtual + th.Th_Photo;
            this.Document.Variables.SetValue("th", th);
            //教师的履历
            EntitiesInfo.TeacherHistory[] history = Business.Do<ITeacher>().HistoryAll(th.Th_ID);
            this.Document.Variables.SetValue("history", history);
            //教师的课程
            List<EntitiesInfo.Course> courses = Business.Do<ICourse>().CourseCount(-1, -1, th.Th_ID,-1, null, true, -1);
            for (int i = 0; i < courses.Count; i++)
            {
                EntitiesInfo.Course c = courses[i];
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
            }
            this.Document.Variables.SetValue("courses", courses);
            //老师列表
            Tag thTag = this.Document.GetChildTagById("teachers");
            if (thTag != null)
            {
                int thCount = int.Parse(thTag.Attributes.GetValue("count", "10"));
                EntitiesInfo.Teacher[] teachers = Business.Do<ITeacher>().TeacherCount(this.Organ.Org_ID, true, thCount);
                foreach (EntitiesInfo.Teacher c in teachers)
                {
                    c.Th_Photo = Upload.Get["Teacher"].Virtual + c.Th_Photo;
                }
                this.Document.SetValue("teachers", teachers);
            }
        }
    }
}