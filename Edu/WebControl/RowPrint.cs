using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	/// <summary>
	/// 数据行内的编辑按钮
	/// </summary>
	// Token: 0x02000006 RID: 6
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:RowPrint runat=server></{0}:RowPrint>")]
	public class RowPrint : ImageButton
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002228 File Offset: 0x00000428
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002255 File Offset: 0x00000455
		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		public string PrintPage
		{
			get
			{
				string text = (string)this.ViewState["PrintPage"];
				if (text != null)
				{
					return text;
				}
				return "";
			}
			set
			{
				this.ViewState["PrintPage"] = value;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002268 File Offset: 0x00000468
		protected override void OnInit(EventArgs e)
		{
			string str = "~/App_Themes/" + this.Page.Theme + "/Images/";
			this.ImageUrl = str + "empty22.gif";
			this.CssClass = base.GetType().Name;
			string text = this.Page.Request.Url.AbsoluteUri;
			if (text.IndexOf("/") > -1)
			{
				text = text.Substring(0, text.LastIndexOf("/") + 1);
			}
			base.Attributes.Add("PrintPage", text + this.PrintPage);
			this.ToolTip = "打印";
			base.OnInit(e);
		}
	}
}
