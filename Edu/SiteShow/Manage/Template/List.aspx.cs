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

namespace SiteShow.Manage.Template
{
    public partial class List : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //webģ���б�
            Common.Templates.TemplateBank[] tem = Business.Do<ServiceInterfaces.ITemplate>().WebTemplates();
            rtpTemplate.DataSource = tem;
            rtpTemplate.DataBind();
            //�ֻ�ģ���б�
            Common.Templates.TemplateBank[] mobitm = Business.Do<ServiceInterfaces.ITemplate>().MobiTemplates();
            rtpTempMobi.DataSource = mobitm;
            rtpTempMobi.DataBind();
        }

    }
}
