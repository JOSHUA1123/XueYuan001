using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	// Token: 0x02000012 RID: 18
	[ToolboxData("<{0}:EnterButton runat=server></{0}:EnterButton>")]
	[DefaultProperty("Text")]
	public class EnterButton : ImageButton
	{
		// Token: 0x06000094 RID: 148 RVA: 0x000045E0 File Offset: 0x000027E0
		protected override void OnInit(EventArgs e)
		{
			this.CssClass = "btnEnterButton";
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
		}
	}
}
