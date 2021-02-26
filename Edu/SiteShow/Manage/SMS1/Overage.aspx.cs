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
using System.Xml;

namespace SiteShow.Manage.SMS1
{
    public partial class Overage : Extend.CustomPage
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
                btnEnter_Click(null, null);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            ///当前短信平台
            lbPlate.Text = SMS.Config.Current.Remarks;
            //短信帐号           
            lbName.Text = SMS.Config.Current.User;       
            //密码
            lbPw.Text = SMS.Config.Current.Password;
           
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                int result = SMS.Gatway.Service.Query();
                lbOverNumber.Text = "" + result + "条";
            }
            catch (Exception ex)
            {
                lbOverNumber.Text = ex.Message;
            }
           
        }
    }
}
