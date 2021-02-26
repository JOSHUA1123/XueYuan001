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

namespace SiteShow.Manage.View
{
    public partial class Contents : Extend.CustomPage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlColumnBind();
            }
        }
        /// <summary>
        /// 栏目下拉绑定
        /// </summary>
        private void ddlColumnBind()
        {
            //所属机构的所有课程
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            EntitiesInfo.Columns[] nc = Business.Do<IColumns>().All(org.Org_ID, true);
            this.ddlColumns.DataSource = nc;
            this.ddlColumns.DataTextField = "Col_Name";
            this.ddlColumns.DataValueField = "Col_ID";
            this.ddlColumns.DataBind(); 
        }

    }
}
