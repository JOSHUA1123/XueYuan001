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
using System.Collections.Generic;

namespace SiteShow.Manage.Exam
{
    public partial class TestPaper : Extend.CustomPage
    {
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        #region
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            EntitiesInfo.Subject[] eas = null;
            eas = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, -1);
            ddlTree.DataSource = eas;
            this.ddlTree.DataTextField = "Sbj_Name";
            this.ddlTree.DataValueField = "Sbj_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("-- רҵ --", "0"));
            ddlTree_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// ��רҵ�����˵�����ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTree_SelectedIndexChanged(object sender, EventArgs e)
        {
            //�γ�
            int sbjid;
            int.TryParse(ddlTree.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                List<EntitiesInfo.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, true, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            if (ddlCourse.Items.Count < 1)
            {
                ddlCourse.Items.Add(new ListItem("--�γ�--", "0"));
            }
        }
        /// <summary>
        /// ��ǰ�γ������˵�����ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(null, null);
        }
        #endregion
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //�ܼ�¼��
            int count = 0;
            int sbj = Convert.ToInt32(ddlTree.SelectedValue);
            string sear = tbSear.Text.Trim();
            int couid = 0;
            int.TryParse(ddlCourse.SelectedValue, out couid);
            //
            EntitiesInfo.TestPaper[] eas = null;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            eas = Business.Do<ITestPaper>().PaperPager(org.Org_ID, sbj, couid,-1, null, sear, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Tp_Id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
 
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<ITestPaper>().PagerDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
             try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<ITestPaper>().PagerDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                EntitiesInfo.TestPaper entity = Business.Do<ITestPaper>().PagerSingle(id);
                entity.Tp_IsUse = !entity.Tp_IsUse;
                Business.Do<ITestPaper>().PagerSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
        }

        
    }
}
