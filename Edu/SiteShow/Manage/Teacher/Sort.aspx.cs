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

namespace SiteShow.Manage.Teacher
{
    public partial class Sort : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EntitiesInfo.TeacherSort[] eas = null;
            eas = Business.Do<ITeacher>().SortAll(org.Org_ID, null);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Ths_id" };
            GridView1.DataBind();
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUseClick(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.TeacherSort entity = Business.Do<ITeacher>().SortSingle(id);
            entity.Ths_IsUse = !entity.Ths_IsUse;
            Business.Do<ITeacher>().SortSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ�Ĭ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbDefault_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            Business.Do<ITeacher>().SortSetDefault(orgid, id);
            
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
                    Business.Do<ITeacher>().SortDelete(Convert.ToInt32(id));
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
                Business.Do<ITeacher>().SortDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        //����
        protected void lbUp_Click(object sender, EventArgs e)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ITeacher>().SortRemoveUp(orgid, id))
                BindData(null, null);
        }
        //����
        protected void lbDown_Click(object sender, EventArgs e)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ITeacher>().SortRemoveDown(orgid, id))                
                BindData(null, null); 
        }
    }
}
