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

namespace SiteShow.Manage.Personal
{
    public partial class Info : Extend.CustomPage
    {
        EmpAccount currentUser;
        //Ա���ϴ����ϵ�����·��
        private string _uppath = "Employee";
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = Extend.LoginState.Admin.CurrentUser;
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            EmpAccount ea = Extend.LoginState.Admin.CurrentUser;
            //Ա���ʺ�
            this.lbAcc.Text = ea.Acc_AccName;
            //Ա������
            this.tbName.Text = ea.Acc_Name;
            tbNamePinjin.Text = ea.Acc_NamePinyin;
            this.lbEmpCode.Text = ea.Acc_EmpCode;
            //�Ա�
            string sex = ea.Acc_Sex.ToString().ToLower();
            ListItem liSex = rbSex.Items.FindByValue(sex);
            if (liSex != null) liSex.Selected = true;
            //Ժϵ
            EntitiesInfo.Depart depart = Business.Do<IDepart>().GetSingle(ea.Dep_Id);
            if (depart != null)
            {
                lbDepart.Text = depart.Dep_CnName;
            }
            //��ɫ
            EntitiesInfo.Position posi = Business.Do<IPosition>().GetSingle((int)ea.Posi_Id);
            if (posi != null)
            {
                lbPosi.Text = posi.Posi_Name;
            }
            //ְλ��ͷ�Σ�
            lbTeam.Text = ea.Team_Name;
            //��ϵ��ʽ
            tbTel.Text = ea.Acc_Tel;
            cbIsOpenTel.Checked = ea.Acc_IsOpenTel;
            tbMobile.Text = ea.Acc_MobileTel;
            cbIsOpenMobile.Checked = ea.Acc_IsOpenMobile;
            tbEmail.Text = ea.Acc_Email;
            tbQQ.Text = ea.Acc_QQ;
            tbWeixin.Text = ea.Acc_Weixin;
            //������Ƭ
            if (!string.IsNullOrEmpty(ea.Acc_Photo) && ea.Acc_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + ea.Acc_Photo;
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
                if (currentUser == null) return;
                EmpAccount ea = currentUser;
                //������ƴ����д
                ea.Acc_Name = tbName.Text.Trim();
                ea.Acc_NamePinyin = tbNamePinjin.Text.Trim();
                //�Ա�
                ea.Acc_Sex = Convert.ToInt16(rbSex.SelectedValue);
                //��ϵ��ʽ
                ea.Acc_Tel = tbTel.Text.Trim();
                ea.Acc_IsOpenTel = cbIsOpenTel.Checked;
                ea.Acc_MobileTel = tbMobile.Text.Trim();
                ea.Acc_IsOpenMobile = cbIsOpenMobile.Checked;
                ea.Acc_Email = tbEmail.Text.Trim();
                ea.Acc_QQ = tbQQ.Text.Trim();
                ea.Acc_Weixin = tbWeixin.Text.Trim();
                //ͼƬ
                if (fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.IsMakeSmall = false;
                        fuLoad.IsConvertJpg = true;
                        fuLoad.SaveAndDeleteOld(ea.Acc_Photo);
                        fuLoad.File.Server.ChangeSize(150, 200, false);
                        ea.Acc_Photo = fuLoad.File.Server.FileName;
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
                //
                Business.Do<IEmployee>().Save(ea);
                Master.AlertAndClose("�����ɹ���");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
    }
}
