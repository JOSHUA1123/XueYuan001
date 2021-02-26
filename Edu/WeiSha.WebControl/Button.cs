using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Button runat=server></{0}:Button>")]
    public class Button : System.Web.UI.WebControls.Button
    {
        protected override void OnInit(EventArgs e)
        {
            this.CssClass = "btnButton";
            base.OnInit(e);
        }
    }
}
