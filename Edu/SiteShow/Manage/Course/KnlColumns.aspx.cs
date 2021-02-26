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

namespace SiteShow.Manage.Course
{
    public partial class KnlColumns : Extend.CustomPage
    {
        //�γ�id
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            //��ǰ�γ�
            EntitiesInfo.Course course = Business.Do<ICourse>().CourseSingle(couid);
            plEdit.Visible = course != null;
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
            //�ܼ�¼��
            EntitiesInfo.KnowledgeSort[] eas = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Kns_ID";
            tree.ParentIdKeyName = "Kns_PID";
            tree.TaxKeyName = "Kns_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);            
            gvColumns.DataSource = dt;
            gvColumns.DataKeyNames = new string[] { "Kns_ID" };
            gvColumns.DataBind();
            
        }
        #region ��������İ�ť
        /// <summary>
        /// �������Ŀ�İ�ť���˴�ֻ�ǽ�����ӵĽ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            plAddColumn.Visible = true;
            gvColumns.Visible = false;
            tbTitle.Text = "";
            //�ϼ�
            EntitiesInfo.KnowledgeSort[] cous = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, couid, null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Kns_name";
            this.ddlTree.DataValueField = "Kns_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("   -- ���� --", "0"));
        }
        /// <summary>
        /// �˳���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBack_Click(object sender, EventArgs e)
        {
            plAddColumn.Visible = false;
            gvColumns.Visible = true;
        }
        /// <summary>
        /// ������ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.KnowledgeSort col = new EntitiesInfo.KnowledgeSort();
            col.Kns_Name = tbTitle.Text.Trim();
            col.Kns_PID = Convert.ToInt32(ddlTree.SelectedValue);
            col.Cou_ID = couid;
            col.Kns_Intro = tbIntro.Text.Trim();
            col.Kns_IsUse = cbIsUse.Checked;
            Business.Do<IKnowledge>().SortAdd(col);
            BindData(null, null);
            gvColumns.EditIndex = -1;
            btnAddBack_Click(null, null);
        }
        
        #endregion

        #region Gridview �����¼�
        /// <summary>
        /// ����ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvColumns.DataKeys[index].Value.ToString());
            Business.Do<IKnowledge>().SortDelete(id);
            BindData(null, null);
        }
        /// <summary>
        /// ����༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowEdit img = (WeiSha.WebControl.RowEdit)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;                      
            gvColumns.EditIndex = index;
            BindData(null, null);            
            //����
            DropDownTree tree = (DropDownTree)gvColumns.Rows[index].FindControl("ddlColTree");
            EntitiesInfo.KnowledgeSort[] cous = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, couid, null);
            tree.DataSource = cous;
            tree.DataTextField = "Kns_name";
            tree.DataValueField = "Kns_ID";
            tree.Root = 0;
            tree.DataBind();
            tree.Items.Insert(0, new ListItem("   -- ���� --", "0"));
            //��ǰ����
            int pid = Convert.ToInt32(img.CommandArgument);
            ListItem li = tree.Items.FindByValue(pid.ToString());
            if (li != null) li.Selected = true;
        }
        /// <summary>
        /// �༭��ǰ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            int index = ((GridViewRow)(btn.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvColumns.DataKeys[index].Value.ToString());  
            //
            EntitiesInfo.KnowledgeSort col = Business.Do<IKnowledge>().SortSingle(id);
            if (col != null)
            {
                //����
                TextBox tb = (TextBox)gvColumns.Rows[index].FindControl("tbTitle");
                col.Kns_Name = tb.Text.Trim();
                //����
                DropDownTree tree = (DropDownTree)gvColumns.Rows[index].FindControl("ddlColTree");
                col.Kns_PID = Convert.ToInt32(tree.SelectedValue);
                //�Ƿ����
                CheckBox cb = (CheckBox)gvColumns.Rows[index].FindControl("cbIsUse");
                col.Kns_IsUse = cb.Checked;
                //���
                TextBox tbintro = (TextBox)gvColumns.Rows[index].FindControl("tbIntro");
                col.Kns_Intro = tbintro.Text.Trim();
                //
                Business.Do<IKnowledge>().SortSave(col);
            }
            gvColumns.EditIndex = -1;
            BindData(null, null);
        }
        /// <summary>
        /// �˳��༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditBack_Click(object sender, EventArgs e)
        {
            gvColumns.EditIndex = -1;
            BindData(null, null);
        }
        /// �޸��Ƿ�ʹ�õ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvColumns.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.KnowledgeSort entity = Business.Do<IKnowledge>().SortSingle(id);
            entity.Kns_IsUse = !entity.Kns_IsUse;
            Business.Do<IKnowledge>().SortSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            gvColumns.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvColumns.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IKnowledge>().SortRemoveUp(id)) BindData(null, null);

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            gvColumns.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvColumns.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IKnowledge>().SortRemoveDown(id)) BindData(null, null);
        }
        #endregion

    }
}
