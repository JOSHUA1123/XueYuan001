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
using System.IO;
using Common;
using ServiceInterfaces;
namespace SiteShow.Manage
{
    public partial class WeixinApi : System.Web.UI.Page
    {
        //Token
        private readonly string token = App.Get["WeixinToken"].String;
        protected void Page_Load(object sender, EventArgs e)
        {

            string postStr = "";

            if (Request.HttpMethod.ToLower() == "post")
            {
                System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = System.Text.Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr))
                {
                    //ResponseMsg(postStr);                    
                    Response.Write(ResponseMsg(postStr));
                    Response.End();
                }
                //WriteLog("postStr:" + postStr);
            }
            else
            {
                Valid();
            }
        }


        /// <summary>
        /// ��֤΢��ǩ��
        /// </summary>
        /// * ��token��timestamp��nonce�������������ֵ�������
        /// * �����������ַ���ƴ�ӳ�һ���ַ�������sha1����
        /// * �����߻�ü��ܺ���ַ�������signature�Աȣ���ʶ��������Դ��΢�š�
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = Common.Request.QueryString["signature"].String;
            string timestamp = Common.Request.QueryString["timestamp"].String;
            string nonce = Common.Request.QueryString["nonce"].String;
            string[] ArrTmp = { token, timestamp, nonce };
            //�ֵ�����
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr.ToLower() == signature;
        }


        private void Valid()
        {
            string echoStr = Common.Request.QueryString["echoStr"].String;
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Business.Do<ISystemPara>().Save("΢�Ų���", "ʱ�䣺" + DateTime.Now.ToString());
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }


        /// <summary>
        /// ������Ϣ���(΢����Ϣ����)
        /// </summary>
        /// <param name="weixinXML"></param>
        private string ResponseMsg(string weixinXML)
        {
            string tm = this.Server.UrlEncode(weixinXML);
            Business.Do<ISystemPara>().Save("΢����Ϣ����", tm);
            ///����д��ķ�����Ϣ����
            return "��˵ʲô��";
        }

        /// <summary>
        /// unixʱ��ת��Ϊdatetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetimeת��Ϊunixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>

        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>  
        /// ����������Ϣ  
        /// </summary>  
        /// <param name="wx">��ȡ���շ�����Ϣ</param>  
        /// <param name="content">Ц������</param>  
        /// <returns></returns>  
        private string sendTextMessage(string content)
        {
            string res = string.Format(@"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[{3}]]></Content>
                                <FuncFlag>0</FuncFlag>
                            </xml> ",
                content);
            return res;
        } 
        /// <summary>
        /// д��־(���ڸ���)
        /// </summary>
        private void WriteLog(string strMemo)
        {
            string filename = Server.MapPath("/logs/log.txt");
            if (!Directory.Exists(Server.MapPath("//logs//")))
                Directory.CreateDirectory("//logs//");
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }
                sr.WriteLine(strMemo);
            }
            catch { }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }
}
