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
using System.Collections.Generic;

namespace SiteShow.Manage.View
{
    public partial class News_Preview : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        private string _uppath = "news";
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
             EntitiesInfo.Article mm= Business.Do<IContents>().ArticleSingle(id);
             if (mm == null) return;
             ltTitle.Text = mm.Art_Title;
             ltAuthor.Text = mm.Art_Author == "" ? "无" : mm.Art_Author;
             ltSource.Text = mm.Art_Source == "" ? "无" : mm.Art_Source;
             ltCrtTime.Text = ((DateTime)mm.Art_CrtTime).ToShortDateString();
             ltDetails.Text = mm.Art_Details;
             ltKeyword.Text = mm.Art_Keywords;
             artFoot.Visible = ltKeyword.Text.Trim() != "";
             //唯一标识
             ViewState["UID"] = mm.Art_Uid;
            //
             AccessoryBind();
        }
        /// <summary>
        /// 绑定附件
        /// </summary>
        protected void AccessoryBind()
        {
            List<EntitiesInfo.Accessory> acs = Business.Do<IAccessory>().GetAll(this.getUID());
            foreach (Accessory ac in acs)
            {
                ac.As_FileName = Upload.Get[_uppath].Virtual + ac.As_FileName;
            }
            dlAcc.DataSource = acs;
            dlAcc.DataKeyField = "As_Id";
            dlAcc.DataBind();
            dlAcc.Visible = dlAcc.Items.Count > 0;
        }
    }
}
