using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
    [ToolboxData("<{0}:DeleteButton runat=server></{0}:DeleteButton>")]
    [DefaultProperty("Text")]
    public class DeleteButton : ImageButton
    {
        [DefaultValue("您是否确定删该项？")]
        [Browsable(true)]
        [Description("点击“删除”按钮时的提示，如果为空，则不提示；")]
        [Category("Appearance")]
        public string DeleteToolTip
        {
            get
            {
                if (this.ViewState["DeleteToolTip"] == null)
                    return "您是否确定删该项？";
                return (string)this.ViewState["DeleteToolTip"];
            }
            set
            {
                if (value == null)
                    return;
                value = value.Replace("\"", " ");
                value = value.Replace("'", " ");
                this.ViewState["DeleteToolTip"] = (object)value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            int num = this.Text.Trim() == "" ? 1 : 0;
            if (this.CssClass == "")
                this.CssClass = "btnDeleteWin";
            string str = "";
            if (this.DeleteToolTip.Trim() != "")
                str = "return confirm('" + this.DeleteToolTip + "');";
            this.OnClientClick = str;
            this.CssClass = "btnDeleteWin";
            this.ImageUrl = "~/App_Themes/" + this.Page.Theme + "/Images/" + "empty.gif";
            base.OnInit(e);
        }
    }
}
