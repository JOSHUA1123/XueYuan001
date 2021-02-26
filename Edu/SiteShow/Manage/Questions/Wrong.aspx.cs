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
using System.Data.OleDb;
using System.IO;
using Common;

using ServiceInterfaces;
using EntitiesInfo;
using WeiSha.WebControl;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace SiteShow.Manage.Questions
{
    public partial class Wrong : Extend.CustomPage
    {

        //���ͷ��຺������
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {            
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        #region ��������
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            //רҵ
            EntitiesInfo.Subject[] subjects = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, -1);
            ddlSubject.DataSource = subjects;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.Root = 0;
            this.ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, new ListItem("-רҵ-", "0"));            
            //
            this.SearchBind(this.searchBox);
            ddlSubject_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// רҵѡ�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCourse.Items.Clear();
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                //�ϼ�
                List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, true, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            ddlCourse.Items.Insert(0, new ListItem("-�γ�-", "0"));            
            this.SearchBind(this.searchBox);
            ddlCourse_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// �γ�ѡ�������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOutline.Items.Clear();
            //�γ�id
            int cou;
            int.TryParse(ddlCourse.SelectedValue, out cou);
            if (cou > 0)
            {                
                EntitiesInfo.Outline[] outline = Business.Do<IOutline>().OutlineAll(cou, null);
                ddlOutline.DataSource = outline;
                this.ddlOutline.DataTextField = "Ol_Name";
                this.ddlOutline.DataValueField = "Ol_ID";
                ddlOutline.DataBind();
            }
            this.ddlOutline.Items.Insert(0, new ListItem("-�½�-", "0"));
            this.SearchBind(this.searchBox);
            BindData(null, null);
        }
        #endregion
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //�ܼ�¼��
            int count = 0;
            //ѧ�ƣ��γ̣��½�
            int sbjid = 0, couid = 0, olid = 0;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            int.TryParse(ddlCourse.SelectedValue, out couid);
            int.TryParse(ddlOutline.SelectedValue, out olid);
            //���׶�
            int diff = Convert.ToInt32(ddlDiff.SelectedItem.Value);
            EntitiesInfo.Questions[] eas = null;
            eas = Business.Do<IQuestions>().QuesPager(org.Org_ID, -1, sbjid, couid, olid, null, null,true, diff, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            //ȥ������е�html��ǩ
            string regexstr = @"(<[^>]*>)|\r|\n|\s";
            foreach (EntitiesInfo.Questions q in eas)
            {
                q.Qus_Title = Regex.Replace(q.Qus_Title, regexstr, string.Empty, RegexOptions.IgnoreCase);
            }
            GridView1.DataKeyNames = new string[] { "Qus_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery(this.searchBox);
            Pager1.Index = 1;
            ////BindData(null, null);
            //string tm = this.Search(this.searchBox);
            //this.Response.Write(tm);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;            
            Business.Do<IQuestions>().QuesDelete(keys);
            BindData(null, null);           
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {            
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IQuestions>().QuesDelete(id);
            BindData(null, null);            
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Questions entity = Business.Do<IQuestions>().QuesSingle(id);
            entity.Qus_IsUse = !entity.Qus_IsUse;
            Business.Do<IQuestions>().QuesSave(entity);
            BindData(null, null);
 
        }

        protected void lbUse_Click(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                if (string.IsNullOrEmpty(id)) continue;
                int tmid = Convert.ToInt16(id);
                EntitiesInfo.Questions entity = Business.Do<IQuestions>().QuesSingle(tmid);
                entity.Qus_IsUse = true;
                Business.Do<IQuestions>().QuesSave(entity);
            }
            BindData(null, null);

        }
        protected void lbNoUse_Click(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    if (string.IsNullOrEmpty(id)) continue;
                    int tmid = Convert.ToInt16(id);
                    EntitiesInfo.Questions entity = Business.Do<IQuestions>().QuesSingle(tmid);
                    entity.Qus_IsUse = false;
                    Business.Do<IQuestions>().QuesSave(entity);
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
