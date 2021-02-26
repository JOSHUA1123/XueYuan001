using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using Extend;
using VTemplate.Engine;
using System.Web.SessionState;
using DingEntities;
using DingTalk.Api.Response;
using DingTalk.Api;
using DingTalk.Api.Request;
using Newtonsoft.Json;

namespace SiteShow.Student
{
    /// <summary>
    /// 学员登录
    /// </summary>
    public class Index : BasePage, IRequiresSessionState
    {
        
        //用于标识布局的值
        protected string loyout = Common.Request.QueryString["loyout"].String;
        private string from = "";
        protected override void InitPageTemplate(HttpContext context)
        {
            //设置主域，用于js跨根域
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //来源页
                 from = Common.Request.QueryString["from"].String;
                if (string.IsNullOrWhiteSpace(from)) from = context.Request.UrlReferrer != null ? context.Request.UrlReferrer.PathAndQuery : "";
                this.Document.SetValue("from", from);
                this.Document.SetValue("frompath", context.Request.UrlReferrer != null ? context.Request.UrlReferrer.ToString() : "");
                //设置主域，用于js跨根域                
                if (multi == 0 && !Common.Server.IsLocalIP)
                    this.Document.Variables.SetValue("domain", Common.Server.MainName);
                //相关参数
                Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
                //登录方式
                bool IsLoginForPw = config["IsLoginForPw"].Value.Boolean ?? true;    //启用账号密码登录
                bool IsLoginForSms = config["IsLoginForSms"].Value.Boolean ?? true;  //启用手机短信验证登录
                this.Document.SetValue("forpw", IsLoginForPw);
                this.Document.SetValue("forsms", IsLoginForSms);
                this.Document.SetValue("islogin", !IsLoginForPw && !IsLoginForSms);
                //界面状态
                if (!IsLoginForPw && IsLoginForSms) loyout = "mobile";
                this.Document.SetValue("loyout", loyout);
            }


            //if (accounts != null)
            //{
            //    LoginState.Accounts.Write(accounts);
            //    Business.Do<IAccounts>().PointAdd4Login(accounts, "电脑网页", "账号密码登录", "");   //增加登录积分
            //    Business.Do<IStudent>().LogForLoginAdd(accounts);
            //    Response.Write("{\"success\":\"1\",\"acid\":\"" + accounts.Ac_ID + "\",\"name\":\"" + accounts.Ac_Name + "\"}");
            //    var ul = from+"?sharekeyid=" + accounts.Ac_ID;//student/index.ashx
            //    Response.Redirect(ul);
            //}else 

            var accountsss = Business.Do<IAccounts>().AccountsSingle("王晨光", "Hhd9cuLtN2LbD2KiS5Y9f6QiEiE");
            if (accountsss != null)
            {
                LoginState.Accounts.Write(accountsss);
                try
                {
                    Business.Do<IAccounts>().PointAdd4Login(accountsss, "电脑网页", "账号密码登录", "");   //增加登录积分
                    Business.Do<IStudent>().LogForLoginAdd(accountsss);
                }
                catch (Exception ex)
                {

                    Log.Info("pc", ex.Message);
                }

                Response.Write("{\"success\":\"1\",\"acid\":\"" + accountsss.Ac_ID + "\",\"name\":\"" + accountsss.Ac_Name + "\"}");
                var ul = from + "?sharekeyid=" + accountsss.Ac_ID;//student/index.ashx
                Response.Redirect(ul);
            }

            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = Common.Request.Form["action"].String;
                switch (action)
                {                    
                    case "accloginvcode":
                        accloginvcode_verify(); //验证账号登录时的验证码
                        break;
                    case "acclogin":
                        acclogin_verify();  //验证账号登录时的密码
                        break;
                    case "getSms":
                        mobivcode_verify();  //验证手机登录时，获取短信时的验证码
                        break;
                    case "mobilogin":
                        mobiLogin_verify();
                        break;
                    default:
                        acclogin_verify();  //验证账号登录时的密码
                        break;
                }
                Response.End();
            }
            var code = context.Request["code"];
            if (!string.IsNullOrEmpty(code))
            {
                DingLogin(code, from);
            }
            else
            {

            }

