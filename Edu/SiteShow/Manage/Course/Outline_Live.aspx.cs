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
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;


namespace SiteShow.Manage.Course
{
    public partial class Outline_Live : Extend.CustomPage
    {
        //�½�ID
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)            
                initBind();   
        }
        /// <summary>
        /// ������
        /// </summary>
        protected void initBind()
        {
            EntitiesInfo.Outline mm =  Business.Do<IOutline>().OutlineSingle(id);
            //�Ƿ�Ϊֱ����
            cbIsLive.Checked = mm.Ol_IsLive;
            tbLiveTime.Text = mm.Ol_LiveTime < DateTime.Now.AddYears(-100) ? "" : mm.Ol_LiveTime.ToString("yyyy-MM-dd HH:mm");
            tbLiveSpan.Text = mm.Ol_LiveSpan == 0 ? "" : mm.Ol_LiveSpan.ToString();
            //ֱ������ַ
            pili_sdk.pili.Stream stream = null;
            try
            {
                stream = Business.Do<ILive>().StreamGet(mm.Ol_LiveID);
                if (stream != null)
                {
                    //������ַ
                    ltPublish.Text = string.Format("rtmp://{0}/{1}/{2}", stream.PublishRtmpHost, stream.HubName, stream.Title);
                    //ֱ����ַ
                    string proto = Business.Do<ILive>().GetProtocol;    //Э�飬http����https
                    ltPlayHls.Text = string.Format("{0}://{1}/{2}/{3}.m3u8", proto, stream.LiveHlsHost, stream.HubName, stream.Title);
                    ltPlayRtmp.Text = string.Format("rtmp://{0}/{1}/{2}", stream.LiveRtmpHost, stream.HubName, stream.Title);
                }
            }
            catch (Exception ex)
            {
                panelError.Visible = true;
                lbError.Text = "ֱ�����÷����쳣��" + ex.Message;
            }            

        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Outline ol = Business.Do<IOutline>().OutlineSingle(id);
            //�Ƿ�Ϊֱ��
            ol.Ol_IsLive = cbIsLive.Checked;
            DateTime timeLive = DateTime.Now;   //ֱ����ʼʱ��
            DateTime.TryParse(tbLiveTime.Text, out timeLive);
            ol.Ol_LiveTime = timeLive;  //
            int liveSpan = 0;       //ֱ���ƻ�ʱ��
            int.TryParse(tbLiveSpan.Text, out liveSpan);
            ol.Ol_LiveSpan = liveSpan;
            try
            {

                Business.Do<IOutline>().OutlineSave(ol);

                Master.AlertCloseAndRefresh("�������");
            }
            catch
            {
                throw;
            }
        }    
        
    }
}
