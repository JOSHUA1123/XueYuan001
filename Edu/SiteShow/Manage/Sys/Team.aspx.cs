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

namespace SiteShow.Manage.Sys
{
    public partial class Team : Extend.CustomPage
    {
        //��������Ա��ɫ��id
        protected string superid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Form.DefaultButton = "";
                if (!this.IsPostBack)
                {
                    EntitiesInfo.Position super = Business.Do<IPosition>().GetSuper();
                    superid = super.Posi_Id.ToString();
                    ddlDepartBind();
                    BindData(null, null);
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// Ժϵ������
        /// </summary>
        private void ddlDepartBind()
        {
            try
            {
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                EntitiesInfo.Depart[] nc = Business.Do<IDepart>().GetAll(orgid, true, true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                //
                this.ddlDepart.Items.Insert(0, new ListItem(" -- ����Ժϵ -- ", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //�ܼ�¼��
                int count = 0;
                //��ǰѡ���Ժϵid
                int depId = Convert.ToInt16(ddlDepart.SelectedItem.Value);
                bool? isUse = null;
                EntitiesInfo.Team[] eas = null;
                eas = Business.Do<ITeam>().GetTeamPager(depId, isUse, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "team_id" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbOpenMobile_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                EntitiesInfo.EmpAccount entity = Business.Do<IEmployee>().GetSingle(id);
                entity.Acc_IsOpenMobile = !entity.Acc_IsOpenMobile;
                Business.Do<IEmployee>().Save(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
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
                    Business.Do<ITeam>().TeamDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
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
                Business.Do<ITeam>().TeamDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
