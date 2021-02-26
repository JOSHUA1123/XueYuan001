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
    /// 测试页
    /// </summary>
    public class Test : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {           
            //专业列表           
            Tag sbjTag = this.Document.GetChildTagById("subject");
            if (sbjTag != null)
            {
                EntitiesInfo.Subject[] subj = Business.Do<ISubject>().SubjectCount(Organ.Org_ID, null, true, 0, 0);
                this.Document.SetValue("subject", subj);
            }
            //课程列表           
            Tag testTag = this.Document.GetChildTagById("tests");
            if (testTag != null)
            {
                int sbjid = Common.Request.QueryString["sbj"].Int32 ?? -1;
                this.Document.SetValue("sbjid", sbjid);
                int size = int.Parse(testTag.Attributes.GetValue("size", "12"));
                int index = Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                EntitiesInfo.TestPaper[] test = Business.Do<ITestPaper>()
                    .PaperPager(Organ.Org_ID, sbjid, -1,-1, true, null, size, index, out sum);
    
                this.Document.SetValue("tests", test);
                this.Document.SetValue("path", Upload.Get["TestPaper"].Virtual);
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