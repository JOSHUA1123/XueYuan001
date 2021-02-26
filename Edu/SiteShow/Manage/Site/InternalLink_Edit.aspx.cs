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

namespace SiteShow.Manage.Site
{
    public partial class InternalLink_Edit : Extend.CustomPage
    {
        private int id = Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        void fill()
        {
            try
            {
                EntitiesInfo.InternalLink mm;
                if (id != 0)
                {
                    mm = Business.Do<IInternalLink>().LinkSingle(id);
                    cbIsUse.Checked = mm.IL_IsUse;
                }
                else
                {
                    //如果是新增
                    mm = new EntitiesInfo.InternalLink();
                }
                tbTtl.Text = mm.IL_Title;
                tbName.Text = mm.IL_Name;
                tbUrl.Text = mm.IL_Url;
                ListItem li = ddlTarget.Items.FindByText(mm.IL_Target);
                if (li != null)
                {
                    ddlTarget.SelectedIndex = -1;
                    li.Selected = true;
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EntitiesInfo.InternalLink mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IInternalLink>().LinkSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new EntitiesInfo.InternalLink();
                }
                mm.IL_Title = tbTtl.Text;
                mm.IL_Name = tbName.Text;
                mm.IL_Url = tbUrl.Text;
                mm.IL_IsUse = cbIsUse.Checked;
                mm.IL_Target = ddlTarget.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
            try
            {
                //确定操作
                if (id == 0)
                {
                    Business.Do<IInternalLink>().LinkAdd(mm);
                }
                else
                {
                    Business.Do<IInternalLink>().LinkSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
