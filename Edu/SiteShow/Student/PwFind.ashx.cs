﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using VTemplate.Engine;

namespace SiteShow.Student
{
    /// <summary>
    /// 找回密码
    /// </summary>
    public class PwFind : BasePage
    {
        //账号，答案，密码
        string acc = Common.Request.Form["tbAcc"].String;
        protected string answer = Common.Request.Form["tbAnswer"].String;
        string pw = Common.Request.Form["tbNewPw"].String;
        //步骤
        int step = Common.Request.QueryString["step"].Int16 ?? 1;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("/default.ashx");
            this.Document.Variables.SetValue("step", step);
            //第一步验证账号是否存在
            if (step == 1)
            {
                if (string.IsNullOrWhiteSpace(acc)) return;
                EntitiesInfo.Accounts t = Business.Do<IAccounts>().IsAccountsExist(this.Organ.Org_ID, acc);
                if (t==null)
                {
                    //账号不存在
                    this.Response.Redirect(addPara(context.Request.Url.PathAndQuery, "error=4", "step=1", "acc=" + acc));
                }
                else
                {
                    this.Response.Redirect(addPara(context.Request.Url.PathAndQuery, "step=2", "acc=" + acc));
                }
            }
            //第二步，验证安全问题是否正确
            if (step == 2)
            {
                string acc = Common.Request.QueryString["acc"].String;
                EntitiesInfo.Accounts st = Business.Do<IAccounts>().AccountsSingle(acc, this.Organ.Org_ID);
                this.Document.Variables.SetValue("st", st);
                if (string.IsNullOrWhiteSpace(answer)) return;
                EntitiesInfo.Accounts t = Business.Do<IAccounts>().IsAccountsExist(this.Organ.Org_ID, acc, answer);
                if (t==null)
                {
                    //账号不存在
                    this.Response.Redirect(addPara(context.Request.Url.PathAndQuery, "error=6", "step=2", "acc=" + acc));
                }
                else
                {
                    int accid = t.Ac_ID;
                    string encrypt = Common.DataConvert.EncryptForBase64(accid.ToString());
                    encrypt = System.Web.HttpUtility.UrlEncode(encrypt);
                    HttpCookie cookie = new HttpCookie("findpw-acid", encrypt);
                    cookie.Expires = DateTime.Now.AddHours(10);
                    this.Response.Cookies.Add(cookie);
                    this.Response.Redirect(addPara(context.Request.Url.PathAndQuery, "step=3", "acc=" + acc));
                }
            }
            //第三步，设置新密码
            if (step == 3)
            {
                //string acc = Common.Request.QueryString["acc"].String;
                int accid = Common.Request.Cookies["findpw-acid"].Decrypt().Int32 ?? 0;
                EntitiesInfo.Accounts st = Business.Do<IAccounts>().AccountsSingle(accid);
                this.Document.Variables.SetValue("st", st);
                if (string.IsNullOrWhiteSpace(pw)) return;
                if (!isCodeImg())
                {
                    this.Response.Redirect(addPara(context.Request.Url.PathAndQuery, "step=3", "error=2", "acc=" + acc));
                }
                else
                {
                    st.Ac_Pw = pw;
                    st.Ac_Pw = new Common.Param.Method.ConvertToAnyValue(st.Ac_Pw).MD5;  
                    Business.Do<IAccounts>().AccountsSave(st);
                    this.Response.Redirect("/default.ashx");
                }
            }           
        }
        /// <summary>
        /// 验证图片验证是否正确
        /// </summary>
        /// <returns></returns>
        private bool isCodeImg()
        {
            string code = Common.Request.Form["tbCode"].String;
            //取图片验证码
            string imgCode = Common.Request.Cookies["stpwcode"].ParaValue;
            //取员工输入的验证码
            string userCode = new Common.Param.Method.ConvertToAnyValue(code).MD5;
            //验证
            return imgCode == userCode;
        }
        /// <summary>
        /// 增加地址的参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private string addPara(string url, params string[] para)
        {
            return Common.Request.Page.AddPara(url, para);
        }
    }
}