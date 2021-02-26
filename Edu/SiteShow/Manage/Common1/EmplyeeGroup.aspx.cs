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
    public partial class EmplyeeGroup : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                BindData(null, null);
            }
        }
       
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //
                EntitiesInfo.EmpAccount[] eas = null;
                eas = Business.Do<IEmpGroup>().GetAll4Group(id);
                foreach (EntitiesInfo.EmpAccount ea in eas)
                {
                    ea.Acc_Photo = Upload.Get["Employee"].Virtual + ea.Acc_Photo;
                    if (!ea.Acc_IsOpenTel)
                        ea.Acc_Tel = "";
                    if (!ea.Acc_IsOpenMobile)
                        ea.Acc_MobileTel = "";
                }
                this.rpList.DataSource = eas;
                rpList.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
