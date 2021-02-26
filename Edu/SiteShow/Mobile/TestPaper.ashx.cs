using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
using EntitiesInfo;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 试卷详情
    /// </summary>
    public class TestPaper : BasePage
    {
        //考试id
        protected int tpid = Common.Request.QueryString["id"].Int32 ?? 0;
        //课程id
        int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("couid", couid); 
            //当前试卷
            EntitiesInfo.TestPaper paper = null;            
            paper = Business.Do<ITestPaper>().PagerSingle(tpid);
            if (paper != null)
            {
                paper.Tp_Logo = string.IsNullOrWhiteSpace(paper.Tp_Logo) ? paper.Tp_Logo : Upload.Get["TestPaper"].Virtual + paper.Tp_Logo;
                //判断Logo是否存在
                string hylogo = Common.Server.MapPath(paper.Tp_Logo);
                if (!System.IO.File.Exists(hylogo)) paper.Tp_Logo = string.Empty;
                this.Document.SetValue("pager", paper);
                //试卷所属课程
                EntitiesInfo.Course course = Business.Do<ICourse>().CourseSingle(paper.Cou_ID);
                this.Document.SetValue("course", course);
            }
        }
    }
}