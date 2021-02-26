using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
namespace SiteShow
{
    /// <summary>
    /// 课程列表页
    /// </summary>
    public class Courses : BasePage
    {
        //上级专业id
        //private int pid = Common.Request.QueryString["pid"].Int32 ?? -1;
        //当前专业id
        private int sbjid = Common.Request.QueryString["sbjid"].Int32 ?? 0;
        //检索字符
        private string search = Common.Request.QueryString["search"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("sbjid", sbjid);
            //专业列表           
            Tag sbjTag = this.Document.GetChildTagById("subject");
            if (sbjTag != null)
            {
                EntitiesInfo.Subject[] subj = Business.Do<ISubject>().SubjectCount(Organ.Org_ID, null, true, sbjid, 0);
                this.Document.SetValue("subject", subj);
            }            
            //当前专业             
            EntitiesInfo.Subject sbj = Business.Do<ISubject>().SubjectSingle(sbjid);
            this.Document.SetValue("sbj", sbj);
            //上级专业
            if (sbj != null)
            {
                List<EntitiesInfo.Subject> sbjs = Business.Do<ISubject>().Parents(sbj.Sbj_ID, true);
                this.Document.Variables.SetValue("sbjs", sbjs);
            }
            //课程列表           
            Tag couTag = this.Document.GetChildTagById("course");
            if (couTag != null)
            {
                int size = int.Parse(couTag.Attributes.GetValue("size", "12"));
                int index = Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                List<EntitiesInfo.Course> cour = Business.Do<ICourse>().CoursePager(Organ.Org_ID, sbjid, -1, true, search, "rec", size, index, out sum);
                foreach (EntitiesInfo.Course c in cour)
                {
                    c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                    c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                    c.Cou_Intro = HTML.ClearTag(c.Cou_Intro);
                }
                this.Document.SetValue("course", cour);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)size);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
            }
        }
    }
}