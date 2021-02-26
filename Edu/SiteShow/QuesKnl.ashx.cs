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
    /// 试题的知识库
    /// </summary>
    public class QuesKnl : BasePage
    {
        int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前试题
            EntitiesInfo.Questions qus = Business.Do<IQuestions>().QuesSingle(id);
            if (qus != null)
            {
                EntitiesInfo.Knowledge knl = Business.Do<IKnowledge>().KnowledgeSingle(qus.Kn_ID);
                this.Document.Variables.SetValue("knl", knl);
            }
        }
    }
}