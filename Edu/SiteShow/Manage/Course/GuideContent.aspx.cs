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
    public partial class GuideContent : Extend.CustomPage
    {
        //�γ�id����������id
        protected int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        protected int gcid = Common.Request.QueryString["gcid"].Int32 ?? -1;
        //״̬
        protected string state = Common.Request.QueryString["state"].String;
        //private string _uppath = "Guide";
        EntitiesInfo.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                if (state == "add" || state == "edit")
                {
                    plEditArea.Visible = true;
                    plListArea.Visible = false;
                }
                else
                {
                    plEditArea.Visible = false;
                    plListArea.Visible = true;

                }
                //��Ŀ����
                spanColName.Visible = gcid != 0;
                if (gcid != 0)
                {
                    EntitiesInfo.GuideColumns col = Business.Do<IGuide>().ColumnsSingle(gcid);
                    if (col != null) lbColunms.Text = col.Gc_Title;
                }
                BindData(null, null);
            }           
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //�ܼ�¼��
            int count = 0;
            EntitiesInfo.Guide[] eas = Business.Do<IGuide>().GetGuidePager(-1, couid, gcid, tbSear.Text, null, Pager1.Size, Pager1.Index, out count);

            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Gu_ID" };
            GridView1.DataBind();
            
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ�ʹ�õ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            EntitiesInfo.Guide entity = Business.Do<IGuide>().GuideSingle(id);
            entity.Gu_IsShow = !entity.Gu_IsShow;
            Business.Do<IGuide>().GuideSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
                Business.Do<IGuide>().GuideDelete(Convert.ToInt32(id));
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
            Business.Do<IGuide>().GuideDelete(id);
            BindData(null, null);

        }
        #region ���
        /// <summary>
        /// ͨ����������༭״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddEvent(object sender, EventArgs e)
        {
            plEditArea.Visible = true;
            plListArea.Visible = false;
            //�ϼ�
            ddlTree.Items.Clear();
            EntitiesInfo.GuideColumns[] cous = Business.Do<IGuide>().GetColumnsAll(couid, null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Gc_title";
            this.ddlTree.DataValueField = "Gc_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("   -- ���� --", "0"));

            lbID.Text = "";
            tbTitle.Text = "";
            //
            ListItem li = ddlTree.Items.FindByValue(gcid.ToString());
            if (li != null) li.Selected = true;
        }
        /// <summary>
        /// ����༭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            plEditArea.Visible = true;
            plListArea.Visible = false;
            //
            WeiSha.WebControl.RowEdit img = (WeiSha.WebControl.RowEdit)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());            
            //�ϼ�
            ddlTree.Items.Clear();
            EntitiesInfo.GuideColumns[] cous = Business.Do<IGuide>().GetColumnsAll(couid, null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Gc_title";
            this.ddlTree.DataValueField = "Gc_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("   -- ���� --", "0"));
            //
            EntitiesInfo.Guide guide = Business.Do<IGuide>().GuideSingle(id);
            lbID.Text = guide.Gu_Id.ToString();
            tbTitle.Text = guide.Gu_Title;
            cbIsHot.Checked = guide.Gu_IsHot;
            //�Ƿ���ʾ
            cbIsShow.Checked = guide.Gu_IsShow;
            //�Ƿ��ö�
            cbIsTop.Checked = guide.Gu_IsTop;
            //�Ƿ��Ƽ�
            cbIsRec.Checked = guide.Gu_IsRec;
            tbDetails.Text = guide.Gu_Details;
            ListItem li = ddlTree.Items.FindByValue(guide.Gc_ID.ToString());
            if (li != null) li.Selected = true;
        }
        /// <summary>
        /// �˳��༭����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBack_Click(object sender, EventArgs e)
        {
            plEditArea.Visible = false;
            plListArea.Visible = true;
        }
        /// <summary>
        /// ������ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEnter_Click(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(lbID.Text, out id);
            EntitiesInfo.Guide guide = id == 0 ? new EntitiesInfo.Guide() : Business.Do<IGuide>().GuideSingle(id);
            guide.Gu_Title = tbTitle.Text;
            guide.Gu_IsHot = cbIsHot.Checked;
            //�Ƿ���ʾ
            guide.Gu_IsShow = cbIsShow.Checked;
            //�Ƿ��ö�
            guide.Gu_IsTop = cbIsTop.Checked;
            //�Ƿ��Ƽ�
            guide.Gu_IsRec = cbIsRec.Checked;
            guide.Gu_Details = tbDetails.Text;
            //
            int gcolid=0;
            int.TryParse(ddlTree.SelectedValue, out gcolid);            
            guide.Gc_ID = gcolid;
            guide.Cou_ID = couid;
            guide.Org_ID = org.Org_ID;
            //
            if (string.IsNullOrWhiteSpace(guide.Gu_Uid)) guide.Gu_Uid = getUID();
            if (id == 0)
                Business.Do<IGuide>().GuideAdd(guide);
            else
                Business.Do<IGuide>().GuideSave(guide);
            BindData(null, null);
            btnAddBack_Click(null, null);
        }
        #endregion       
    }
}
