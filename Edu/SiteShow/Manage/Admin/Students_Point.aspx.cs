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
    public partial class Students_Point : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }           
        }
        private void fill()
        {
            EntitiesInfo.Accounts ea;
            if (id == 0) return;
            ea = Business.Do<IAccounts>().AccountsSingle(id);
            //Ա������
            this.lbName.Text = ea.Ac_Name;
            //�˻��������
            lbPoint.Text = ea.Ac_Point.ToString();
            lbPointmax.Text = ea.Ac_PointAmount.ToString();

        }

        /// <summary>
        /// ��ֵ��۷�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMoney_Click(object sender, EventArgs e)
        {            
            if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("�ǹ���Ա��Ȩ�˲���Ȩ�ޣ�");
            EntitiesInfo.Accounts st = Business.Do<IAccounts>().AccountsSingle(id);
            if (st == null) throw new Exception("��ǰ��Ϣ�����ڣ�");
            //��������
            int type = 2;
            int.TryParse(rblOpera.SelectedItem.Value, out type);
            //����������
            int point = 0;
            int.TryParse(tbPoint.Text, out point);
            //��������
            EntitiesInfo.PointAccount ma = new PointAccount();
            ma.Pa_Value = point;
            ma.Pa_Remark = tbRemark.Text.Trim();
            ma.Ac_ID = st.Ac_ID;
            ma.Pa_Source = "����Ա����";
            //��ֵ��ʽ������Ա��ֵ
            ma.Pa_From = 1;
            //������
            EntitiesInfo.EmpAccount emp=Extend.LoginState.Admin.CurrentUser;
            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //����ǳ�ֵ
                if (type == 2)
                {                   
                    ma.Pa_Info = string.Format("����Ա{0}��{1}{2}��������ֵ{3}��", emp.Acc_Name, emp.Acc_AccName, mobi, point);
                    Business.Do<IAccounts>().PointAdd(ma);
                }
                //�����ת��
                if (type == 1)
                {
                    ma.Pa_Info = string.Format("����Ա{0}��{1}{2}���۳���{3}��", emp.Acc_Name, emp.Acc_AccName, mobi, point);
                    Business.Do<IAccounts>().PointPay(ma);
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
                if (tbPoint.Attributes["numlimit"] == null)
                {
                    tbPoint.Attributes.Add("numlimit", lbPoint.Text);
                }
                else
                {
                    tbPoint.Attributes["numlimit"] = lbPoint.Text;
                }
            }
            if (rbl.SelectedValue == "2")
            {
                lbOperator.Text = "+";
                if (tbPoint.Attributes["numlimit"] == null)
                {
                    tbPoint.Attributes.Add("numlimit", "0");
                }
                else
                {
                    tbPoint.Attributes["numlimit"] = "0";
                }
            }
        }       

    }
}
