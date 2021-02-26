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



namespace SiteShow.Manage.Admin
{
    public partial class Students_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //Ա���ϴ����ϵ�����·��
        private string _uppath = "Student";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                init();
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        private void init()
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EntitiesInfo.StudentSort[] sort = Business.Do<IStudent>().SortAll(org.Org_ID, true);
            Sts_ID.DataSource = sort;
            Sts_ID.DataBind();
            foreach (EntitiesInfo.StudentSort ts in sort)
            {
                if (ts.Sts_IsDefault)
                {
                    ListItem li = Sts_ID.Items.FindByValue(ts.Sts_ID.ToString());
                    if (li != null) li.Selected = true;
                }
            }
        }
        private void fill()
        {
            EntitiesInfo.Accounts th = id == 0 ? new EntitiesInfo.Accounts() : Business.Do<IAccounts>().AccountsSingle(id);
            if (th == null) return;
            this.EntityBind(th);
            //��������
            Ac_Birthday.Text = th.Ac_Birthday < DateTime.Now.AddYears(-100) ? "" : th.Ac_Birthday.ToString("yyyy-MM-dd");
            //������Ƭ
            if (!string.IsNullOrEmpty(th.Ac_Photo) && th.Ac_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + th.Ac_Photo;
            }
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {            
            //
            EntitiesInfo.Accounts st = id == 0 ? new EntitiesInfo.Accounts() : Business.Do<IAccounts>().AccountsSingle(id);
            st = this.EntityFill(st) as EntitiesInfo.Accounts;
            if (st.Ac_Birthday > DateTime.Now.AddYears(-100))
            {
                st.Ac_Age = Convert.ToInt32((DateTime.Now - st.Ac_Birthday).TotalDays / 365);
            }
            if (id == 0)
            {
                EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org == null) throw new Common.ExceptionForAlert("��ǰ����������");
                st.Org_ID = org.Org_ID;
            }
            //ͼƬ
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(st.Ac_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    st.Ac_Photo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            //���������
            if (Sts_ID.Items.Count > 0)
                st.Sts_Name = Sts_ID.SelectedItem.Text;
            //�ж��Ƿ����
            EntitiesInfo.Accounts t = Business.Do<IAccounts>().IsAccountsExist(-1, st);
            if (t!=null)
            {
                Master.Alert(string.Format("��ǰѧԱ�˺� {0} �Ѿ�����", st.Ac_AccName));
                return;
            }
            try
            {
                if (id == 0)
                {
                    id = Business.Do<IAccounts>().AccountsAdd(st);
                }
                else
                {
                    Business.Do<IAccounts>().AccountsSave(st);
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
