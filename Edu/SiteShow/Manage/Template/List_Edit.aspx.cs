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
    public partial class List_Edit : Extend.CustomPage
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
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
       
        private void fill()
        {
            Common.Templates.TemplateBank tmp = Common.Template.GetTemplate(type,tag);
            tbName.Text = tmp.Name;
            tbAuthor.Text = tmp.Author;
            tbPhone.Text = tmp.Phone;
            tbQQ.Text = tmp.QQ;
            tbCrtTime.Text = tmp.CrtTime.ToString("yyyy-MM-dd");
            tbIntro.Text = tmp.Intro;
            imgShow.Src = tmp.Logo + "?s=" + DateTime.Now.Ticks;
            lbFileName.Text = tmp.Path.Virtual;
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Common.Templates.TemplateBank tmp = Common.Template.GetTemplate(type, tag);
            tmp.Name = tbName.Text.Trim();
            tmp.Author = tbAuthor.Text.Trim();
            tmp.Phone = tbPhone.Text.Trim();
            tmp.QQ = tbQQ.Text.Trim();
            tmp.CrtTime = Convert.ToDateTime(tbCrtTime.Text);
            tmp.Intro = tbIntro.Text.Trim();
            //ͼƬ
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.NewName = "logo.jpg";
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAs(this.Server.MapPath(tmp.Logo));
                    Common.Images.FileTo.Zoom(this.Server.MapPath(tmp.Logo), 200, 200, false);
                    imgShow.Src = tmp.Logo + "?s=" + DateTime.Now.Ticks;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            tmp.Save();
            Common.Template.RefreshTemplate();
            Master.AlertCloseAndRefresh("�����ɹ���");
        }
       
    }
}
