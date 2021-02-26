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



namespace SiteShow.Manage.Site
{
    public partial class SSOApi_Edit : Extend.CustomPage
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
            EntitiesInfo.SingleSignOn entity = id == 0 ? new EntitiesInfo.SingleSignOn() : Business.Do<ISSO>().GetSingle(id);
            if (entity == null) return;
            if (id == 0) entity.SSO_APPID = Common.Request.UniqueID();
            this.EntityBind(entity);           
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.SingleSignOn entity = id == 0 ? new EntitiesInfo.SingleSignOn() : Business.Do<ISSO>().GetSingle(id);
            entity = this.EntityFill(entity) as EntitiesInfo.SingleSignOn;
            entity.SSO_APPID = SSO_APPID.Text;
            //域名全部转小写
            entity.SSO_Domain = entity.SSO_Domain.ToLower();
            try
            {
                if (id == 0) Business.Do<ISSO>().Add(entity);
                if (id > 0) Business.Do<ISSO>().Save(entity);
                Master.AlertCloseAndRefresh("操作完成");               
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
       
    }
}
