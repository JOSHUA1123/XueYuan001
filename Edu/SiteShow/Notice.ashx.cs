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
    /// Notice 的摘要说明
    /// </summary>
    public class Notice : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {           
            //通知
            int noid = Common.Request.QueryString["id"].Int32 ?? 0;
            EntitiesInfo.Notice notice = Business.Do<INotice>().NoticeSingle(noid);
            if (notice == null) return;
            if (notice.No_IsShow && ((DateTime)notice.No_StartTime) < DateTime.Now) 
                this.Document.Variables.SetValue("notice", notice);
            //当前通知的上一条
            EntitiesInfo.Notice prev = Business.Do<INotice>().NoticePrev(noid, notice.Org_ID);
            this.Document.Variables.SetValue("prev", prev);
            //当前通知的下一条
            EntitiesInfo.Notice next = Business.Do<INotice>().NoticeNext(noid, notice.Org_ID);
            this.Document.Variables.SetValue("next", next);
        }
    }
}