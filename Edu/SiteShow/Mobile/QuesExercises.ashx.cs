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
    /// 试题练习
    /// </summary>
    public class QuesExercises : BasePage
    {
        //章节id
        protected int olid = Common.Request.QueryString["olid"].Int32 ?? 0;
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        //试题的启始索引与当前取多少条记录       
        protected int count = Common.Request.QueryString["count"].Int32 ?? 100;       
        protected override void InitPageTemplate(HttpContext context)
        {
            //题型
            this.Document.SetValue("quesType", Common.App.Get["QuesType"].Split(','));
            this.Document.SetValue("couid", couid);            
        }
    }
}