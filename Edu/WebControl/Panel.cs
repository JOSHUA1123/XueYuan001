using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:WebCustomControl1 runat=server></{0}:WebCustomControl1>")]
    public class Panel : System.Web.UI.WebControls.Panel, INamingContainer
    {
        [DefaultValue("")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        public string Text
        {
            get
            {
                return (string)this.ViewState["Text"] ?? string.Empty;
            }
            set
            {
                this.ViewState["Text"] = (object)value;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        [Bindable(true)]
        public string More
        {
            get
            {
                return (string)this.ViewState["More"] ?? string.Empty;
            }
            set
            {
                this.ViewState["More"] = (object)value;
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.RenderHead(output);
            this.CssClass = "BoxContext";
            base.Render(output);
            this.RenderEnd(output);
        }

        protected void RenderHead(HtmlTextWriter writer)
        {
            string str1 = "" + "<div class=\"ShowBox\">" + "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                      <tr>" + "<td class=\"MoreBoxTitle\">" + this.Text + "</td>" + "<td class=\"MoreRight\">";
            if (this.More != "")
                str1 = str1 + "<a href=\"" + this.More + "\">更多&gt;&gt;</a>";
            string str2 = str1 + "</td></tr></table>";
            writer.Write(str2);
        }

        protected void RenderEnd(HtmlTextWriter writer)
        {
            string str = "" + "</div>";
            writer.Write(str);
        }
    }
}
