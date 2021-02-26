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



namespace SiteShow.Manage.Course
{
    public partial class List_Edit : Extend.CustomPage
    {
        private int couid = Common.Request.QueryString["couid"].Int32 ?? 0;
        //�ϴ����ϵ�����·��
        private string _uppath = "Course";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        /// <summary>
        /// ����ĳ�ʼ��
        /// </summary>
        private void InitBind()
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //ѧ��/רҵ
            EntitiesInfo.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, 0, 0);
            this.ddlSubject.DataSource = subs;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.DataBind();
        }

        private void fill()
        {
            EntitiesInfo.Course cou = couid < 1 ? new EntitiesInfo.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return;
            Cou_Name.Text = cou.Cou_Name;
            //����רҵ
            ListItem liSbj = ddlSubject.Items.FindByValue(cou.Sbj_ID.ToString());
            if (liSbj != null)
            {
                ddlSubject.SelectedIndex = -1;
                liSbj.Selected = true;
            }            
            //��飬ѧϰĿ��
            tbIntro.Text = cou.Cou_Intro;
            tbTarget.Text = cou.Cou_Target;
            //�γ�ͼƬ
            if (!string.IsNullOrEmpty(cou.Cou_LogoSmall) && cou.Cou_LogoSmall.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + cou.Cou_LogoSmall;
            }
            if (couid > 0)
            {
                cbIsUse.Checked = cou.Cou_IsUse;
            }
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Course cou = couid < 1 ? new EntitiesInfo.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return;
            //����
            cou.Cou_Name = Cou_Name.Text.Trim();
            //����רҵ
            cou.Sbj_ID = Convert.ToInt32(ddlSubject.SelectedValue);
            cou.Sbj_Name = ddlSubject.SelectedItem.Text;
            //��飬ѧϰĿ��
            cou.Cou_Intro = tbIntro.Text;
            cou.Cou_Target = tbTarget.Text;
            //��ͼƬ
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = true;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SmallHeight = 113;
                    fuLoad.SmallWidth = 200;
                    fuLoad.SaveAndDeleteOld(cou.Cou_Logo);
                    fuLoad.File.Server.ChangeSize(500, 500, false);
                    cou.Cou_Logo = fuLoad.File.Server.FileName;
                    cou.Cou_LogoSmall = fuLoad.File.Server.SmallFileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            cou.Cou_IsUse = cbIsUse.Checked;
            ////������ʦ
            //EntitiesInfo.Teacher th = Extend.LoginState.Accounts.Teacher;
            //cou.Th_ID = th.Th_ID;
            //cou.Th_Name = th.Th_Name;
            ////��������
            //EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //cou.Org_ID = org.Org_ID;
            //cou.Org_Name = org.Org_Name;
            try
            {
                if (couid < 1)
                {
                    //����
                    Business.Do<ICourse>().CourseAdd(cou);
                }
                else
                {
                    Business.Do<ICourse>().CourseSave(cou);
                }
                Master.AlertCloseAndRefresh("�����ɹ�");
            }
            catch
            {
                throw;
            }
            
        }
       
    }
}
