using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
namespace SiteShow.Ajax
{
    /// <summary>
    /// 添加分享积分
    /// </summary>
    public class AddSharePoint : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //分享人的id
            int sharekeyid = Common.Request.QueryString["sharekeyid"].Int32 ?? 1;
            //如果有登录的账户，则表示对方已经在访问了，不增加积分；
            if (Extend.LoginState.Accounts.IsLogin) return;

            //增加分享积分
            //获取推荐人
            EntitiesInfo.Accounts acc = Business.Do<IAccounts>().AccountsSingle(sharekeyid);
            if (acc == null) return;
            Business.Do<IAccounts>().PointAdd4Share(acc);    //增加分享积分
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