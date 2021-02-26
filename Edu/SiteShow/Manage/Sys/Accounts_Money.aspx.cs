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
    public partial class Accounts_Money : Extend.CustomPage
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
            //Ա������
            this.lbName.Text = ea.Ac_Name;
            //�˻����
            lbMoney.Text = ea.Ac_Money.ToString("0.00");

        }

        /// <summary>
        /// ��ֵ��۷�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMoney_Click(object sender, EventArgs e)
        {            
            if (!Extend.LoginState.Admin.IsSuperAdmin) throw new Exception("��ϵͳ����Ա�������ܣ���Ȩ�˲���Ȩ�ޣ�");
            EntitiesInfo.Accounts st = Business.Do<IAccounts>().AccountsSingle(id);
            if (st == null) throw new Exception("��ǰ��Ϣ�����ڣ�");
            //��������
            int type = 2;
            int.TryParse(rblOpera.SelectedItem.Value, out type);
            //�������
            int money = 0;
            int.TryParse(tbMoney.Text, out money);
            //��������
            EntitiesInfo.MoneyAccount ma = new MoneyAccount();
            ma.Ma_Money = money;
            ma.Ma_Total = st.Ac_Money; //��ǰ�ʽ�����
            ma.Ma_Remark = tbRemark.Text.Trim();
            ma.Ac_ID = st.Ac_ID;
            ma.Ma_Source = "ϵͳ����Ա����";
            //��ֵ��ʽ������Ա��ֵ
            ma.Ma_From = 1;
            ma.Ma_IsSuccess = true;     //��ֵ���Ϊ���ɹ���
            //������
            EntitiesInfo.EmpAccount emp = Extend.LoginState.Admin.CurrentUser;
            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //����ǳ�ֵ
                if (type == 2)
                {                   
                    ma.Ma_Info = string.Format("ϵͳ����Ա{0}��{1}{2}��������ֵ{3}Ԫ", emp.Acc_Name, emp.Acc_AccName, mobi, money);
                    Business.Do<IAccounts>().MoneyIncome(ma);
                }
                //�����ת��
                if (type == 1)
                {
                    ma.Ma_Info = string.Format("ϵͳ����Ա{0}��{1}{2}���۳���{3}Ԫ", emp.Acc_Name, emp.Acc_AccName, mobi, money);
                    Business.Do<IAccounts>().MoneyPay(ma);
                }
                Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }

        protected void rblOpera_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.SelectedValue == "1")
            {
                lbOperator.Text = "-";
                if (tbMoney.Attributes["numlimit"] == null)
                {
                    tbMoney.Attributes.Add("numlimit", lbMoney.Text);
                }
                else
                {
                    tbMoney.Attributes["numlimit"] = lbMoney.Text;
                }
            }
            if (rbl.SelectedValue == "2")
            {
                lbOperator.Text = "+";
                if (tbMoney.Attributes["numlimit"] == null)
                {
                    tbMoney.Attributes.Add("numlimit", "0");
                }
                else
                {
                    tbMoney.Attributes["numlimit"] = "0";
                }
            }
        }

    }
}