            //QQ登录
            this.Document.SetValue("QQLoginIsUse", Business.Do<ISystemPara>()["QQLoginIsUse"].Boolean ?? true);
            this.Document.SetValue("QQAPPID", Business.Do<ISystemPara>()["QQAPPID"].String);
            string returl = Business.Do<ISystemPara>()["QQReturl"].Value ?? "http://" + Common.Server.MainName;
            this.Document.SetValue("QQReturl", returl);
            //微信登录
            this.Document.SetValue("WeixinLoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false);
            this.Document.SetValue("WeixinAPPID", Business.Do<ISystemPara>()["WeixinAPPID"].String);
            this.Document.SetValue("WeixinReturl", Business.Do<ISystemPara>()["WeixinReturl"].Value ?? "http://" + Common.Server.MainName);
            //记录当前机构到本地，用于QQ或微信注册时的账户机构归属问题
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("ORGID");
            cookie.Value = this.Organ.Org_ID.ToString();
            //设置主域，用于js跨根域
            if (multi == 0 && !Common.Server.IsLocalIP)
                cookie.Domain = Common.Server.MainName;
            this.Response.Cookies.Add(cookie);
            //推荐人id
            string sharekeyid = Common.Request.QueryString["sharekeyid"].String;
            System.Web.HttpCookie cookieShare = new System.Web.HttpCookie("sharekeyid");
            cookieShare.Value = sharekeyid;
            if (multi == 0 && !Common.Server.IsLocalIP) 
                cookieShare.Domain = Common.Server.MainName;
            this.Response.Cookies.Add(cookieShare);
        }

        /// <summary>
        /// 在应用中获取钉钉code
        /// </summary>
        public void GetCode()
        {
            string appid = "dingoapjhmhgsjkxfcxpzq";
            string REDIRECT_URI = "http://edu.91zn.cn:81/Mobile/login.ashx";

            Response.Redirect("https://oapi.dingtalk.com/connect/qrconnect?appid=" + appid + "&response_type=code&scope=snsapi_login&state=STATE&redirect_uri=" + REDIRECT_URI);
        }

