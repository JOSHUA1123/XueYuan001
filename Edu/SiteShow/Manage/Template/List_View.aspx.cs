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



namespace SiteShow.Manage.Template
{
    public partial class List_View : Extend.CustomPage
    {
        //ģ���ʶ
        private string tag = Common.Request.QueryString["tag"].String;
        //ģ������
        private string type = Common.Request.QueryString["type"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
           
        }
       
        private void fill()
        {
            Common.Templates.TemplateBank tmp = Common.Template.GetTemplate(type, tag);
            ltName.Text = tmp.Name;
            ltAuthor.Text = tmp.Author;
            ltPhone.Text = tmp.Phone;
            ltQQ.Text = tmp.QQ;
            ltCrtTime.Text = tmp.CrtTime.ToString();
            ltIntro.Text = tmp.Intro;
            imgShow.Src = tmp.Logo + "?s=" + DateTime.Now.Ticks;
            lbFileName.Text = tmp.Path.Virtual;
        }        
       
    }
}
