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
    public partial class GuideColumns : Extend.CustomPage
    {
        //�γ�id
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
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
            EntitiesInfo.GuideColumns[] eas = Business.Do<IGuide>().GetColumnsAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Gc_ID";
            tree.ParentIdKeyName = "Gc_PID";
            tree.TaxKeyName = "Gc_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);            
            gvColumns.DataSource = dt;
            gvColumns.DataKeyNames = new string[] { "Gc_ID" };
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
            EntitiesInfo.GuideColumns[] cous = Business.Do<IGuide>().GetColumnsAll(couid,null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Gc_title";
            this.ddlTree.DataValueField = "Gc_ID";
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
            EntitiesInfo.GuideColumns col = new EntitiesInfo.GuideColumns();
            col.Gc_Title = tbTitle.Text.Trim();
            col.Gc_PID = Convert.ToInt32(ddlTree.SelectedValue);
            col.Cou_ID = couid;
            col.Gc_Intro = tbIntro.Text.Trim();
            col.Gc_IsUse = cbIsUse.Checked;
            Business.Do<IGuide>().ColumnsAdd(col);
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
            Business.Do<IGuide>().ColumnsDelete(id);
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
            EntitiesInfo.GuideColumns[] cous = Business.Do<IGuide>().GetColumnsAll(couid, null);
            tree.DataSource = cous;
            tree.DataTextField = "Gc_title";
            tree.DataValueField = "Gc_ID";
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
            EntitiesInfo.GuideColumns col = Business.Do<IGuide>().ColumnsSingle(id);
            if (col != null)
            {
                //����
                TextBox tb = (TextBox)gvColumns.Rows[index].FindControl("tbTitle");
                col.Gc_Title = tb.Text.Trim();
                //����
                DropDownTree tree = (DropDownTree)gvColumns.Rows[index].FindControl("ddlColTree");
                col.Gc_PID = Convert.ToInt32(tree.SelectedValue);
                //�Ƿ����
                CheckBox cb = (CheckBox)gvColumns.Rows[index].FindControl("cbIsUse");
                col.Gc_IsUse = cb.Checked;
                //���
                TextBox tbintro = (TextBox)gvColumns.Rows[index].FindControl("tbIntro");
                col.Gc_Intro = tbintro.Text.Trim();
                //
                Business.Do<IGuide>().ColumnsSave(col);
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
            EntitiesInfo.GuideColumns entity = Business.Do<IGuide>().ColumnsSingle(id);
            entity.Gc_IsUse = !entity.Gc_IsUse;
            Business.Do<IGuide>().ColumnsSave(entity);
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
            if (Business.Do<IGuide>().ColumnsRemoveUp(id)) BindData(null, null);

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
            if (Business.Do<IGuide>().ColumnsRemoveDown(id)) BindData(null, null);
        }
        #endregion

    }
}
