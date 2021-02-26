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
    public partial class Select : Extend.CustomPage
    {
        //��������վ������
        private string site = Common.Request.QueryString["site"].String;
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {
                plWeb.Visible = site == "web";
                plMobi.Visible = site == "mobi";
                BindData(null, null);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //ˢ��ģ����Ϣ
            Common.Template.RefreshTemplate();
            //webģ���б�
            if (site == "web")
            {
                Common.Templates.TemplateBank[] tem = Business.Do<ServiceInterfaces.ITemplate>().WebTemplates();
                Common.Template.ForWeb.SetCurrent(org.Org_Template);
                rtpTemplate.DataSource = tem;
                rtpTemplate.DataBind();
            }
            //�ֻ�ģ���б�
            if (site == "mobi")
            {
                Common.Templates.TemplateBank[] mobitm = Business.Do<ServiceInterfaces.ITemplate>().MobiTemplates();
                Common.Template.ForMobile.SetCurrent(org.Org_TemplateMobi);
                rtpTempMobi.DataSource = mobitm;
                rtpTempMobi.DataBind();
            }
        }
        /// <summary>
        /// ѡ��ǰģ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelectWeb_Click(object sender, EventArgs e)
        {            
            LinkButton ub = (LinkButton)sender;
            string tag = ub.CommandArgument;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Business.Do<ServiceInterfaces.ITemplate>().SetWebCurr(org.Org_ID,tag); 
            BindData(null, null);           
        }
        /// <summary>
        /// ѡ��ǰ�ֻ�ģ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelectMobi_Click(object sender, EventArgs e)
        {
            LinkButton ub = (LinkButton)sender;
            string tag = ub.CommandArgument;
            EntitiesInfo.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Business.Do<ServiceInterfaces.ITemplate>().SetMobiCurr(org.Org_ID, tag);
            BindData(null, null);
        }
    }
}
