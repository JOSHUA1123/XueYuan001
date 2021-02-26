using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebControlShow.Tree;

namespace WebControlShow
{
	// Token: 0x0200000E RID: 14
	[ToolboxData("<{0}:MenuTree runat=server></{0}:MenuTree>")]
	[DefaultProperty("Text")]
    public class MenuTree : System.Web.UI.WebControls.WebControl
	{
		/// <summary>
		/// 数据源
		/// </summary>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003193 File Offset: 0x00001393
		// (set) Token: 0x06000053 RID: 83 RVA: 0x0000319B File Offset: 0x0000139B
		[Browsable(false)]
		public object DataSource
		{
			get
			{
				return this._datasource;
			}
			set
			{
				this._datasource = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000031A4 File Offset: 0x000013A4
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000031D1 File Offset: 0x000013D1
		[Category("属性字段")]
		[Localizable(true)]
		[Description("树形菜单的标题，即根节点")]
		[DefaultValue("")]
		[Bindable(true)]
		public string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000031E4 File Offset: 0x000013E4
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00003211 File Offset: 0x00001411
		[Bindable(true)]
		[DefaultValue("")]
		[Description("数据源的主键字段名")]
		[Localizable(true)]
		[Category("属性字段")]
		public string IdKeyName
		{
			get
			{
				string text = (string)this.ViewState["IdKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["IdKeyName"] = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003224 File Offset: 0x00001424
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003251 File Offset: 0x00001451
		[Category("属性字段")]
		[Bindable(true)]
		[Localizable(true)]
		[Description("标识当前节点的父节点ID的字段名")]
		[DefaultValue("0")]
		public string ParentIdKeyName
		{
			get
			{
				string text = (string)this.ViewState["ParentIdKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["ParentIdKeyName"] = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003264 File Offset: 0x00001464
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003292 File Offset: 0x00001492
		[Bindable(true)]
		[DefaultValue("0")]
		[Description("根节点的id值")]
		[Localizable(true)]
		[Category("属性字段")]
		public int Root
		{
			get
			{
				string text = (string)this.ViewState["Root"];
				if (text != null)
				{
					return Convert.ToInt32(text);
				}
				return 0;
			}
			set
			{
				this.ViewState["Root"] = value.ToString();
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000032AC File Offset: 0x000014AC
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000032D9 File Offset: 0x000014D9
		[Bindable(true)]
		[Description("用排序的字段名")]
		[Category("属性字段")]
		[DefaultValue("")]
		[Localizable(true)]
		public string TaxKeyName
		{
			get
			{
				string text = (string)this.ViewState["TaxKeyName"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["TaxKeyName"] = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000032EC File Offset: 0x000014EC
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003319 File Offset: 0x00001519
		[Category("属性字段")]
		[Bindable(true)]
		[Description("标识节点类型的字段; 字段内容node标识为普通文字，link标识为超链接；默认为link")]
		[DefaultValue("")]
		[Localizable(true)]
		public string TypeKeyName
		{
			get
			{
				string text = (string)this.ViewState["TypeKeyName"];
				if (text != null)
				{
					return text;
				}
				return "";
			}
			set
			{
				this.ViewState["TypeKeyName"] = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000332C File Offset: 0x0000152C
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003359 File Offset: 0x00001559
		[Bindable(true)]
		[Description("标识节点是否禁用")]
		[Category("属性字段")]
		[DefaultValue("")]
		[Localizable(true)]
		public string IsUseKeyName
		{
			get
			{
				string text = (string)this.ViewState["IsUseKeyName"];
				if (text != null)
				{
					return text;
				}
				return "";
			}
			set
			{
				this.ViewState["IsUseKeyName"] = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000336C File Offset: 0x0000156C
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003399 File Offset: 0x00001599
		[Description("标识节点是否显示")]
		[Localizable(true)]
		[Bindable(true)]
		[Category("属性字段")]
		[DefaultValue("")]
		public string IsShowKeyName
		{
			get
			{
				string text = (string)this.ViewState["IsShowKeyName"];
				if (text != null)
				{
					return text;
				}
				return "";
			}
			set
			{
				this.ViewState["IsShowKeyName"] = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000033AC File Offset: 0x000015AC
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000033D9 File Offset: 0x000015D9
		[DefaultValue("18")]
		[Description("节点的高度")]
		[Category("树节点属性")]
		[Localizable(true)]
		[Bindable(true)]
		public string NodeHeight
		{
			get
			{
				string text = (string)this.ViewState["NodeHeight"];
				if (text != null)
				{
					return text;
				}
				return "18";
			}
			set
			{
				this.ViewState["NodeHeight"] = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000033EC File Offset: 0x000015EC
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003419 File Offset: 0x00001619
		[Description("超链接的目标")]
		[Bindable(true)]
		[Category("树节点属性")]
		[DefaultValue("_blank")]
		[Localizable(true)]
		public string Target
		{
			get
			{
				string text = (string)this.ViewState["Target"];
				if (text != null)
				{
					return text;
				}
				return "_blank";
			}
			set
			{
				this.ViewState["Target"] = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000342C File Offset: 0x0000162C
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003459 File Offset: 0x00001659
		[Category("树节点属性")]
		[DefaultValue("")]
		[Description("图片资源的路径，建议为相对路径")]
		[Bindable(true)]
		[Localizable(true)]
		public string SourcePath
		{
			get
			{
				string text = (string)this.ViewState["SourcePath"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["SourcePath"] = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000346C File Offset: 0x0000166C
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003499 File Offset: 0x00001699
		[DefaultValue("")]
		[Localizable(true)]
		[Description("用于节点上显示的名称的数据字段")]
		[Bindable(true)]
		[Category("树节点属性")]
		public string DataTextField
		{
			get
			{
				string text = (string)this.ViewState["DataTextField"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["DataTextField"] = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000034AC File Offset: 0x000016AC
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000034D9 File Offset: 0x000016D9
		[Description("节点的超链接地址，支持：[字段名]；例如：a.aspx?id=[Art_Id]")]
		[Category("树节点属性")]
		[Localizable(true)]
		[Bindable(true)]
		[DefaultValue("")]
		public string NodeLinkUrl
		{
			get
			{
				string text = (string)this.ViewState["NodeLinkUrl"];
				if (text != null)
				{
					return text;
				}
				return "#";
			}
			set
			{
				this.ViewState["NodeLinkUrl"] = value;
			}
		}

		/// <summary>
		/// 树形的HTML代码
		/// </summary>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000034EC File Offset: 0x000016EC
		public string HTML
		{
			get
			{
				return this._buildMenu();
			}
		}

		/// <summary>
		/// 向web页面呈现数据
		/// </summary>
		/// <param name="output"></param>
		// Token: 0x0600006F RID: 111 RVA: 0x000034F4 File Offset: 0x000016F4
		protected override void RenderContents(HtmlTextWriter output)
		{
			try
			{
				string text = this._buildMenu();
				text += this.javascript();
				output.Write((text == "" || text == string.Empty) ? "" : text);
			}
			catch
			{
				output.Write("树形菜单");
			}
		}

		/// <summary>
		/// 当控件初始化的时候
		/// </summary>
		/// <param name="e"></param>
		// Token: 0x06000070 RID: 112 RVA: 0x00003560 File Offset: 0x00001760
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		/// <summary>
		/// 绑定数据源
		/// </summary>
		// Token: 0x06000071 RID: 113 RVA: 0x00003569 File Offset: 0x00001769
		public override void DataBind()
		{
			this.ico = new Ico(this.SourcePath, Convert.ToInt32(this.NodeHeight));
		}

		/// <summary>
		/// 生成菜单项，此处生成根节点与所有子级
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000072 RID: 114 RVA: 0x00003588 File Offset: 0x00001788
		private string _buildMenu()
		{
			this.general.IdKeyName = this.IdKeyName;
			this.general.ParentIdKeyName = this.ParentIdKeyName;
			this.general.TaxKeyName = this.TaxKeyName;
			DataTable dataTable = null;
			if (this.DataSource is DataTable)
			{
				dataTable = (this.DataSource as DataTable);
			}
			if (this.DataSource is IList && !(this.DataSource is Array))
			{
				IList list = this.DataSource as IList;
				if (list != null && list.Count < 0)
				{
					object[] array = new object[list.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = list[i];
					}
					this.DataSource = array;
				}
			}
			if (this.DataSource is Array)
			{
				dataTable = ObjectArrayToDataTable.To((Array)this.DataSource);
			}
			DataRow dataRow = null;
			if (dataTable != null)
			{
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					DataRow dataRow2 = dataTable.Rows[j];
					dataRow2[this.TaxKeyName] = j;
				}
				foreach (object obj in dataTable.Rows)
				{
					DataRow dataRow3 = (DataRow)obj;
					if (dataRow3[this.IdKeyName].ToString() == this.Root.ToString())
					{
						dataRow = dataRow3;
						break;
					}
				}
			}
			string str = "<div type=\"MenuTree\">";
			str += "<div panelId=\"-1\">";
			str += "<div type=\"root\" style=\"width:auto;float:left;\">";
			str += this.ico.Root;
			str += "</div>";
			string str2 = string.Concat(new string[]
			{
				"line-height: ",
				this.NodeHeight,
				"px;height: ",
				this.NodeHeight,
				"px;float: none;DISPLAY: table;width:auto;cursor:pointer;"
			});
			str = str + "<div type=\"rootText\" style=\"" + str2 + "\" nodeType=\"root\">";
			str += this._BuildNode(dataRow);
			str += "</div>";
			str += "</div>";
			str += this._ConsLevel3Menu(dataRow, this.Root, dataTable);
			return str + "</div>";
		}

		/// <summary>
		/// 生成菜单的树形，也就是无限级
		/// </summary>
		/// <param name="single">当前菜单节点对象</param>
		/// <param name="topid"></param>
		/// <returns></returns>
		// Token: 0x06000073 RID: 115 RVA: 0x00003810 File Offset: 0x00001A10
		private string _ConsLevel3Menu(DataRow single, int topid, DataTable source)
		{
			string text = "";
			if (source == null)
			{
				return text;
			}
			MenuNode menuNode = new MenuNode(single, source, this.general);
			if (!menuNode.IsChilds)
			{
				return "";
			}
			string str = (single == null) ? this.Root.ToString() : single[this.IdKeyName].ToString();
			text = text + "<div type=\"nodePanel\" panelId=\"" + str + "\">";
			foreach (DataRow dataRow in menuNode.Childs)
			{
				MenuNode menuNode2 = new MenuNode(dataRow, source, this.general);
				text = text + "<div type=\"nodeline\" nodeId=\"" + dataRow[this.IdKeyName].ToString() + "\"  style=\"WIDTH: 100%; DISPLAY: table; FLOAT: none\">";
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<div type=\"nodeIco\" IsChilds=\"",
					menuNode2.IsChilds,
					"\" style=\"width:auto;float:left;\">"
				});
				text += this.nodeLine(dataRow, topid.ToString(), source);
				text += this.nodeIco(dataRow, source);
				text += "</div>";
				string text2 = string.Concat(new string[]
				{
					"line-height: ",
					this.NodeHeight,
					"px;height: ",
					this.NodeHeight,
					"px;float: left;DISPLAY: table;width:auto;cursor:pointer;"
				});
				int num = Convert.ToInt32(dataRow[this.TaxKeyName].ToString()) * 10;
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"<div type=\"text\" style=\"",
					text2,
					"\" nodeId=\"",
					dataRow[this.IdKeyName].ToString(),
					"\" nodeType=\"item\" IsChilds=\"",
					menuNode2.IsChilds,
					"\" patId=\"",
					dataRow[this.ParentIdKeyName].ToString(),
					"\" tax=\"",
					num,
					"\" >"
				});
				text += this._BuildNode(dataRow);
				text += "</div>";
				text += "</div>";
				text += this._ConsLevel3Menu(dataRow, topid, source);
			}
			return text + "</DIV>";
		}

		/// <summary>
		/// 生成节点文件项
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		// Token: 0x06000074 RID: 116 RVA: 0x00003A7C File Offset: 0x00001C7C
		private string _BuildNode(DataRow m)
		{
			string text = (m == null) ? this.Title : m[this.DataTextField].ToString();
			string text2 = "font-size:" + (double)Convert.ToInt32(this.NodeHeight) * 0.7 + "px";
			string text3 = string.Concat(new string[]
			{
				"<span style=\"",
				text2,
				"\">",
				text,
				"</span>"
			});
			if (m == null)
			{
				return text3;
			}
			string str = (!(this.IsUseKeyName != string.Empty) || Convert.ToBoolean(m[this.IsUseKeyName].ToString())) ? "" : "<span style=\"color:red\" title=\"该栏目被禁用\">[禁]</span>";
			string str2 = "";
			if (this.IsShowKeyName != string.Empty)
			{
				str2 = ((!(this.IsShowKeyName != string.Empty) || Convert.ToBoolean(m[this.IsShowKeyName].ToString())) ? "" : "<span style=\"color:red\" title=\"该栏目在使用中将不显示\">[隐]</span>");
			}
			string text4 = (this.TypeKeyName != string.Empty) ? m[this.TypeKeyName].ToString() : "node";
			if (text4 == "link")
			{
				string text5 = this.NodeLinkUrl.Trim();
				string pattern = "\\[(\\w[^\\]]*)\\]";
				Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
				MatchCollection matchCollection = regex.Matches(text5);
				for (int i = 0; i < matchCollection.Count; i++)
				{
					Match match = matchCollection[i];
					string value = match.Groups[1].Value;
					text5 = text5.Replace("[" + value + "]", m[value].ToString());
				}
				string str3 = string.Concat(new string[]
				{
					"<a href=\"",
					text5,
					"\"  target=\"",
					this.Target,
					"\" style=\"",
					text2,
					"\">",
					text,
					"</a>"
				});
				return str3 + str + str2;
			}
			if (text4 == "node")
			{
				return text3 + str + str2;
			}
			string str4 = "<span class=\"type\">&nbsp;[" + text4 + "]</span>";
			return text3 + str + str2 + str4;
		}

		/// <summary>
		/// 生成菜单项前的链接线
		/// </summary>
		/// <param name="m">当前节点</param>
		/// <param name="topid">当前节点上溯到最顶节点的id</param>
		/// <returns></returns>
		// Token: 0x06000075 RID: 117 RVA: 0x00003D00 File Offset: 0x00001F00
		private string nodeLine(DataRow m, string topid, DataTable source)
		{
			string text = "";
			if (m == null)
			{
				return "";
			}
			MenuNode menuNode = new MenuNode(m, source, this.general);
			if (menuNode.Parent == null)
			{
				return "";
			}
			MenuNode menuNode2 = new MenuNode(menuNode.Parent, source, this.general);
			while (menuNode2.Item[this.IdKeyName].ToString() != topid)
			{
				if (menuNode2.IsLast)
				{
					text = this.ico.Empty + text;
				}
				else
				{
					text = this.ico.Line + text;
				}
				if (menuNode2.Parent == null)
				{
					break;
				}
				menuNode2 = new MenuNode(menuNode2.Parent, source, this.general);
			}
			return text;
		}

		/// <summary>
		/// 生成当前节点前的图标，包括连接线
		/// </summary>
		/// <param name="m"></param>
		/// <returns></returns>
		// Token: 0x06000076 RID: 118 RVA: 0x00003DB8 File Offset: 0x00001FB8
		private string nodeIco(DataRow m, DataTable source)
		{
			string text = " ";
			MenuNode menuNode = new MenuNode(m, source, this.general);
			if (m == null && menuNode.Parent == null)
			{
				return text + this.ico.Root;
			}
			if (menuNode.IsChilds && menuNode.IsLast)
			{
				text = text + this.ico.MinusBottom + this.ico.FolderOpen;
			}
			if (menuNode.IsChilds && !menuNode.IsLast)
			{
				text = text + this.ico.Minus + this.ico.FolderOpen;
			}
			if (!menuNode.IsChilds && menuNode.IsLast)
			{
				text = text + this.ico.JoinBottom + this.ico.Page;
			}
			if (!menuNode.IsChilds && !menuNode.IsLast)
			{
				text = text + this.ico.Join + this.ico.Page;
			}
			return text.ToLower();
		}

		/// <summary>
		/// 生成执行脚本
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000077 RID: 119 RVA: 0x00003EB4 File Offset: 0x000020B4
		private string javascript()
		{
			string str = "$(TreeEvent);function TreeEvent(){var panel=$('div[type=\\'MenuTree\\']');panel.find('div[IsChilds=\\'True\\'][type=\\'nodeIco\\']').click(function(){var last=$(this).find('img:last');var pre=last.prev();tranImg(pre,'plus.gif','minus.gif');tranImg(pre,'minusbottom.gif','plusbottom.gif');tranImg(last,'folderopen.gif','folder.gif');$(this).parent().next().slideToggle();var state=$(this).attr('state');$(this).attr('state', state=='true' ? 'false' : 'true');});panel.find('div[IsChilds=\\'True\\'][type=\\'text\\']').click(function(){var a=$(this).parent().find('a');if(a.size()>0)return;var last=$(this).parent().find('div:first').find('img:last');var pre=last.prev();tranImg(pre,'plus.gif','minus.gif');tranImg(pre,'minusbottom.gif','plusbottom.gif');tranImg(last,'folderopen.gif','folder.gif');$(this).parent().next().slideToggle();var state=$(this).attr('state');$(this).attr('state', state=='true' ? 'false' : 'true');});}function tranImg(el,img1,img2){if(el.attr('src').indexOf(img1)>0){el.attr('src',el.attr('src').replace(img1,img2));}else{if(el.attr('src').indexOf(img2)>0){el.attr('src',el.attr('src').replace(img2,img1));}}}";
			return "\n<script type=\"text/javascript\">\n" + str + "\n</script>\n";
		}

		// Token: 0x04000009 RID: 9
		private object _datasource;

		// Token: 0x0400000A RID: 10
		private Ico ico;

		// Token: 0x0400000B RID: 11
		private GeneralProperty general = new GeneralProperty();
	}
}
