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
    public partial class Student_Password : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            //������������ʾ���
            //this.trPw1.Visible = this.trPw2.Visible = id == 0;
        }
        private void fill()
        {
            EntitiesInfo.Accounts ea;
            if (id == 0) return;
            ea = Business.Do<IAccounts>().AccountsSingle(id);
            //Ա���ʺ�
            this.tbStudentAcc.Text = ea.Ac_AccName;
            //Ա������
            this.lbName.Text = ea.Ac_Name;

        }

        /// <summary>
        /// ��֤�˺��Ƿ��Ѿ�����
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusv_ServerValidate(object source, ServerValidateEventArgs args)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EntitiesInfo.Accounts th = Business.Do<IAccounts>().AccountsSingle(this.tbStudentAcc.Text.Trim(), org.Org_ID);
            if (th == null) th = new EntitiesInfo.Accounts();
            th.Org_ID = org.Org_ID;
            th.Ac_AccName = this.tbStudentAcc.Text.Trim();
            //�ж��Ƿ�ͨ����֤
            EntitiesInfo.Accounts t = Business.Do<IAccounts>().IsAccountsExist(org.Org_ID, th);
            args.IsValid = t==null;
        }

        /// <summary>
        /// �޸��˺���Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAcc_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("�ǹ���Ա��Ȩ�˲���Ȩ�ޣ�");
                if (id == 0) throw new Exception("��ǰ��Ϣ�����ڣ�");
                //��֤�����
                if (!cusv.IsValid)
                    return;
                EntitiesInfo.Accounts obj;
                obj = Business.Do<IAccounts>().AccountsSingle(id);
                obj.Ac_AccName = tbStudentAcc.Text.Trim();
                Business.Do<IAccounts>().AccountsSave(obj);
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPw_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("�ǹ���Ա��Ȩ�˲���Ȩ�ޣ�");
                if (id == 0) throw new Exception("��ǰ��Ϣ�����ڣ�");
                //��֤�����
                if (!cusv.IsValid)
                    return;
                EntitiesInfo.Accounts obj;
                obj = Business.Do<IAccounts>().AccountsSingle(id);
                //Ա����¼���룬Ϊ��
                if (tbPw1.Text.Trim() != "")
                    obj.Ac_Pw = tbPw1.Text.Trim();
                obj.Ac_Pw = new Common.Param.Method.ConvertToAnyValue(obj.Ac_Pw).MD5;  
                Business.Do<IAccounts>().AccountsSave(obj);
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

    }
}
