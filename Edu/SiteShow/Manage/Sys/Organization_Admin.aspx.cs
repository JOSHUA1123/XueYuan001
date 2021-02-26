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
using System.Text.RegularExpressions;
using WeiSha.WebControl;

namespace SiteShow.Manage.Sys
{
    public partial class Organization_Admin : Extend.CustomPage
    {
        //����id
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //��������Ա��ɫ��id
        protected string superid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Position super = Business.Do<IPosition>().GetAdmin(id);
            superid = super.Posi_Id.ToString();
            if (!this.IsPostBack)
            {
                BindData(null,null);
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
                //��ǰѡ��Ļ���id
                int orgId = (int)Extend.LoginState.Admin.CurrentUser.Org_ID;
                EmpAccount[] eas = null;
                eas = Business.Do<IEmployee>().GetAll4Org(id, true, "");

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "acc_id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// �޸��Ƿ���ʾ��״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                //��ȡԱ��id
                int accid = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                EntitiesInfo.EmpAccount entity = Business.Do<IEmployee>().GetSingle(accid);
                //�ֳ��Ĺ����λ
                EntitiesInfo.Position posi = Business.Do<IPosition>().GetAdmin(id);

                entity.Posi_Id = posi.Posi_Id;
                entity.Posi_Name = posi.Posi_Name;  
                Business.Do<IEmployee>().Save(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// �жϵ�ǰԱ���ǲ��Ƿֳ�����Ա
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        protected bool GetAminState(string accid)
        {
           return  Business.Do<IEmployee>().IsAdmin(Convert.ToInt32(accid));
        }
    }
}
