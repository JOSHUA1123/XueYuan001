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
    /// KnowledgesList 的摘要说明
    /// </summary>
    public class KnowledgesList : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前课程信息
            int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
            this.Document.SetValue("couid", couid);
            //课程列表           
            Tag knTag = this.Document.GetChildTagById("Knowledges");
            if (knTag != null)
            {
                //知识库分类
                int sort = Common.Request.QueryString["sort"].Int32 ?? -1;
                EntitiesInfo.KnowledgeSort ksort = Business.Do<IKnowledge>().SortSingle(sort);
                this.Document.SetValue("sort", ksort);
                int size = int.Parse(knTag.Attributes.GetValue("size", "12"));
                int len = int.Parse(knTag.Attributes.GetValue("lenth", "100"));
                int index = Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                EntitiesInfo.Knowledge[] knl = Business.Do<IKnowledge>().KnowledgePager(Organ.Org_ID, couid, sort, true,
                    null, null, null, "", size, index, out sum);
                foreach (EntitiesInfo.Knowledge k in knl)
                {
                    if (string.IsNullOrWhiteSpace(k.Kn_Intro))
                        k.Kn_Intro = Common.HTML.ClearTag(k.Kn_Details, len);
                    else
                        k.Kn_Intro = Common.HTML.ClearTag(k.Kn_Intro, len);
                }
                this.Document.SetValue("Knowledge", knl);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)size);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
                this.Document.SetValue("pageSize", size);
            }
        }
    }
}