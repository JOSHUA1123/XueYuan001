using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	/// <summary>
	/// 数据行内的查看按钮
	/// </summary>
	// Token: 0x02000008 RID: 8
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:RowView runat=server></{0}:RowView>")]
	public class RowView : ImageButton
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002378 File Offset: 0x00000578
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty22.gif";
			this.CssClass = base.GetType().Name;
			this.ToolTip = "查看";
			base.OnInit(e);
		}
	}
}
