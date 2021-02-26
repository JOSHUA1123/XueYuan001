using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Common.Param.Method;

namespace Common.Templates
{
	/// <summary>
	/// 模板库对象
	/// </summary>
	// Token: 0x02000028 RID: 40
	public class TemplateBank
	{
		/// <summary>
		/// 模板的名称
		/// </summary>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00009640 File Offset: 0x00007840
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00009648 File Offset: 0x00007848
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		/// <summary>
		/// 模板的标识，即模板所在文件夹的名称
		/// </summary>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00009651 File Offset: 0x00007851
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00009659 File Offset: 0x00007859
		public string Tag
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		/// <summary>
		/// 模板的作者
		/// </summary>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00009662 File Offset: 0x00007862
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x0000966A File Offset: 0x0000786A
		public string Author
		{
			get
			{
				return this._author;
			}
			set
			{
				this._author = value;
			}
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00009673 File Offset: 0x00007873
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000967B File Offset: 0x0000787B
		public DateTime CrtTime
		{
			get
			{
				return this._crtTime;
			}
			set
			{
				this._crtTime = value;
			}
		}

		/// <summary>
		/// 作者QQ
		/// </summary>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00009684 File Offset: 0x00007884
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000968C File Offset: 0x0000788C
		public string QQ
		{
			get
			{
				return this._qq;
			}
			set
			{
				this._qq = value;
			}
		}

		/// <summary>
		/// 作者QQ
		/// </summary>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00009695 File Offset: 0x00007895
		// (set) Token: 0x060000FE RID: 254 RVA: 0x0000969D File Offset: 0x0000789D
		public string Phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				this._phone = value;
			}
		}

		/// <summary>
		/// 模板说明
		/// </summary>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000096A6 File Offset: 0x000078A6
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000096AE File Offset: 0x000078AE
		public string Intro
		{
			get
			{
				return this._intro;
			}
			set
			{
				this._intro = value;
			}
		}

		/// <summary>
		/// 模板的文件总大小,换算成mb或kb为单位
		/// </summary>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000096B7 File Offset: 0x000078B7
		public string Size
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>
		/// 模板的文件总大小,单位字节
		/// </summary>
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000102 RID: 258 RVA: 0x000096BF File Offset: 0x000078BF
		public ulong Length
		{
			get
			{
				return this._length;
			}
		}

		/// <summary>
		/// 模板的Logo,这里是其图片路径
		/// </summary>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000096C7 File Offset: 0x000078C7
		public string Logo
		{
			get
			{
				return this.Path.Virtual + "logo.jpg";
			}
		}

		/// <summary>
		/// 模板的文件夹名称
		/// </summary>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000096DE File Offset: 0x000078DE
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000096E6 File Offset: 0x000078E6
		public _Path Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		/// <summary>
		/// 是否是当前使用模板
		/// </summary>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000096EF File Offset: 0x000078EF
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000096F7 File Offset: 0x000078F7
		public bool IsCurrent
		{
			get
			{
				return this._isCurrent;
			}
			set
			{
				this._isCurrent = value;
			}
		}

		/// <summary>
		/// 模板库的配置项
		/// </summary>
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00009700 File Offset: 0x00007900
		public TemplateConfingItem Config
		{
			get
			{
				return this._config;
			}
		}

