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
    /// 知识库的详情
    /// </summary>
    public class Knowledge : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前知识
            int id = Common.Request.QueryString["id"].Int32 ?? 0;
            EntitiesInfo.Knowledge kn = Business.Do<IKnowledge>().KnowledgeSingle(id);
            this.Document.Variables.SetValue("kn", kn);
            if (kn != null)
            {
                EntitiesInfo.Course course = Business.Do<ICourse>().CourseSingle(kn.Cou_ID);
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                this.Document.Variables.SetValue("course", course);
                //是否购买课程（免费的也可以学习）
                bool isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, this.Account.Ac_ID);
                this.Document.Variables.SetValue("isBuy", isBuy);
            }

            //上一条
            EntitiesInfo.Knowledge prev = Business.Do<IKnowledge>().KnowledgePrev(kn.Cou_ID, -1, id);
            this.Document.Variables.SetValue("prev", prev);
            //下一条
            EntitiesInfo.Knowledge next = Business.Do<IKnowledge>().KnowledgeNext(kn.Cou_ID, -1, id);
            this.Document.Variables.SetValue("next", next);
        }
    }
}