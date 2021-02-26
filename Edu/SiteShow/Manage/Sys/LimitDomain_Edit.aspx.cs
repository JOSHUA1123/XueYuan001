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
    public partial class LimitDomain_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {              
                fill();
            }   
        }
        private void fill()
        {
            EntitiesInfo.LimitDomain ld = id == 0 ? new EntitiesInfo.LimitDomain() : Business.Do<ILimitDomain>().DomainSingle(id);
            if (id > 0) this.EntityBind(ld);            
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.LimitDomain ld = id == 0 ? new EntitiesInfo.LimitDomain() : Business.Do<ILimitDomain>().DomainSingle(id);
            ld = this.EntityFill(ld) as EntitiesInfo.LimitDomain;
            if (Business.Do<ILimitDomain>().DomainIsExist(ld))
            {
                Message.Alert("当前域名已经存在！");
            }
            else
            {
                try
                {
                    if (id == 0)
                    {
                        Business.Do<ILimitDomain>().DomainAdd(ld);
                    }
                    else
                    {
                        Business.Do<ILimitDomain>().DomainSave(ld);
                    }
                    Master.AlertCloseAndRefresh("操作成功！");
                }
                catch (Exception ex)
                {
                    Message.ExceptionShow(ex);
                }
            }         
        }
       
    }
}
