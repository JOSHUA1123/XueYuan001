using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiSha.WebControl
{
	/// <summary>
	/// 数据行内的删除按钮
	/// </summary>
	// Token: 0x02000003 RID: 3
	[ToolboxData("<{0}:RowDelete runat=server></{0}:RowDelete>")]
	[DefaultProperty("Text")]
	public class RowDelete : ImageButton
	{
		// Token: 0x06000008 RID: 8 RVA: 0x0000210C File Offset: 0x0000030C
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty22.gif";
			this.CssClass = base.GetType().Name;
			this.ToolTip = "删除";
			this.OnClientClick = "return confirm('是否确定删除?')";
			base.OnInit(e);
		}
	}
}
