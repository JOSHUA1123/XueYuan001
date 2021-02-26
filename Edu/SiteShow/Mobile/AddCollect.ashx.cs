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
    /// 增加试题收藏
    /// </summary>
    public class AddCollect : IHttpHandler
    {
        //试题id
        protected int qid = Common.Request.QueryString["qid"].Int32 ?? 0;
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        //是否已经收藏，如果已经收藏，则删除
        protected bool IsCollect = Common.Request.QueryString["IsCollect"].Boolean ?? false;
        public void ProcessRequest(HttpContext context)
        {           
            if (!Extend.LoginState.Accounts.IsLogin)
            {
                return;
            }
            if (!IsCollect)
            {
                //如果不存在收藏，则添加
                EntitiesInfo.Student_Collect stc = new EntitiesInfo.Student_Collect();
                stc.Ac_ID = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
                stc.Qus_ID = qid;
                stc.Cou_ID = couid;
                Business.Do<IStudent>().CollectAdd(stc);
            }
            else
            {
                Business.Do<IStudent>().CollectDelete(qid, Extend.LoginState.Accounts.CurrentUser.Ac_ID);
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}