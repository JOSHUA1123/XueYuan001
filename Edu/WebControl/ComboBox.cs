using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebControlShow
{
	// Token: 0x0200000A RID: 10
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:ComboBox runat=server></{0}:ComboBox>")]
	public class ComboBox : CompositeControl, IPostBackEventHandler, IPostBackDataHandler
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000024A7 File Offset: 0x000006A7
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000024BA File Offset: 0x000006BA
		[DefaultValue("")]
		[Bindable(true)]
		[Category("Appearance")]
		public string Text
		{
			get
			{
				this.EnsureChildControls();
				return this.tbText.Text;
			}
			set
			{
				this.EnsureChildControls();
				this.tbText.Text = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000024CE File Offset: 0x000006CE
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000024E1 File Offset: 0x000006E1
		public new Unit Height
		{
			get
			{
				this.EnsureChildControls();
				return this.ddlList.Height;
			}
			set
			{
				this.EnsureChildControls();
				this.ddlList.Height = value;
				this.tbText.Height = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002501 File Offset: 0x00000701
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002533 File Offset: 0x00000733
		public new Unit Width
		{
			get
			{
				this.EnsureChildControls();
				if (!(this.ddlList.Width == Unit.Empty))
				{
					return this.ddlList.Width;
				}
				return 100;
			}
			set
			{
				this.EnsureChildControls();
				this.ddlList.Width = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002547 File Offset: 0x00000747
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002572 File Offset: 0x00000772
		[Category("Appearance")]
		[DefaultValue("")]
		[Bindable(true)]
		public bool AutoPostBack
		{
			get
			{
				return this.ViewState["AutoPostBack"] != null && Convert.ToBoolean(this.ViewState["AutoPostBack"]);
			}
			set
			{
				this.ViewState["AutoPostBack"] = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000258A File Offset: 0x0000078A
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000025B5 File Offset: 0x000007B5
		[Bindable(true)]
		[DefaultValue("")]
		[Category("Appearance")]
		public new bool Enabled
		{
			get
			{
				return this.ViewState["Enabled"] == null || Convert.ToBoolean(this.ViewState["Enabled"]);
			}
			set
			{
				this.ViewState["Enabled"] = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000025CD File Offset: 0x000007CD
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000025F8 File Offset: 0x000007F8
		[DefaultValue("")]
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsChange
		{
			get
			{
				return this.ViewState["IsChange"] == null || Convert.ToBoolean(this.ViewState["IsChange"]);
			}
			set
			{
				if (!value)
				{
					this.tbText.Attributes.Add("style", "display:none");
				}
				this.ViewState["IsChange"] = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000262D File Offset: 0x0000082D
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002658 File Offset: 0x00000858
		[Category("Appearance")]
		[Bindable(true)]
		[DefaultValue("")]
		public bool IsEmpty
		{
			get
			{
				return this.ViewState["IsEmpty"] != null && Convert.ToBoolean(this.ViewState["IsEmpty"]);
			}
			set
			{
				this.ViewState["IsEmpty"] = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002670 File Offset: 0x00000870
		public ListItemCollection Items
		{
			get
			{
				this.EnsureChildControls();
				return this.ddlList.Items;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002683 File Offset: 0x00000883
		// (set) Token: 0x0600002A RID: 42 RVA: 0x00002696 File Offset: 0x00000896
		public string DataTextField
		{
			get
			{
				this.EnsureChildControls();
				return this.ddlList.DataTextField;
			}
			set
			{
				this.EnsureChildControls();
				this.ddlList.DataTextField = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000026AA File Offset: 0x000008AA
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000026BD File Offset: 0x000008BD
		public string DataValueField
		{
			get
			{
				this.EnsureChildControls();
				return this.ddlList.DataValueField;
			}
			set
			{
				this.EnsureChildControls();
				this.ddlList.DataValueField = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000026D1 File Offset: 0x000008D1
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000026E4 File Offset: 0x000008E4
		[DefaultValue(0)]
		[Bindable(true)]
		[Category("Behavior")]
		public int SelectedIndex
		{
			get
			{
				this.EnsureChildControls();
				return this.ddlList.SelectedIndex;
			}
			set
			{
				this.EnsureChildControls();
				if (this.ddlList.Items.Count > value && value > -1 && this.ddlList.Items.Count > 0)
				{
					ListItem listItem = this.ddlList.Items[value];
					if (listItem != null)
					{
						this.ddlList.SelectedIndex = -1;
						listItem.Selected = true;
					}
				}
				this.currentIndex = value;
			}
		}

		/// <summary>
		/// 当前选中的值
		/// </summary>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002750 File Offset: 0x00000950
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000027C8 File Offset: 0x000009C8
		public string SelectedValue
		{
			get
			{
				this.EnsureChildControls();
				if (!this.Enabled)
				{
					return this.tbValue.Text;
				}
				if (!this.IsChange)
				{
					return this.ddlList.SelectedItem.Value;
				}
				ListItem listItem = this.ddlList.Items.FindByText(this.tbText.Text.Trim());
				if (listItem != null)
				{
					return listItem.Value;
				}
				return this.tbValue.Text;
			}
			set
			{
				this.EnsureChildControls();
				this.tbValue.Text = value;
				ListItem listItem = this.ddlList.Items.FindByValue(value);
				if (listItem != null)
				{
					this.ddlList.SelectedIndex = -1;
					listItem.Selected = true;
					this.tbText.Text = listItem.Text;
					return;
				}
				if (this.ddlList.Items.Count > 0)
				{
					this.tbText.Text = this.ddlList.Items[0].Text;
					this.tbValue.Text = this.ddlList.Items[0].Value;
					return;
				}
				this.tbText.Text = "";
			}
		}

		/// <summary>
		/// 当前选中项的文本
		/// </summary>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002888 File Offset: 0x00000A88
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002900 File Offset: 0x00000B00
		public string SelectedText
		{
			get
			{
				this.EnsureChildControls();
				if (!this.Enabled)
				{
					return this.tbText.Text;
				}
				if (!this.IsChange)
				{
					return this.ddlList.SelectedItem.Text;
				}
				ListItem listItem = this.ddlList.Items.FindByText(this.tbText.Text.Trim());
				if (listItem != null)
				{
					return listItem.Text;
				}
				return this.tbText.Text;
			}
			set
			{
				this.EnsureChildControls();
				this.tbText.Text = value;
				ListItem listItem = this.ddlList.Items.FindByText(value);
				if (listItem != null)
				{
					this.ddlList.SelectedIndex = -1;
					listItem.Selected = true;
					this.tbValue.Text = listItem.Value;
					return;
				}
				this.tbValue.Text = "";
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002969 File Offset: 0x00000B69
		// (set) Token: 0x06000034 RID: 52 RVA: 0x0000297C File Offset: 0x00000B7C
		public virtual object DataSource
		{
			get
			{
				this.EnsureChildControls();
				return this.ddlList.DataSource;
			}
			set
			{
				this.EnsureChildControls();
				this.ddlList.DataSource = value;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002990 File Offset: 0x00000B90
		public new void DataBind()
		{
			this.ddlList.DataBind();
			if (this.ddlList.Items.Count > 0)
			{
				this.tbText.Text = this.ddlList.Items[0].Text;
				this.tbValue.Text = this.ddlList.Items[0].Value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000036 RID: 54 RVA: 0x00002A00 File Offset: 0x00000C00
		// (remove) Token: 0x06000037 RID: 55 RVA: 0x00002A38 File Offset: 0x00000C38
		public event EventHandler SelectedIndexChanged;

		// Token: 0x06000038 RID: 56 RVA: 0x00002A70 File Offset: 0x00000C70
		protected void OnSelectedIndexChanged(EventArgs e)
		{
			this.tbValue.Text = this.ddlList.SelectedValue;
			this.tbText.Text = this.ddlList.SelectedItem.Text;
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, e);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002AC3 File Offset: 0x00000CC3
		protected void OnSelectedTextChanged(EventArgs e)
		{
			this.SelectedText = this.tbText.Text;
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, e);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002AEC File Offset: 0x00000CEC
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			this.ddlList = new DropDownList();
			this.ddlList.Attributes.Add("id", this.UniqueID + "_List");
			this.ddlList.Width = this.Width;
			this.Controls.Add(this.ddlList);
			this.tbText = new TextBox();
			this.tbText.Attributes.Add("id", this.UniqueID + "_Text");
			this.tbText.Width = 1;
			this.tbText.Attributes.Add("style", "position: absolute;z-index: 10;");
			this.Controls.Add(this.tbText);
			this.tbValue = new TextBox();
			this.tbValue.Attributes.Add("id", this.UniqueID + "_Value");
			this.tbValue.Width = 1;
			this.tbValue.Visible = this.Enabled;
			this.tbValue.Attributes.Add("style", "display:none");
			this.Controls.Add(this.tbValue);
			base.ChildControlsCreated = true;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002C48 File Offset: 0x00000E48
		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (this.AutoPostBack)
			{
				this.ddlList.Attributes.Add("onchange", "javascript:" + this.Page.ClientScript.GetPostBackEventReference(this, this.UniqueID + "_List"));
				this.tbText.Attributes.Add("onchange", "javascript:" + this.Page.ClientScript.GetPostBackEventReference(this, this.UniqueID + "_Text"));
			}
			if (!this.IsChange)
			{
				this.tbText.Attributes.Add("style", "display:none");
			}
			if (!this.IsEmpty)
			{
				this.tbText.Attributes.Add("empty", "false");
			}
			this.tbText.Height = this.Height;
			if (!this.Enabled)
			{
				this.tbText.Attributes.Add("style", "display:none");
			}
			this.tbText.RenderControl(writer);
			this.tbValue.RenderControl(writer);
			if (!this.Enabled)
			{
				this.ddlList.Attributes.Add("disabled", "disabled");
				if (this.ddlList.Items.Count >= this.currentIndex && this.ddlList.Items.Count > 0 && this.currentIndex > 0)
				{
					ListItem listItem = this.ddlList.Items[this.currentIndex];
					if (listItem != null)
					{
						this.ddlList.SelectedIndex = -1;
						listItem.Selected = true;
					}
				}
			}
			this.ddlList.RenderControl(writer);
			string text = "<script type=\"text/javascript\">{0}</script>";
			string arg = "ComboBoxInit(\"" + this.ID + "\");";
			text = string.Format(text, arg);
			writer.Write(text);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002E29 File Offset: 0x00001029
		protected override void OnInit(EventArgs e)
		{
			this.Page.RegisterRequiresControlState(this);
			base.OnInit(e);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002E3E File Offset: 0x0000103E
		protected override object SaveControlState()
		{
			base.SaveControlState();
			if (this.currentIndex == 0)
			{
				return null;
			}
			return this.currentIndex;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E5C File Offset: 0x0000105C
		protected override void LoadControlState(object state)
		{
			int selectedIndex = this.SelectedIndex;
			if (state != null)
			{
				this.currentIndex = (int)state;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E74 File Offset: 0x00001074
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			if (eventArgument.IndexOf("_") > -1)
			{
				string a = eventArgument.Substring(eventArgument.LastIndexOf("_") + 1);
				if (a == "List")
				{
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
				if (a == "Text")
				{
					this.OnSelectedTextChanged(EventArgs.Empty);
					return;
				}
			}
			else
			{
				this.OnSelectedIndexChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002EDF File Offset: 0x000010DF
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002EEB File Offset: 0x000010EB
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		// Token: 0x04000004 RID: 4
		private DropDownList ddlList;

		// Token: 0x04000005 RID: 5
		private TextBox tbText;

		// Token: 0x04000006 RID: 6
		private TextBox tbValue;

		// Token: 0x04000008 RID: 8
		private int currentIndex;
	}
}
