using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extend;
using Common;
using ServiceInterfaces;

namespace SiteShow.Teacher
{
    /// <summary>
    /// 教师管理的界面
    /// </summary>
    public class Panel : BasePage
    {       
        protected override void InitPageTemplate(HttpContext context)
        {
            EntitiesInfo.Teacher teacher = this.Teacher;
            if (teacher == null)
            {
                context.Response.Redirect("index.ashx");
                return;
            }
            this.Document.Variables.SetValue("CurrUser", teacher);

            //是否启用或能过审核
            bool isEnable = teacher.Th_IsPass && teacher.Th_IsUse;
            this.Document.Variables.SetValue("isEnable", isEnable);
        }
    }
}