		/// <summary>
		/// 构建模板库
		/// </summary>
		/// <param name="di">模板库所在的路径</param>
		/// <param name="config">当前模板库的配置项</param>
		// Token: 0x06000109 RID: 265 RVA: 0x00009708 File Offset: 0x00007908
		public TemplateBank(DirectoryInfo di, TemplateConfingItem config)
		{
			this._config = config;
			this._init(di);
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="di"></param>
		// Token: 0x0600010A RID: 266 RVA: 0x00010808 File Offset: 0x0000EA08
		private void _init(DirectoryInfo di)
		{
			string text = di.FullName + "\\" + TemplateBank._xmlName;
			if (!File.Exists(text))
			{
				this._creatXML(text);
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Template");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("Info");
			foreach (object obj in xmlNode2.ChildNodes)
			{
				XmlNode xmlNode3 = (XmlNode)obj;
				XmlElement xmlElement = (XmlElement)xmlNode3;
				if (xmlElement.Name == "Name")
				{
					this._name = (string.IsNullOrWhiteSpace(xmlElement.InnerText) ? di.Name : xmlElement.InnerText);
				}
				if (xmlElement.Name == "Author")
				{
					this._author = xmlElement.InnerText;
				}
				if (xmlElement.Name == "CrtTime")
				{
					try
					{
						this._crtTime = (string.IsNullOrWhiteSpace(xmlElement.InnerText) ? DateTime.Now : Convert.ToDateTime(xmlElement.InnerText));
					}
					catch
					{
						this._crtTime = DateTime.Now;
					}
				}
				if (xmlElement.Name == "QQ")
				{
					this._qq = xmlElement.InnerText;
				}
				if (xmlElement.Name == "Phone")
				{
					this._phone = xmlElement.InnerText;
				}
				if (xmlElement.Name == "Intro")
				{
					this._intro = xmlElement.InnerText;
				}
			}
			this._length = Request.Path(di.FullName).Length;
			this._size = Request.Path("").GetSize(this._length);
			this._getPages(di, null);
		}

		/// <summary>
		/// 获取模板库下的文件列表，仅文件名，不包括后缀名
		/// </summary>
		/// <param name="root">模板根路径</param>
		/// <param name="di">当前路径</param>
		// Token: 0x0600010B RID: 267 RVA: 0x00010A18 File Offset: 0x0000EC18
		private void _getPages(DirectoryInfo root, DirectoryInfo di)
		{
			if (di == null)
			{
				di = root;
			}
			foreach (FileInfo fileInfo in di.GetFiles("*.htm"))
			{
				if (fileInfo.Name.IndexOf(".") >= 0)
				{
					string str = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf("."));
					string text = di.FullName.Substring(root.FullName.Length);
					if (text.StartsWith("\\"))
					{
						text = text.Substring(1);
					}
					this._pages.Add((string.IsNullOrWhiteSpace(text) ? "" : (text + "\\")) + str);
				}
			}
			foreach (DirectoryInfo di2 in di.GetDirectories())
			{
				this._getPages(root, di2);
			}
		}

		/// <summary>
		/// 当配置文件不存在时，创建配置文件
		/// </summary>
		/// <param name="xmlPath"></param>
		// Token: 0x0600010C RID: 268 RVA: 0x00010B08 File Offset: 0x0000ED08
		private void _creatXML(string xmlPath)
		{
			StreamWriter streamWriter = new StreamWriter(xmlPath, true, Encoding.UTF8);
			streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			streamWriter.WriteLine("<Template>");
			streamWriter.WriteLine("<Info>");
			streamWriter.WriteLine("<Name/>");
			streamWriter.WriteLine("<Author/>");
			streamWriter.WriteLine("<CrtTime/>");
			streamWriter.WriteLine("<QQ/>");
			streamWriter.WriteLine("<Phone/>");
			streamWriter.WriteLine("<Intro/>");
			streamWriter.WriteLine("</Info>");
			streamWriter.WriteLine("</Template>");
			streamWriter.Close();
			streamWriter.Dispose();
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		public void Save()
		{
			string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
			text += "<Template>";
			text += "<Info>";
			text = text + "<Name>" + this._name + "</Name>";
			text = text + "<Author>" + this._author + "</Author>";
			text = text + "<CrtTime>" + this.CrtTime.ToString() + "</CrtTime>";
			text = text + "<QQ>" + this.QQ + "</QQ>";
			text = text + "<Phone>" + this.Phone + "</Phone>";
			text = text + "<Intro>" + this.Intro + "</Intro>";
			text += "</Info>";
			text += "</Template>";
			string path = this.Path.Physics + "\\" + TemplateBank._xmlName;
			using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
			{
				streamWriter.Write(text);
			}
		}

		/// <summary>
		/// 当前模板库的文件
		/// </summary>
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00009729 File Offset: 0x00007929
		public List<string> Pages
		{
			get
			{
				return this._pages;
			}
		}

		/// <summary>
		/// 页面是否存在于模板库
		/// </summary>
		/// <param name="pageName">页面文件，不含后缀名</param>
		/// <returns></returns>
		// Token: 0x0600010F RID: 271 RVA: 0x00010CD0 File Offset: 0x0000EED0
		public bool PageExists(string pageName)
		{
			bool result = false;
			foreach (string a in this._pages)
			{
				if (string.Equals(a, pageName, StringComparison.CurrentCultureIgnoreCase))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x04000047 RID: 71
		private static string _xmlName = "_self.xml";

		// Token: 0x04000048 RID: 72
		private string _name;

		// Token: 0x04000049 RID: 73
		private string _tag;

		// Token: 0x0400004A RID: 74
		private string _author;

		// Token: 0x0400004B RID: 75
		private DateTime _crtTime;

		// Token: 0x0400004C RID: 76
		private string _qq;

		// Token: 0x0400004D RID: 77
		private string _phone;

		// Token: 0x0400004E RID: 78
		private string _intro;

		// Token: 0x0400004F RID: 79
		private string _size;

		// Token: 0x04000050 RID: 80
		private ulong _length;

		// Token: 0x04000051 RID: 81
		private _Path _path;

		// Token: 0x04000052 RID: 82
		private bool _isCurrent;

		// Token: 0x04000053 RID: 83
		public TemplateConfingItem _config;

		// Token: 0x04000054 RID: 84
		private List<string> _pages = new List<string>();
	}
}
