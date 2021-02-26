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
using WeiSha.WebControl;


namespace SiteShow.Manage.Common1
{
    public partial class AddressSort_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                EntitiesInfo.AddressSort mm;
                if (id != 0)
                {
                    mm = Business.Do<IAddressList>().SortSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new EntitiesInfo.AddressSort();
                }
                tbName.Text = mm.Ads_Name;
                //是否使用
                cbIsUse.Checked = mm.Ads_IsUse;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.AddressSort mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IAddressList>().SortSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new EntitiesInfo.AddressSort();
                }
                mm.Ads_Name = tbName.Text.Trim();
                mm.Ads_IsUse = cbIsUse.Checked;
                mm.Acc_Id = Extend.LoginState.Admin.CurrentUser.Acc_Id;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                //确定操作
                if (id == 0)
                {
                    Business.Do<IAddressList>().SortAdd(mm);
                }
                else
                {
                    Business.Do<IAddressList>().SortSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
