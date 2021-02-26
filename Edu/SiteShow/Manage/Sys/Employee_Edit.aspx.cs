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



namespace SiteShow.Manage.Sys
{
    public partial class Employee_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //Ա���ϴ����ϵ�����·��
        private string _uppath = "Employee";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }   
            //������������ʾ���
            this.trPw1.Visible = id == 0;
        }
        private void fill()
        {

            EmpAccount ea;
            if (id != 0)
            {
                ea = Business.Do<IEmployee>().GetSingle(id);
                if (ea == null) return;
                //������ɫ
                //ddlPosi.Enabled = !Business.Do<IEmployee>().IsAdmin(id);
                ListItem liPosi = ddlPosi.Items.FindByValue(ea.Posi_Id.ToString());
                if (liPosi != null)
                {
                    ddlPosi.SelectedIndex = -1;
                    liPosi.Selected = true;
                }
                //��������
                EntitiesInfo.EmpGroup[] egs = Business.Do<IEmpGroup>().GetAll4Emp(id);
                foreach (EntitiesInfo.EmpGroup eg in egs)
                {
                    ListItem li = this.cblGroup.Items.FindByValue(eg.EGrp_Id.ToString());
                    if (li != null) li.Selected = true;
                }
                //�Ա�
                string sex = ea.Acc_Sex.ToString().ToLower();
                ListItem liSex = rbSex.Items.FindByValue(sex);
                if (liSex != null)
                {
                    rbSex.SelectedIndex = -1;
                    liSex.Selected = true;
                }
                //����
                if (ea.Acc_Age > DateTime.Now.AddYears(-100).Year)
                {
                    tbAge.Text = (DateTime.Now.Year - ea.Acc_Age).ToString();
                }
                //�Ƿ���ְ
                cbIsUse.Checked = ea.Acc_IsUse;
                //�Ƿ�ȫְ
                cbIsPartTime.Checked = ea.Acc_IsPartTime;
                //������Ƭ
                if (!string.IsNullOrEmpty(ea.Acc_Photo) && ea.Acc_Photo.Trim() != "")
                {
                    this.imgShow.Src = Upload.Get[_uppath].Virtual + ea.Acc_Photo;
                }
            }
            else
            {
                ea = new EmpAccount();
                ea.Acc_RegTime = DateTime.Now;
            }
            //Ա���ʺ�
            this.tbAccName.Text = ea.Acc_AccName;
            //���֤
            this.tbIdCard.Text = ea.Acc_IDCardNumber;
            //Ա��������ƴ����д
            this.tbName.Text = ea.Acc_Name;
            tbNamePinjin.Text = ea.Acc_NamePinyin;
            //Ա����¼���룬Ϊ��
            //����
            this.tbEmail.Text = ea.Acc_Email;
            //Ա������
            this.tbEmpCode.Text = ea.Acc_EmpCode;
            //��ְʱ��
            DateTime regTime = ea.Acc_RegTime;
            this.tbRegTime.Text = regTime.ToString("yyyy-MM-dd");
            //����Ժϵ
            ListItem liDepart = ddlDepart.Items.FindByValue(ea.Dep_Id.ToString());
            if (liDepart != null)
            {
                liDepart.Selected = true;
                ddlDepart_SelectedIndexChanged(null, null);
            }
            //ְ��ͷ�Σ�
            ListItem liTitle = ddlTitle.Items.FindByValue(ea.Title_Id.ToString());
            if (liTitle != null) liTitle.Selected = true;
            //��ϵ��ʽ
            tbTel.Text = ea.Acc_Tel;
            cbIsOpenTel.Checked = ea.Acc_IsOpenTel;
            tbMobleTel.Text = ea.Acc_MobileTel;
            cbIsOpenMobile.Checked = ea.Acc_IsOpenMobile;
            tbQQ.Text = ea.Acc_QQ;
            tbWeixin.Text = ea.Acc_Weixin;
            //��ְʱ��
            cbIsAutoOut.Checked = ea.Acc_IsAutoOut;
            DateTime outTime = ea.Acc_OutTime;
            if (outTime.AddYears(100) > DateTime.Now)
            {
                this.tbOutTime.Text = outTime.ToString("yyyy-MM-dd");
            }
            cbIsAutoOut.Enabled = ea.Acc_IsUse;
            //�����Ҫ�����Զ���ְ������ʾ��ְʱ��
            cbIsAutoOut_CheckedChanged(null, null);
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EmpAccount obj = null;
            EntitiesInfo.Organization org;
            if (Extend.LoginState.Admin.IsSuperAdmin)
                org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            else
                org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new Common.ExceptionForAlert("��ǰ����������");
            try
            {
                if (id == 0)
                {
                    obj = new EmpAccount();                   
                    obj.Org_ID = org.Org_ID;
                    obj.Org_Name = org.Org_Name;
                }
                else
                {
                    obj = Business.Do<IEmployee>().GetSingle(id);
                    //��������û���Ĺ���
                    Business.Do<IEmpGroup>().DelRelation4Emplyee(id);
                }
                //�ʺ�
                obj.Acc_AccName = this.tbAccName.Text.Trim();
                //Ա������
                obj.Acc_Name = this.tbName.Text.Trim();
                obj.Acc_NamePinyin = tbNamePinjin.Text.Trim();
                //Ա����¼���룬Ϊ��
                if (tbPw1.Text != "")
                {
                    //string md5 = Common.Request.Controls[tbPw1].MD5;
                    obj.Acc_Pw = tbPw1.Text.Trim();
                }
                //���֤
                obj.Acc_IDCardNumber = tbIdCard.Text.Trim();
                //����
                obj.Acc_Email = this.tbEmail.Text.Trim();
                //�Ƿ���ְ���Ƿ�ȫְ
                obj.Acc_IsUse = this.cbIsUse.Checked;
                obj.Acc_IsPartTime = cbIsPartTime.Checked;
                //Ա������
                obj.Acc_EmpCode = this.tbEmpCode.Text.Trim();
                //�Ա�
                obj.Acc_Sex = Convert.ToInt16(rbSex.SelectedValue);
                //����
                if (tbAge.Text.Trim() != "")
                {
                    obj.Acc_Age = DateTime.Now.Year - Convert.ToInt32(tbAge.Text.Trim());
                }
                //��ְʱ��
                obj.Acc_RegTime = Convert.ToDateTime(this.tbRegTime.Text);
                //����Ժϵ
                obj.Dep_Id = Convert.ToInt16(ddlDepart.SelectedValue);
                if (spanTeam.Visible)
                {
                    obj.Team_ID = Convert.ToInt32(ddlTeam.SelectedItem.Value);
                    obj.Team_Name = ddlTeam.SelectedItem.Text;
                }
                //������ɫ
                obj.Posi_Id = Convert.ToInt16(ddlPosi.SelectedValue);
                //ְ��ͷ�Σ�
                obj.Title_Id = Convert.ToInt32(ddlTitle.SelectedValue);
                obj.Title_Name = ddlTitle.SelectedItem.Text;
                //��ϵ��ʽ
                obj.Acc_Tel = tbTel.Text;
                obj.Acc_IsOpenTel = cbIsOpenTel.Checked;
                obj.Acc_MobileTel = tbMobleTel.Text;
                obj.Acc_IsOpenMobile = cbIsOpenMobile.Checked;
                obj.Acc_QQ = tbQQ.Text;
                obj.Acc_Weixin = tbWeixin.Text.Trim();
                //��ְʱ��
                obj.Acc_IsAutoOut = cbIsAutoOut.Checked;
                //ͼƬ
                if (fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.IsMakeSmall = false;
                        fuLoad.IsConvertJpg = true;
                        fuLoad.SaveAndDeleteOld(obj.Acc_Photo);
                        fuLoad.File.Server.ChangeSize(150, 200, false);
                        obj.Acc_Photo = fuLoad.File.Server.FileName;
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                if (tbOutTime.Text.Trim() != "")
                {
                    obj.Acc_OutTime = Convert.ToDateTime(this.tbOutTime.Text);
                }
                bool isExists = Business.Do<IEmployee>().IsExists(org.Org_ID, obj);
                if (isExists)
                {
                    Master.Alert("��ǰ�ʺ��Ѿ����ڣ�");
                    return;
                }
                if (id == 0)
                {
                    id = Business.Do<IEmployee>().Add(obj);
                    this.AddRel(id);
                }
                else
                {
                    Business.Do<IEmployee>().Save(obj);
                    this.AddRel(id);
                }

                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// ������û���Ĺ���
        /// </summary>
        /// <param name="id">Ա��id</param>
        private void AddRel(int id)
        {
            try
            {
                foreach (ListItem li in this.cblGroup.Items)
                {
                    if (li.Selected)
                    {
                        int gid = Convert.ToInt32(li.Value);
                        Business.Do<IEmpGroup>().AddRelation(id, gid);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            try
            {
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                EntitiesInfo.Depart[] nc = Business.Do<IDepart>().GetAll(orgid,true,true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                //ddlDepart.Items.Insert(0, new ListItem("", "-1"));
                //��ɫ
                EntitiesInfo.Position[] posi = Business.Do<IPosition>().GetAll(orgid,true);
                foreach (EntitiesInfo.Position p in posi)
                {
                    if (p.Posi_IsAdmin)
                    {
                        p.Posi_Name = p.Posi_Name + "*";
                    }
                }
                ddlPosi.DataSource = posi;
                ddlPosi.DataTextField = "Posi_Name";
                ddlPosi.DataValueField = "Posi_Id";
                ddlPosi.DataBind();
                //ddlPosi.Items.Insert(0,new ListItem("", "-1"));
                //�û���
                EntitiesInfo.EmpGroup[] group = Business.Do<IEmpGroup>().GetAll(orgid,true);
                cblGroup.DataSource = group;
                cblGroup.DataTextField = "EGrp_Name";
                cblGroup.DataValueField = "EGrp_Id";
                cblGroup.DataBind();
                //ְ��ͷ�Σ�
                EntitiesInfo.EmpTitle[] title = Business.Do<IEmployee>().TitleAll(orgid,true);
                ddlTitle.DataSource = title;
                ddlTitle.DataTextField = "Title_Name";
                ddlTitle.DataValueField = "Title_Id";
                ddlTitle.DataBind();
                ddlTitle.Items.Insert(0, new ListItem("", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ���Ƿ���ְѡ��ı�ʱ��
        /// �����ְ��������趨ʲôʱ���Զ���ְ
        /// ����Ѿ�����ְ���򲻱��������Զ���ְ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsUse_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            //�������ְ����ʾ��ְʱ��
            cbIsAutoOut_CheckedChanged(null, null);
            //�Ƿ�Ԥ���Զ���ְ
            cbIsAutoOut.Enabled = cb.Checked;
            cbIsAutoOut.Checked = false;
            //if (!cb.Checked) cbIsAutoOut.Enabled = false;
            
        }
        /// <summary>
        /// ��������Զ���ְ������ʾ��ְʱ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsAutoOut_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //�Ƿ��ְ
                bool isUse = cbIsUse.Checked;
                //
                spanOutTime.Visible = (cbIsAutoOut.Checked && cbIsAutoOut.Enabled && isUse) || !isUse;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// ��ѡ��Ժϵʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int depId = Convert.ToInt32(ddlDepart.SelectedValue);
                EntitiesInfo.Team[] nc = Business.Do<ITeam>().GetTeam(true, depId, 0);
                if (nc != null && nc.Length > 0)
                {
                    spanTeam.Visible = true;
                    this.ddlTeam.DataSource = nc;
                    this.ddlTeam.DataTextField = "Team_Name";
                    this.ddlTeam.DataValueField = "Team_ID";
                    this.ddlTeam.DataBind();
                }
                else
                {
                    spanTeam.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
