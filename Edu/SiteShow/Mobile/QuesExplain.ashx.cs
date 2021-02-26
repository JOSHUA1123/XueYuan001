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
    /// 试题解析
    /// </summary>
    public class QuesExplain : BasePage
    {
        //试题id
        protected int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前试题
            EntitiesInfo.Questions qus = null;
            qus = Business.Do<IQuestions>().QuesSingle4Cache(id);
            if (qus == null) qus = Business.Do<IQuestions>().QuesSingle(id);
            if (qus != null)
            {
                if (!string.IsNullOrWhiteSpace(qus.Qus_Explain))
                {
                    qus.Qus_Explain = qus.Qus_Explain.Replace("\n","<br/>");
                }
                this.Document.SetValue("explain", qus.Qus_Explain);
            }    
        }
    }
}