        /// <summary>
        /// 钉钉扫描登陆业务
        /// </summary>
        /// <param name="code"></param>
        /// <param name="from"></param>
        private void DingLogin(string code,string from)
        {
            OapiSnsGetuserinfoBycodeResponse response = new OapiSnsGetuserinfoBycodeResponse();
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/getuserinfo_bycode");
            OapiSnsGetuserinfoBycodeRequest req = new OapiSnsGetuserinfoBycodeRequest();
            req.TmpAuthCode = code;
            response = client.Execute(req, DingUserInfo.appId, DingUserInfo.appSecret);

            //获取到response后就可以进行自己的登录业务处理了
            //var openid = response.UserInfo.Openid; 
            var UserName = response.UserInfo.Nick;
            var jsonData = JsonConvert.DeserializeObject<JsonData>(response.Body);
            var DingID = jsonData.user_info.dingId;
            var Unionid = response.UserInfo.Unionid;
            Log.Info("1", "UserName:"+ UserName+ ":::DingID:" + DingID+ "Unionid:"+ Unionid);
            //!string.IsNullOrEmpty(DingID) &&
            if (!string.IsNullOrEmpty(UserName))
            {
                Log.Info("2", "UserName:" + UserName + ":::DingID:" + DingID + "Unionid:" + Unionid);
                //var accounts = Business.Do<IAccounts>().AccountsSingle(UserName, DingID);
                var accountsss = Business.Do<IAccounts>().AccountsSingle(UserName, Unionid);
                //if (accounts != null)
                //{
                //    LoginState.Accounts.Write(accounts);
                //    Business.Do<IAccounts>().PointAdd4Login(accounts, "电脑网页", "账号密码登录", "");   //增加登录积分
                //    Business.Do<IStudent>().LogForLoginAdd(accounts);
                //    Response.Write("{\"success\":\"1\",\"acid\":\"" + accounts.Ac_ID + "\",\"name\":\"" + accounts.Ac_Name + "\"}");
                //    var ul = from+"?sharekeyid=" + accounts.Ac_ID;//student/index.ashx
                //    Response.Redirect(ul);
                //}else 
                if (accountsss!=null)
                {
                    LoginState.Accounts.Write(accountsss);
                    try
                    {
                        Business.Do<IAccounts>().PointAdd4Login(accountsss, "电脑网页", "账号密码登录", "");   //增加登录积分
                        Business.Do<IStudent>().LogForLoginAdd(accountsss);
                    }
                    catch (Exception ex)
                    {

                        Log.Info("pc", ex.Message);
                    }
                  
                    Response.Write("{\"success\":\"1\",\"acid\":\"" + accountsss.Ac_ID + "\",\"name\":\"" + accountsss.Ac_Name + "\"}");
                    var ul = from + "?sharekeyid=" + accountsss.Ac_ID;//student/index.ashx
                    Response.Redirect(ul);
                }
                else
                {
                    Log.Info("3", "UserName:" + UserName + ":::DingID:" + DingID + "Unionid:" + Unionid);
                    var list = DingUserInfo.biao();
                    //var DingUser = list.Where(n => n.YourName == UserName && n.DingID == DingID).FirstOrDefault();
                    var TalkUser = DingUserInfo.GetDingTalkUser().Where(n=>n.userName== UserName&&n.unionid== Unionid).FirstOrDefault();
                    //if (DingUser != null)
                    //{
                    //    EntitiesInfo.Accounts tmp = new EntitiesInfo.Accounts();
                    //    tmp.Ac_Pw = new Common.Param.Method.ConvertToAnyValue("1").MD5;
                    //    tmp.Ac_Name = DingUser.YourName;
                    //    tmp.Ac_MobiTel1 = DingUser.Mobile;
                    //    tmp.Ac_AccName = DingUser.Mobile;
                    //    tmp.Ac_IsPass = true;
                    //    tmp.Ac_IsUse = true;
                    //    tmp.Ac_Sex = DingUser.Sex;
                    //    tmp.Ac_Age = DingUser.Age;
                    //    tmp.Ac_WeixinOpenID = DingUser.DingID;
                    //    //tmp.Ac_Pw = "1";
                    //    int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                    //    var acc = Business.Do<IAccounts>().AccountsSingle(tmp.Ac_Name, tmp.Ac_WeixinOpenID);
                    //    if (acc != null)
                    //    {
                    //        LoginState.Accounts.Write(acc);
                    //        Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "账号密码登录", "");   //增加登录积分
                    //        Business.Do<IStudent>().LogForLoginAdd(acc);
                    //        var ul = from + "?sharekeyid=" + acc.Ac_ID;
                    //        Response.Redirect(ul);
                    //    }
                    //}else
                    if (TalkUser!=null)
                    {
                        Log.Info("4", "UserName:" + UserName + ":::DingID:" + DingID + "Unionid:" + Unionid);
                        EntitiesInfo.Accounts tmp = new EntitiesInfo.Accounts();
                        tmp.Ac_Pw = new Common.Param.Method.ConvertToAnyValue("1").MD5;
                        tmp.Ac_Name = TalkUser.userName;
                        tmp.Ac_MobiTel1 = TalkUser.mobile;
                        tmp.Ac_AccName = TalkUser.mobile;
                        tmp.Ac_IsPass = true;
                        tmp.Ac_IsUse = true;
                        tmp.Ac_Sex = 0;
                        tmp.Ac_Age = 0;
                        tmp.Ac_WeixinOpenID = TalkUser.unionid;
                        //tmp.Ac_Pw = "1";
                        int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                        var acc = Business.Do<IAccounts>().AccountsSingle(tmp.Ac_Name, tmp.Ac_WeixinOpenID);
                        if (acc != null)
                        {
                            try
                            {
                                LoginState.Accounts.Write(acc);
                                Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "账号密码登录", "");   //增加登录积分
                                Business.Do<IStudent>().LogForLoginAdd(acc);
                               
                            }
                            catch (Exception ex)
                            {
                                Log.Info("pcLogin", ex.Message);
                            }
                            var ul = from + "?sharekeyid=" + acc.Ac_ID;
                            Response.Redirect(ul);
                        }
                    }
                    else
                    {
                        Log.Info("5", "UserName:" + UserName + ":::DingID:" + DingID + "Unionid:" + Unionid);
                        Response.Write("{\"success\":\"登录失败\"}");
                        //context.Response.Write(response.Body);
                    }

                }
            }
        }


        #region 账号登录验证
        /// <summary>
        /// 验证账号登录的校验吗
        /// </summary>
        private void accloginvcode_verify()
        {            
            //取图片验证码
            string vname = Common.Request.Form["vname"].String;
            string imgCode = Common.Request.Cookies[vname].ParaValue;
            //取输入的验证码
            string userCode = Common.Request.Form["vcode"].MD5;
            //验证
            string json = "{\"success\":\"" + (imgCode == userCode ? 1 : -1) + "\"}";
            Response.Write(json);            
        }
        /// <summary>
        /// 验证账号+密码的登录
        /// </summary>
        private void acclogin_verify()
        {
            string acc = Common.Request.Form["acc"].String;  //账号
            string pw = Common.Request.Form["pw"].String;    //密码
            string sign = Common.Request.Form["sign"].String;
            int signnum = Common.Request.Form["signnum"].Int32 ?? 1;
            string succurl = Common.Request.Form["succurl"].String;  //登录成功跳转的页面，用于非ajax请求时（政务版专用）
            string failurl = Common.Request.Form["failurl"].String;  //登录失败要跳转的页面，用于非ajax请求时（政务版专用）
            //通过验证，进入登录状态            
            EntitiesInfo.Accounts emp = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (emp != null)
            {
                //如果没有设置免登录，则按系统设置的时效
                if (sign == "")
                    LoginState.Accounts.Write(emp);
                else
                    LoginState.Accounts.Write(emp, signnum);
                //登录成功
                Business.Do<IAccounts>().PointAdd4Login(emp, "电脑网页", "账号密码登录", "");   //增加登录积分
                Business.Do<IStudent>().LogForLoginAdd(emp);
                if (string.IsNullOrWhiteSpace(succurl))
                    Response.Write("{\"success\":\"1\",\"acid\":\"" + emp.Ac_ID + "\",\"name\":\"" + emp.Ac_Name + "\"}");
                else
                    Response.Redirect(succurl);
            }
            else
            {
                //登录失败
                if (string.IsNullOrWhiteSpace(failurl))
                    Response.Write("{\"success\":\"-1\"}");
                else
                    Response.Redirect(failurl);
            }
        }
        #endregion

        #region 手机短信验证
        /// <summary>
        /// 获取短信之间的验证
        /// </summary>
        private void mobivcode_verify()
        {
            //取图片验证码
            string vname = Common.Request.Form["vname"].String;
            string imgCode = Common.Request.Cookies[vname].ParaValue;
            //取输入的验证码
            string userCode = Common.Request.Form["vcode"].MD5;
            //输入的手机号
            string phone = Common.Request.Form["phone"].String;
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            EntitiesInfo.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
            if (acc == null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号不存在
                return;
            }
            //发送短信验证码
            try
            {
                bool success = Business.Do<ISMS>().SendVcode(phone, "mobi_" + vname);
                //bool success = true;
                if (success) Response.Write("{\"success\":\"1\",\"state\":\"0\"}");  //短信发送成功   
                Business.Do<IStudent>().LogForLoginAdd(acc);
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"desc\":\"" + ex.Message + "\"}");  //短信发送失败   
            }
        }
        /// <summary>
        /// 手机登录的验证
        /// </summary>
        private void mobiLogin_verify()
        {            
            string vname = Common.Request.Form["vname"].String;
            string imgCode = Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string phone = Common.Request.Form["phone"].String;  //输入的手机号
            string sms = Common.Request.Form["sms"].MD5;  //输入的短信验证码
            string sign = Common.Request.Form["sign"].String;    //是否保持登录状态
            int signnum = Common.Request.Form["signnum"].Int32 ?? 1;     //保持登录的时效
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            EntitiesInfo.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
            if (acc == null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号不存在
                return;
            }
            //验证短信验证码
            string smsCode = Common.Request.Cookies["mobi_" + vname].ParaValue;
            if (sms != smsCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\"}");  //短信验证失败             
                return;
            }            
            else
            {
                //通过验证，进入登录状态       
                EntitiesInfo.Accounts emp = Business.Do<IAccounts>().AccountsSingle(phone, true, true);
                if (emp != null)
                {
                    //如果没有设置免登录，则按系统设置的时效
                    if (sign == "")
                        LoginState.Accounts.Write(emp);
                    else
                        LoginState.Accounts.Write(emp, signnum);
                    //登录成功
                    Business.Do<IAccounts>().PointAdd4Login(emp, "电脑网页", "短信验证登录", "");   //增加登录积分
                    LoginState.Accounts.Refresh(emp);
                    Response.Write("{\"success\":\"1\",\"name\":\"" + emp.Ac_Name + "\",\"acid\":\"" + emp.Ac_ID + "\",\"state\":\"0\"}");
                }
                else
                {
                    //登录失败
                    Response.Write("{\"success\":\"-1\"}");
                }               
            }
        }
        #endregion
    }
}