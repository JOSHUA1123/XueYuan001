using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;

namespace SiteShow.Manage.SMS1
{
    public partial class SMSSetup : Extend.CustomPage
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = Common.Request.Form["action"].String;
                switch (action)
                {
                    case "getnumber":
                        GetNumber();
                        break;                    
                }
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            string smsCurr=Business.Do<ISystemPara>().GetValue("SmsCurrent");
            SMS.Config.SetCurrent(smsCurr);
            //获取账号
            for (int i = 0; i < SMS.Config.SmsItems.Length; i++)
            {
                string smsacc = Business.Do<ISystemPara>().GetValue(SMS.Config.SmsItems[i].Remarks + "SmsAcc");
                SMS.Config.SmsItems[i].User = smsacc;
            }
            GridView1.DataSource = SMS.Config.SmsItems;
            GridView1.DataKeyNames = new string[] { "Remarks" };
            GridView1.DataBind();             
        }
        /// <summary>
        /// 设置短信平台
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void linkBtn_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            string remark = lb.CommandArgument;
            Business.Do<ISystemPara>().Save("SmsCurrent", remark);
            SMS.Config.SetCurrent(remark);
            //
            GridView1.DataSource = SMS.Config.SmsItems;
            GridView1.DataBind(); 
        }
        /// <summary>
        /// 获取短信条数
        /// </summary>
        /// <param name="marks"></param>
        /// <returns></returns>
        protected void GetNumber()
        {
            string marks = Common.Request.Form["marks"].String;
            int num = -1;
            try
            {
                //账号与密码
                string smsacc = Business.Do<ISystemPara>().GetValue(marks + "SmsAcc");
                string smspw = Business.Do<ISystemPara>().GetValue(marks + "SmsPw");
                if (!string.IsNullOrWhiteSpace(smspw))
                {
                    smspw = Common.DataConvert.DecryptForBase64(smspw);    //将密码解密
                    //短信平台操作对象
                    SMS.ISMS sms = SMS.Gatway.GetService(marks);
                    //设置账号与密码
                    sms.Current.User = smsacc;
                    sms.Current.Password = smspw;
                    num = sms.Query();
                    string json = "{\"num\":" + num + ",\"marks\":\"" + marks + "\"}";
                    Response.Write(json);
                }
                else
                {
                    throw new Exception("未填写密码！"); 
                }
            }
            catch (Exception ex)
            {
                num = -1;
                string json = "{\"num\":" + num + ",\"marks\":\"" + marks + "\",\"desc\":\"" + ex.Message + "\"}";
                Response.Write(json);
            }
            Response.End();
        }

    }
}
