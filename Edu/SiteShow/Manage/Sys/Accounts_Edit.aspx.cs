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
    public partial class Accounts_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //�ϴ����ϵ�����·��
        private string _uppath = "Accounts";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            EntitiesInfo.Accounts acc = id != 0 ? Business.Do<IAccounts>().AccountsSingle(id) : new EntitiesInfo.Accounts();
            if (acc == null) return;
            if (id != 0)
            {   
                //�Ա�
                string sex = acc.Ac_Sex.ToString().ToLower();
                ListItem liSex = rbSex.Items.FindByValue(sex);
                if (liSex != null)
                {
                    rbSex.SelectedIndex = -1;
                    liSex.Selected = true;
                }
                //����
                if (acc.Ac_Age > DateTime.Now.AddYears(-100).Year)
                {
                    tbAge.Text = (DateTime.Now.Year - acc.Ac_Age).ToString();
                }                
                //������Ƭ
                if (!string.IsNullOrEmpty(acc.Ac_Photo) && acc.Ac_Photo.Trim() != "")
                    this.imgShow.Src = Upload.Get[_uppath].Virtual + acc.Ac_Photo;
               
            }
            if (acc.Org_ID > 0)
            {
                EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
                if (org != null) lbOrgin.Text = org.Org_Name;
            }
            //Ա���ʺ�
            this.tbAccName.Text = acc.Ac_AccName;
            //���֤
            this.tbIdCard.Text = acc.Ac_IDCardNumber;
            //Ա��������ƴ����д
            this.tbName.Text = acc.Ac_Name;
            tbNamePinjin.Text = acc.Ac_Pinyin;
            //Ա����¼���룬Ϊ��
            //����
            this.tbEmail.Text = acc.Ac_Email;           

            //��ϵ��ʽ
            tbTel.Text = acc.Ac_Tel;
            cbIsOpenTel.Checked = acc.Ac_IsOpenTel;
            tbMobleTel1.Text = acc.Ac_MobiTel1;
            tbMobleTel2.Text = acc.Ac_MobiTel2;
            cbIsOpenMobile.Checked = acc.Ac_IsOpenMobile;
            tbQQ.Text = acc.Ac_Qq;
            tbWeixin.Text = acc.Ac_Weixin;            
            
        }
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts acc = id != 0 ? Business.Do<IAccounts>().AccountsSingle(id) : new EntitiesInfo.Accounts();           
            //�ʺ�
            acc.Ac_AccName = this.tbAccName.Text.Trim();
            //Ա������
            acc.Ac_Name = this.tbName.Text.Trim();
            acc.Ac_Pinyin = tbNamePinjin.Text.Trim();
            acc.Ac_IDCardNumber = tbIdCard.Text.Trim(); //���֤
            //�Ա�
            acc.Ac_Sex = Convert.ToInt16(rbSex.SelectedValue);
            //����
            if (tbAge.Text.Trim() != "")
            {
                acc.Ac_Age = DateTime.Now.Year - Convert.ToInt32(tbAge.Text.Trim());
            }           
            //��ϵ��ʽ
            acc.Ac_Email = this.tbEmail.Text.Trim();    //����  
            acc.Ac_Tel = tbTel.Text;
            acc.Ac_IsOpenTel = cbIsOpenTel.Checked;
            acc.Ac_MobiTel1 = tbMobleTel1.Text;
            acc.Ac_MobiTel2 = tbMobleTel2.Text;
            acc.Ac_IsOpenMobile = cbIsOpenMobile.Checked;
            acc.Ac_Qq = tbQQ.Text;
            acc.Ac_Weixin = tbWeixin.Text.Trim();            
            //ͼƬ
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(acc.Ac_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    acc.Ac_Photo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            try
            {               
                if (id == 0)
                {
                    id = Business.Do<IAccounts>().AccountsAdd(acc);                   
                }
                else
                {
                    Business.Do<IAccounts>().AccountsSave(acc);
                }

                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

       
    }
}
