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



namespace SiteShow.Manage.Site
{
    public partial class User_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlGroupBind();
                fill();
            }   
            //������������ʾ���
            this.trPw1.Visible = this.trPw2.Visible = id == 0;
        }
        private void fill()
        {
            try
            {
                EntitiesInfo.User ea;
                if (id != 0)
                {
                    ea = Business.Do<IUser>().GetUserSingle(id);
                    //��������
                    EntitiesInfo.UserGroup egs = Business.Do<IUser>().GetGroup4User(id);
                    ListItem li = this.ddlGroup.Items.FindByValue(egs.UGrp_Id.ToString());
                    if (li != null) li.Selected = true;

                    //�Ա�
                    string sex = ea.User_Sex.ToString().ToLower();
                    ListItem liSex = rbSex.Items.FindByValue(sex);
                    if (liSex != null) liSex.Selected = true;
                }
                else
                {
                    ea = new EntitiesInfo.User();
                    ea.User_RegTime = DateTime.Now;
                    ea.User_IsUse = true;
                }
                //Ա���ʺ�
                this.tbAccName.Text = ea.User_AccName;
                //Ա������
                this.tbName.Text = ea.User_Name;
                //Ա����¼���룬Ϊ��
                //����
                this.tbEmail.Text = ea.User_Email;
                //�Ƿ���ְ
                this.cbIsUse.Checked = ea.User_IsUse;
                //��ϵ��ʽ
                tbTel.Text = ea.User_Tel;
                tbMobleTel.Text = ea.User_MobileTel;
                tbQQ.Text = ea.User_QQ;
                tbMsn.Text = ea.User_Msn;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }           
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                EntitiesInfo.User obj;
                if (id == 0)
                {
                    obj = new EntitiesInfo.User();
                }
                else
                {
                    obj = Business.Do<IUser>().GetUserSingle(id);
                    //��������û���Ĺ���
                    Business.Do<IEmpGroup>().DelRelation4Emplyee(id);
                }
                //�ʺ�
                obj.User_AccName = this.tbAccName.Text.Trim();
                //Ա������
                obj.User_Name = this.tbName.Text;
                //Ա����¼���룬Ϊ��
                if (tbPw1.Text != "")
                {
                    string md5 = Common.Request.Controls[tbPw1].MD5;
                    obj.User_Pw = md5;
                }
                //����
                obj.User_Email = this.tbEmail.Text;
                //�Ƿ���ְ
                obj.User_IsUse = this.cbIsUse.Checked;
                //�Ա�
                obj.User_Sex = Convert.ToInt16(rbSex.SelectedValue);
                //������
                obj.UGrp_Id = Convert.ToInt32(ddlGroup.SelectedItem.Value);
                obj.UGrp_Name = ddlGroup.SelectedItem.Text;
                //��ϵ��ʽ
                obj.User_Tel = tbTel.Text;
                obj.User_MobileTel = tbMobleTel.Text;
                obj.User_QQ = tbQQ.Text;
                obj.User_Msn = tbMsn.Text;
                if (id == 0)
                {
                    id = Business.Do<IUser>().AddUser(obj);
                }
                else
                {
                    Business.Do<IUser>().SaveUser(obj);
                }

                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// �û���������
        /// </summary>
        private void ddlGroupBind()
        {
            try
            {
                EntitiesInfo.UserGroup[] group = Business.Do<IUser>().GetGroupAll(true);
                this.ddlGroup.DataSource = group;
                this.ddlGroup.DataTextField = "UGrp_Name";
                this.ddlGroup.DataValueField = "UGrp_Id";
                this.ddlGroup.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }            
        }
    }
}
