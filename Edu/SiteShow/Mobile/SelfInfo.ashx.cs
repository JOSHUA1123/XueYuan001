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
    /// 学员个人信息
    /// </summary>
    public class SelfInfo : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //下级会员数量
                int subcount = Business.Do<IAccounts>().SubordinatesCount(this.Account.Ac_ID, true);
                this.Document.Variables.SetValue("subcount", subcount);
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = Common.Request.Form["action"].String;
                switch (action)
                {
                    case "birthday": modify_birthday();
                        break;
                    case "name": modify_name();
                        break;
                    case "sex": modify_sex();
                        break;
                    case "link": modify_link();
                        break;
                    case "safe": modify_safe();
                        break;
                    case "sign": modify_sign();
                        break;
                }
            }           
        }
        //修改学员姓名
        private void modify_name()
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            st.Ac_Name = Common.Request.Form["stname"].String;          
            Business.Do<IAccounts>().AccountsSave(st);
        }
        /// <summary>
        /// 修改学员生日
        /// </summary>
        private void modify_birthday()
        {
            DateTime date = Common.Request.Form["date"].DateTime ?? DateTime.Now;
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            st.Ac_Birthday = date;
            st.Ac_Age = (int)((DateTime.Now - date).TotalDays / (365 * 3 + 366) * 4);
            Business.Do<IAccounts>().AccountsSave(st);
        }
        /// <summary>
        /// 修改学员性别
        /// </summary>
        private void modify_sex()
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            st.Ac_Sex = Common.Request.Form["sex"].Int16 ?? 0;
            Business.Do<IAccounts>().AccountsSave(st);
        }
        /// <summary>
        /// 修改联系方式
        /// </summary>
        private void modify_link()
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            st.Ac_MobiTel1 = Common.Request.Form["mobi"].String;
            st.Ac_Qq = Common.Request.Form["qq"].String;
            st.Ac_Email = Common.Request.Form["email"].String;
            Business.Do<IAccounts>().AccountsSave(st);
        }
        /// <summary>
        /// 修改安全问题
        /// </summary>
        private void modify_safe()
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            st.Ac_Qus = Common.Request.Form["ques"].String;
            st.Ac_Ans = Common.Request.Form["answer"].String;            
            Business.Do<IAccounts>().AccountsSave(st);
        }
        /// <summary>
        /// 修改个人签名
        /// </summary>
        private void modify_sign()
        {
            EntitiesInfo.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            st.Ac_Signature = Common.Request.Form["sign"].String;           
            Business.Do<IAccounts>().AccountsSave(st);
        }
    }
}