﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;
namespace SiteShow
{
    /// <summary>
    /// 知识库详情页
    /// </summary>
    public class Knowledge : BasePage
    {
        //知识库的id
        int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            
            //知识       
            EntitiesInfo.Knowledge knl = Business.Do<IKnowledge>().KnowledgeSingle(id);            
            this.Document.Variables.SetValue("knl", knl);
            if (knl != null)
            {
                EntitiesInfo.Course course = Business.Do<ICourse>().CourseSingle(knl.Cou_ID);
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                this.Document.Variables.SetValue("course", course);
                //是否购买课程（免费的也可以学习）
                int acid = this.Account != null ? this.Account.Ac_ID : 0;
                bool isBuy = acid > 0 && (course.Cou_IsFree || course.Cou_IsLimitFree) ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, acid);
                this.Document.Variables.SetValue("isBuy", isBuy);
                //上级专业
                if (course != null)
                {
                    List<EntitiesInfo.Subject> sbjs = Business.Do<ISubject>().Parents(course.Sbj_ID, true);
                    this.Document.Variables.SetValue("sbjs", sbjs);
                }
            }
        }
    }
}