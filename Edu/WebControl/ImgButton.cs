using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	// Token: 0x02000004 RID: 4
	[ToolboxData("<{0}:ImgButton runat=server></{0}:ImgButton>")]
	[DefaultProperty("Text")]
	public class ImgButton : ImageButton
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000217C File Offset: 0x0000037C
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty.gif";
			base.OnInit(e);
		}
	}
}
