using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ServiceInterfaces;
using Extend;

namespace SiteShow.Mobile
{
    /// <summary>
    /// 教师评价
    /// </summary>
    public class TeacherComment : BasePage
    {
        int day = 1;    //多长时间（天数）内，不允许再评价
        //当前教师id
        private int thid = Common.Request.QueryString["thid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                if (Extend.LoginState.Accounts.IsLogin)
                {                   
                    int accid = Extend.LoginState.Accounts.UserID;
                    EntitiesInfo.TeacherComment cmt = Business.Do<ITeacher>().CommentSingle(thid, accid, day);
                    if (cmt != null)
                    {
                        //服务器端时间
                        //string time = new Common.Param.Method.ConvertToAnyValue(DateTime.Now.ToString()).JavascriptTime;
                        //string last = new Common.Param.Method.ConvertToAnyValue(cmt.Thc_CrtTime.AddDays(day).ToString()).JavascriptTime;
                        this.Document.Variables.SetValue("Time", Common.Server.getTime());
                        this.Document.Variables.SetValue("Last", Common.Server.getTime(cmt.Thc_CrtTime.AddDays(day)));
                        //最近一次评价
                        this.Document.SetValue("comment", cmt);
                    }
                }
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = Common.Request.Form["action"].String;
                switch (action)
                {
                    case "vcode":
                        vcode_verify(); //验证验证码                   
                        break;
                    case "submit":
                        submit();                  
                        break;
                }
                Response.End();
            }
        }
        /// <summary>
        /// 验证账号登录的校验吗
        /// </summary>
        private void vcode_verify()
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
        /// 接收评价信息
        /// </summary>
        private void submit()
        {
            string json = string.Empty;
            //评分、评价
            int num = Common.Request.Form["num"].Int32 ?? 0;
            string txt = Common.Request.Form["txt"].UrlDecode;
            //学员ID
            if (!Extend.LoginState.Accounts.IsLogin)
            {
                json = "{\"success\":\"0\",\"msg\":\"学员未登录！\"}";
                Response.Write(json);
            }
            else
            {
                int accid=Extend.LoginState.Accounts.UserID;
                //每天只允许评价一次
                int count = Business.Do<ITeacher>().CommentOfCount(thid, accid, day);
                if (count > 0)
                {
                    json = "{\"success\":\"0\",\"msg\":\"每天只能评价一次！\"}";
                }
                else
                {
                    EntitiesInfo.TeacherComment cmt = new EntitiesInfo.TeacherComment();
                    cmt.Thc_Score = num;
                    cmt.Thc_Comment = txt;
                    cmt.Th_ID = thid;
                    EntitiesInfo.Teacher th = Business.Do<ITeacher>().TeacherSingle(thid);
                    if (th != null) cmt.Th_Name = th.Th_Name;
                    cmt.Ac_ID = accid;
                    cmt.Thc_IsShow = true;
                    cmt.Thc_IsUse = true;
                    try
                    {
                        Business.Do<ITeacher>().CommentAdd(cmt);
                        json = "{\"success\":\"1\"}";
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message.Replace("\r","");
                        msg = msg.Replace("\n", "");
                        json = "{\"success\":\"0\",\"msg\":\"" + msg+ "\"}";
                    }
                }                
                Response.Write(json);
            }
        }  
    }
}