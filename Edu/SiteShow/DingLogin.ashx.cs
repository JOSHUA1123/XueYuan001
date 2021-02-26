using Common;
using DingEntities;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using Extend;
using Newtonsoft.Json;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteShow
{
    /// <summary>
    /// DingLogin 的摘要说明
    /// </summary>
    public class DingLogin : BasePage
    {

        private string appId = "dingoapjhmhgsjkxfcxpzq";
        private string appSecret = "15aEFVfLMrbCZViJI-e6xW5FmOiAVT7vLUc-wjvQc19WKzQ642HVNzY51w_UQVuu";

        protected override void InitPageTemplate(HttpContext context)
        {
            var code = context.Request["code"];
            OapiSnsGetuserinfoBycodeResponse response = new OapiSnsGetuserinfoBycodeResponse();
          
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/getuserinfo_bycode");
                OapiSnsGetuserinfoBycodeRequest req = new OapiSnsGetuserinfoBycodeRequest();
            
                req.TmpAuthCode = code;
                response = client.Execute(req, appId, appSecret);

                //获取到response后就可以进行自己的登录业务处理了
                //var openid = response.UserInfo.Openid; 
                var UserName = response.UserInfo.Nick;
                var jsonData = JsonConvert.DeserializeObject<JsonData>(response.Body);

                var DingID = jsonData.user_info.dingId;



            if (!string.IsNullOrEmpty(DingID) && !string.IsNullOrEmpty(UserName))
                {
                    var accounts = Business.Do<IAccounts>().AccountsSingle(UserName, DingID);
                    if (accounts != null)
                    {
                        LoginState.Accounts.Write(accounts);
                        Business.Do<IAccounts>().PointAdd4Login(accounts, "电脑网页", "账号密码登录", "");   //增加登录积分
                        Business.Do<IStudent>().LogForLoginAdd(accounts);
                     context.Response.Write("{\"success\":\"1\",\"acid\":\"" + accounts.Ac_ID + "\",\"name\":\"" + accounts.Ac_Name + "\"}");
                    //this.Document.Variables.SetValue("Ac_AccName", accounts.Ac_AccName);
                    //this.Document.Variables.SetValue("Ac_pw", accounts.Ac_Pw);
                    //this.Document.Variables.SetValue("success", "1");   //登录成功
                    ////var ul = "Default.ashx?sharekeyid=" + accounts.Ac_ID;student/index.ashx
                    //var ul = "student/index.ashx";
                    //var url = context.Request.Url.Host;
                    //url = "http://" + url + ":81/" + ul;
                    //context.Response.Redirect(url);

                }
                    else
                    {
                        var list = DingUserInfo.biao();
                        var DingUser = list.Where(n => n.YourName == UserName && n.DingID == DingID).FirstOrDefault();
                        if (DingUser != null)
                        {
                            EntitiesInfo.Accounts tmp = new EntitiesInfo.Accounts();
                            tmp.Ac_Pw = DingUser.DingID;
                            tmp.Ac_Name = DingUser.YourName;
                            tmp.Ac_MobiTel1 = DingUser.Mobile;
                            tmp.Ac_AccName = DingUser.Mobile;
                            tmp.Ac_IsPass = true;
                            tmp.Ac_IsUse = true;
                            tmp.Ac_Sex = DingUser.Sex;
                            tmp.Ac_Age = DingUser.Age;
                            //tmp.Ac_Pw = "1";
                            int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                            var acc = Business.Do<IAccounts>().AccountsSingle(tmp.Ac_Name, tmp.Ac_Pw);
                            if (acc != null)
                            {
                                LoginState.Accounts.Write(acc);
                                context.Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\"}");
                            }
                        }
                        else
                        {
                        context.Response.Write("{\"success\":\"登录失败\"}");
                        //context.Response.Write(response.Body);
                    }


                    }
                }


              
          
            //context.Response.Write(response.Body);
            context.Response.End();
        }

        //public bool IsReusable
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
    }
}