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


namespace SiteShow.Manage.Course
{
    public partial class OutlineEvent_Edit : Extend.CustomPage
    {
        //�½��¼�ID
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        //�����γ̵�ID
        private int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        //�����½ڵ�ID
        private int olid = Common.Request.QueryString["olid"].Int32 ?? 0;
        //�����½ڵ�UID
        private string uid = Common.Request.QueryString["uid"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {               
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind(int type)
        {
            type = type <= 1 ? 1 : type;
            //����ҳ����
            ContentPlaceHolder cpl1 = (ContentPlaceHolder)Master.FindControl("cphMain");  
            for (int i = 1; i <= 4; i++)
            {
                //System.Web.UI.WebControls.contr
                System.Web.UI.WebControls.Panel p = (System.Web.UI.WebControls.Panel)cpl1.FindControl("Panel" + i);
                if (p == null) continue;
                p.Visible = i == type;
            }
        }
        protected void tblTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int type;
            int.TryParse(rblTypes.SelectedValue, out type);
            InitBind(type);
        }
        private void fill()
        {
            EntitiesInfo.OutlineEvent obj = id < 1 ? new EntitiesInfo.OutlineEvent() : Business.Do<IOutline>().EventSingle(id);
            if (id < 1)
            {
                InitBind(1);
                setEventQues(id);
                setEventFeedback(id);
            }
            if (id > 0 && obj != null)
            {
                InitBind(obj.Oe_EventType);
                rblTypes.Enabled = false;
                //������Ϣ
                tbTitle.Text = obj.Oe_Title;
                cbIsUse.Checked = obj.Oe_IsUse;
                tbWidth.Text = obj.Oe_Width.ToString();
                tbHeight.Text = obj.Oe_Height.ToString();
                tbPoint.Text = obj.Oe_TriggerPoint.ToString();
                ListItem liType = rblTypes.Items.FindByValue(obj.Oe_EventType.ToString());
                if (liType != null)
                {
                    rblTypes.SelectedIndex = -1;
                    liType.Selected = true;
                }
            }
            //����ǡ����ѡ�
            if (obj.Oe_EventType == 1)
            {
                tbContext1.Text = obj.Oe_Context;
            }
            //����ǡ�֪ʶչʾ��
            if (obj.Oe_EventType == 2)
            {
                tbContext2.Text = obj.Oe_Context;
            }
            //����ǡ��γ����ʡ�
            if (obj.Oe_EventType == 3)
            {
                //�������
                tbQuesTit.Text = obj.Oe_Context;
                //����ѡ��
                setEventQues(id);
                //�����
                //tbAnswer.Text = obj.Oe_Answer;
            }
            //����ǡ�ʵʱ������
            if (obj.Oe_EventType == 4)
            {
                tbQuesTit4.Text = obj.Oe_Context;
                setEventFeedback(id);
            }           
           
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.OutlineEvent obj = id < 1 ? new EntitiesInfo.OutlineEvent() : Business.Do<IOutline>().EventSingle(id);
            if (obj == null) return;
            //������Ϣ
            obj.Cou_ID = couid;
            obj.Ol_ID = olid;
            obj.Ol_UID = uid;
            obj.Oe_Title = tbTitle.Text.Trim();
            obj.Oe_IsUse = cbIsUse.Checked;
            int width, height, point, type;
            int.TryParse(tbWidth.Text, out width);
            int.TryParse(tbHeight.Text, out height);
            int.TryParse(tbPoint.Text, out point);
            int.TryParse(rblTypes.SelectedValue, out type);
            obj.Oe_Width = width;
            obj.Oe_Height = height;
            obj.Oe_TriggerPoint = point;
            obj.Oe_EventType = type;
            //����ǡ����ѡ�
            if (obj.Oe_EventType == 1)
            {
                if (tbContext1.Text.Trim().Length > 300)
                    tbContext1.Text = tbContext1.Text.Substring(0, 300);
                obj.Oe_Context = tbContext1.Text.Trim();
            }
            //����ǡ�֪ʶչʾ��
            if (obj.Oe_EventType == 2)
            {
                obj.Oe_Context = tbContext2.Text;
            }
            //����ǡ��γ����ʡ�
            if (obj.Oe_EventType == 3)
            {
                //�������
                obj.Oe_Context = tbQuesTit.Text.Trim();
                DataTable dt = getEventQues();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                StringWriter sw = new StringWriter();
                xmlSerial.Serialize(sw, dt); // ���л�table
                obj.Oe_Datatable = sw.ToString();
                //�����
                //obj.Oe_Answer = tbAnswer.Text.Trim();
            }
            //����ǡ�ʵʱ������
            if (obj.Oe_EventType == 4)
            {
                obj.Oe_Context = tbQuesTit4.Text.Trim();
                DataTable dt = getEventFeedback();
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                StringWriter sw = new StringWriter();
                xmlSerial.Serialize(sw, dt); // ���л�table
                obj.Oe_Datatable = sw.ToString();
            }
            try
            {
                if (id < 1)
                {
                    //����
                    Business.Do<IOutline>().EventAdd(obj);
                }
                else
                {
                    Business.Do<IOutline>().EventSave(obj);
                }
                Master.AlertCloseAndRefresh("�����ɹ�");
            }
            catch
            {
                throw;
            }

        }

        #region ˽�з���
        /// <summary>
        /// ���������ѡ������
        /// </summary>
        /// <returns></returns>
        private void setEventQues(int oeid)
        {
            DataTable dt = Business.Do<IOutline>().EventQues(oeid);
            if (dt == null)
            {
                dt = new DataTable("EventQues");
                dt.Columns.Add(new DataColumn("iscorrect", Type.GetType("System.Boolean")));
                dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));
            }
            int rowcount = dt.Rows.Count;
            int maxLine = 4;
            for (int i = maxLine - rowcount; i > rowcount; i--)
            {
                DataRow dr = dt.NewRow();
                dr["item"] = "";
                dr["iscorrect"] = false; 
                dt.Rows.Add(dr);
            }
            gvAnswer.DataSource = dt;
            gvAnswer.DataBind();
        }
        /// <summary>
        /// ����ʵʱ������ѡ������
        /// </summary>
        /// <returns></returns>
        private void setEventFeedback(int oeid)
        {
            DataTable dt = Business.Do<IOutline>().EventFeedback(oeid);
            if (dt == null)
            {
                dt = new DataTable("EventFeedback");
                dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("point", Type.GetType("System.Int32")));
            }
            int rowcount = dt.Rows.Count;
            int maxLine = 6;
            for (int i = maxLine - rowcount; i > rowcount; i--)
            {
                DataRow dr = dt.NewRow();
                dr["item"] = "";
                dr["point"] = 0;
                dt.Rows.Add(dr);
            }
            rptFeedback.DataSource = dt;
            rptFeedback.DataBind();
        }
        /// <summary>
        /// ��ȡ�����ѡ������
        /// </summary>
        /// <returns></returns>
        private DataTable getEventQues()
        {
            DataTable dt = new DataTable("EventFeedback");
            dt.Columns.Add(new DataColumn("iscorrect", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));            
            //���ѡ����
            for (int i = 0; i < gvAnswer.Rows.Count; i++)
            {
                //��ѡť
                RadioButton rb = (RadioButton)gvAnswer.Rows[i].FindControl("rbAns");
                //ѡ���ı���
                TextBox tb = (TextBox)gvAnswer.Rows[i].FindControl("itemTxt");
                DataRow dr = dt.NewRow();
                dr["iscorrect"] = rb.Checked;
                dr["item"] = tb.Text;                
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// ��ȡʵʱ������ѡ������
        /// </summary>
        /// <returns></returns>
        private DataTable getEventFeedback()
        {
            DataTable dt = new DataTable("EventFeedback");
            dt.Columns.Add(new DataColumn("item", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("point", Type.GetType("System.Int32")));
            foreach (Control c in rptFeedback.Controls)
            {
                string item = ((TextBox)c.FindControl("tbItem")).Text;
                int point;
                int.TryParse(((TextBox)c.FindControl("tbPoint")).Text,out point);
                DataRow dr = dt.NewRow();
                dr["item"] = item;
                dr["point"] = point;                
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion


    }
}
