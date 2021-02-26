using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using ServiceInterfaces;

namespace SiteShow.Manage.Student
{
    public partial class Safe : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts st = this.Master.Account;
            if (st == null) return;
            this.Form.DefaultButton = this.btnEnter.UniqueID;
            if (!IsPostBack)
            {
                fill();
            }
        }

        private void fill()
        {
            EntitiesInfo.Accounts acc = id == 0 ? this.Master.Account : Business.Do<IAccounts>().AccountsSingle(id);
            if (acc == null) return;
            this.EntityBind(acc);
            
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts obj = this.Master.Account;
            try
            {
                string pw = this.tbOldPw.Text.Trim();
                obj = Business.Do<IAccounts>().AccountsSingle(obj.Ac_AccName, pw, obj.Org_ID);
                this.lbShow.Visible = obj == null;
                if (obj == null) return;
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    string md5 = Common.Request.Controls[tbPw1].MD5;
                    obj.Ac_Pw = md5;
                }
                Business.Do<IAccounts>().AccountsSave(obj);
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
            
        }
        /// <summary>
        /// 保存安全问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSafeQues_Click(object sender, EventArgs e)
        {
            EntitiesInfo.Accounts obj = this.Master.Account;
            obj = this.EntityFill(obj) as EntitiesInfo.Accounts;
            Business.Do<IAccounts>().AccountsSave(obj);
            this.Alert("操作成功！");
        }
    }
}