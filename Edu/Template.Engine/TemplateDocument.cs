using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using Common;

namespace VTemplate.Engine
{
	/// <summary>
	/// 模板文档
	/// </summary>
	// Token: 0x02000021 RID: 33
	public class TemplateDocument : Template
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x0600019A RID: 410 RVA: 0x00007C55 File Offset: 0x00005E55
		private TemplateDocument(TemplateDocumentConfig documentConfig)
		{
			this.DocumentConfig = documentConfig;
		}

		/// <summary>
		/// 采用默认的文档配置并根据TextRader数据进行解析
		/// </summary>
		/// <param name="reader"></param>
		// Token: 0x0600019B RID: 411 RVA: 0x00007C64 File Offset: 0x00005E64
		public TemplateDocument(TextReader reader) : this(reader, TemplateDocumentConfig.Default)
		{
		}

		/// <summary>
		/// 根据TextRader数据进行解析
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="documentConfig"></param>
		// Token: 0x0600019C RID: 412 RVA: 0x00007C72 File Offset: 0x00005E72
		public TemplateDocument(TextReader reader, TemplateDocumentConfig documentConfig)
		{
			this.DocumentConfig = documentConfig;
			this.ParseString(reader.ReadToEnd());
		}

		/// <summary>
		/// 采用默认的文档配置并根据文件内容进行解析
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="charset"></param>
		// Token: 0x0600019D RID: 413 RVA: 0x00007C8D File Offset: 0x00005E8D
		public TemplateDocument(string fileName, Encoding charset) : this(fileName, charset, TemplateDocumentConfig.Default)
		{
		}

		/// <summary>
		/// 根据文件内容进行解析
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="charset"></param>
		/// <param name="documentConfig"></param>
		// Token: 0x0600019E RID: 414 RVA: 0x00007C9C File Offset: 0x00005E9C
		public TemplateDocument(string fileName, Encoding charset, TemplateDocumentConfig documentConfig)
		{
			string text = System.IO.File.ReadAllText(fileName, charset);
			HttpContext httpContext = HttpContext.Current;
			base.Page = httpContext.Server.MapPath(httpContext.Request.FilePath);
			text = TemplateDocument._replacePath(text, base.Page, fileName);
			//if (License.Value.VersionLevel == 0)
			//{
			//	if (DateTime.Now > Server.InitDate.AddMonths(1))
			//	{
			//		string arg = "position:fixed !important;\tbackground-color: #fff;\tbottom: 50px;z-index:999999 !important;\twidth:100%;\tdisplay:block !important;";
			//		string text2 = "<div style=\"text-align:center;padding:10px;{0}\">当前版本为<a href=\"/license.aspx\" style=\"color:red;\">免费版</a><br/>\r\n                    <a href=\"http://www.weishakeji.net\" target=\"_blank\" title=\"微厦科技，专注在线教育系统开发！\">Powered by Weishakeji</a></div>";
			//		text2 = string.Format(text2, arg);
			//		string pattern = "</body.*[^>]>|</body>";
			//		if (Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase))
			//		{
			//			text = Regex.Replace(text, pattern, text2 + "</body>", RegexOptions.IgnoreCase);
			//		}
			//		else
			//		{
			//			text += text2;
			//		}
			//	}
			//	text = Regex.Replace(text, "<html[^>]*>", "<html license=\"false\" copyright=\"Weishakeji\">", RegexOptions.IgnoreCase);
			//}
			base.File = Path.GetFullPath(fileName);
			base.Charset = charset;
			base.AddFileDependency(base.File);
			this.DocumentConfig = documentConfig;
			this.ParseString(this, text);
		}

		/// <summary>
		/// 采用默认的文档配置并根据字符串进行解析
		/// </summary>
		/// <param name="text"></param>
		// Token: 0x0600019F RID: 415 RVA: 0x00007D93 File Offset: 0x00005F93
		public TemplateDocument(string text) : this(text, TemplateDocumentConfig.Default)
		{
		}

		/// <summary>
		/// 根据字符串进行解析
		/// </summary>
		/// <param name="text"></param>
		/// <param name="documentConfig"></param>
		// Token: 0x060001A0 RID: 416 RVA: 0x00007DA1 File Offset: 0x00005FA1
		public TemplateDocument(string text, TemplateDocumentConfig documentConfig)
		{
			this.DocumentConfig = documentConfig;
			this.ParseString(text);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="documentElement"></param>
		/// <param name="text"></param>
		/// <param name="documentConfig"></param>
		// Token: 0x060001A1 RID: 417 RVA: 0x00007DB7 File Offset: 0x00005FB7
		internal TemplateDocument(Template documentElement, string text, TemplateDocumentConfig documentConfig) : this(documentElement, documentElement, text, documentConfig)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="documentElement"></param>
		/// <param name="container"></param>
		/// <param name="text"></param>
		/// <param name="documentConfig"></param>
		// Token: 0x060001A2 RID: 418 RVA: 0x00007DC4 File Offset: 0x00005FC4
		internal TemplateDocument(Template documentElement, Tag container, string text, TemplateDocumentConfig documentConfig)
		{
			HttpContext httpContext = HttpContext.Current;
			base.Page = httpContext.Server.MapPath(httpContext.Request.FilePath);
			text = TemplateDocument._replacePath(text, base.Page, documentElement.File);
			this.DocumentConfig = documentConfig;
			this.AppendChild(documentElement);
			base.ChildTemplates.Add(documentElement);
			this.ParseString(documentElement, container, text);
		}

		/// <summary>
		/// 根文档模板
		/// </summary>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007E31 File Offset: 0x00006031
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00007E39 File Offset: 0x00006039
		public Template DocumentElement
		{
			get
			{
				return base.TagContainer;
			}
			set
			{
				base.TagContainer = value;
			}
		}

		/// <summary>
		/// 返回此模板块的宿主模板文档
		/// </summary>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007E42 File Offset: 0x00006042
		public override TemplateDocument OwnerDocument
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// 模板文档的配置参数
		/// </summary>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007E45 File Offset: 0x00006045
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007E4D File Offset: 0x0000604D
		public TemplateDocumentConfig DocumentConfig { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007E56 File Offset: 0x00006056
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00007E5E File Offset: 0x0000605E
		public object Orgin { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007E67 File Offset: 0x00006067
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00007E6F File Offset: 0x0000606F
		public int OrginID { get; set; }

		/// <summary>
		/// 返回当前正在呈现数据的标签
		/// </summary>
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007E78 File Offset: 0x00006078
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00007E80 File Offset: 0x00006080
		public Tag CurrentRenderingTag
		{
			get
			{
				return this._CurrentRenderingTag;
			}
			private set
			{
				this._CurrentRenderingTag = value;
			}
		}

		/// <summary>
		/// 当前正在呈现数据的文档
		/// </summary>
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007E89 File Offset: 0x00006089
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00007E90 File Offset: 0x00006090
		public static TemplateDocument CurrentRenderingDocument
		{
			get
			{
				return TemplateDocument._CurrentRenderingDocument;
			}
			private set
			{
				TemplateDocument._CurrentRenderingDocument = value;
			}
		}

		/// <summary>
		/// 获取此模板文档的呈现数据
		/// </summary>
		// Token: 0x060001B0 RID: 432 RVA: 0x00007E98 File Offset: 0x00006098
		public string GetRenderText()
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				this.Render(stringWriter);
				result = stringWriter.ToString();
			}
			return result;
		}

		/// <summary>
		/// 注册当前呈现的标签
		/// </summary>
		/// <param name="tag"></param>
		// Token: 0x060001B1 RID: 433 RVA: 0x00007ED8 File Offset: 0x000060D8
		internal void RegisterCurrentRenderingTag(Tag tag)
		{
			TemplateDocument.CurrentRenderingDocument = ((tag == null) ? null : tag.OwnerDocument);
			this.CurrentRenderingTag = tag;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007EF2 File Offset: 0x000060F2
		public static string _replacePath(string text, string page, string tmFile)
		{
			return TemplateDocument._replacePath(text, page, tmFile, "body|link|script|img");
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007F04 File Offset: 0x00006104
		public static string _replacePath(string text, string page, string tmFile, string tags)
		{
			string str = tmFile.Substring(0, tmFile.LastIndexOf("\\") + 1);
			HttpContext.Current.Server.MapPath("/");
			RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace;
			string pattern = "<(" + tags + ")[^>]+>";
			foreach (object obj in new Regex(pattern, options).Matches(text))
			{
				Match match = (Match)obj;
				string text2 = match.Groups[1].Value.Trim();
				string text3 = match.Groups[0].Value.Trim();
				string pattern2 = "(?<=\\s+)(?<key>path|href|src|error|default|action|background[^=\"']*)=([\"'])?(?<value>[^'\">]*)\\1?";
				foreach (object obj2 in new Regex(pattern2, options).Matches(text3))
				{
					Match match2 = (Match)obj2;
					string text4 = match2.Groups["key"].Value.Trim();
					string text5 = match2.Groups["value"].Value.Trim();
					if (!new Regex("[a-zA-z]+://[^\\s]*", RegexOptions.Singleline).IsMatch(text5) && !new Regex("^[{\\\\\\/\\#]").IsMatch(text5) && !new Regex("^data:image").IsMatch(text5) && !new Regex("^javascript:").IsMatch(text5) && ((!(text4.ToLower() == "error") && !(text4.ToLower() == "default")) || !(text2.ToLower() != "img")))
					{
						text5 = TemplateDocument.RelativePath(page, str + text5);
						text5 = match2.Groups[2].Value + "=\"" + text5 + "\"";
						text5 = Regex.Replace(text5, "//", "/");
						text3 = text3.Replace(match2.Value, text5);
					}
				}
				text = text.Replace(match.Groups[0].Value.Trim(), text3);
			}
			return text;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008198 File Offset: 0x00006398
		public static string RelativePath(string baseFile, string targetFile)
		{
			baseFile = baseFile.Replace("\\", "/").ToLower();
			targetFile = targetFile.Replace("\\", "/").ToLower();
			while (baseFile.IndexOf("/") > -1 && targetFile.IndexOf("/") > -1)
			{
				string a = baseFile.Substring(0, baseFile.IndexOf("/"));
				string b = targetFile.Substring(0, targetFile.IndexOf("/"));
				if (a != b)
				{
					break;
				}
				baseFile = baseFile.Substring(baseFile.IndexOf("/") + 1);
				targetFile = targetFile.Substring(targetFile.IndexOf("/") + 1);
			}
			string str = "";
			while (baseFile.IndexOf("/") > -1)
			{
				baseFile = baseFile.Substring(baseFile.IndexOf("/") + 1);
				str += "../";
			}
			string text = str + targetFile;
			while (text.IndexOf("//") > -1)
			{
				text = text.Replace("//", "/");
			}
			return TemplateDocument.RelativePath_ClearnPoint(text);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000082B4 File Offset: 0x000064B4
		public static string RelativePath_ClearnPoint(string path)
		{
			if (path.IndexOf("/") < 0)
			{
				return path;
			}
			if (path.IndexOf("../") < 0)
			{
				return path;
			}
			Stack stack = new Stack();
			string[] array = path.Split(new char[]
			{
				'/'
			});
			string str = "";
			int num = 0;
			while (array[num] == "..")
			{
				str = str + array[num++] + "/";
			}
			for (int i = num; i < array.Length; i++)
			{
				if (array[i] == ".." && i > 0 && array[i - 1] != "..")
				{
					stack.Pop();
				}
				else
				{
					stack.Push(array[i]);
				}
			}
			string text = "";
			foreach (object obj in stack)
			{
				string str2 = (string)obj;
				text = str2 + "/" + text;
			}
			if (text.Length > 0)
			{
				string a = text.Substring(text.Length - 1);
				if (a == "/")
				{
					text = text.Substring(0, text.Length - 1);
				}
			}
			if (text.IndexOf("../") > -1)
			{
				text = TemplateDocument.RelativePath_ClearnPoint(text);
			}
			return str + text;
		}

		/// <summary>
		/// 解析字符串
		/// </summary>
		/// <param name="text"></param>
		// Token: 0x060001B6 RID: 438 RVA: 0x00008434 File Offset: 0x00006634
		private void ParseString(string text)
		{
			this.ParseString(this, text);
		}

		/// <summary>
		/// 解析字符串
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="text"></param>
		// Token: 0x060001B7 RID: 439 RVA: 0x0000843E File Offset: 0x0000663E
		private void ParseString(Template ownerTemplate, string text)
		{
			this.ParseString(ownerTemplate, ownerTemplate, text);
		}

		/// <summary>
		/// 解析字符串
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="text">模板文本数据</param>
		// Token: 0x060001B8 RID: 440 RVA: 0x0000844C File Offset: 0x0000664C
		private void ParseString(Template ownerTemplate, Tag container, string text)
		{
            if (Common.Template.IsTrim)
			{
				text = Regex.Replace(text, "(<!--).*(-->)", "", RegexOptions.Multiline);
				text = Regex.Replace(text, "//([^\"\\r\\n]*)[\\r\\n]", "", RegexOptions.Multiline);
				text = Regex.Replace(text, "(\\/\\*(\\n|.)*?\\*\\/)", "", RegexOptions.Multiline);
				text = Regex.Replace(text, "\\s{2,}", " ", RegexOptions.Singleline);
				text = Regex.Replace(text, "(?<=\\>)\\s+(?=\\<)|(?<=\\>)\\s+(?=\\S)|(?<=\\S)\\s+(?=\\<)", "", RegexOptions.Singleline);
				text = Regex.Replace(text, "(<!--).*(-->)", "", RegexOptions.Singleline);
			}
			this.DocumentElement = ownerTemplate;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			int num = 0;
			int i = 0;
			Match match = null;
			Stack<Tag> stack = new Stack<Tag>();
			stack.Push(container);
			try
			{
				while (i < text.Length)
				{
					if (ParserHelper.IsVariableTagStart(text, i) && (match = ParserRegex.VarTagRegex.Match(text, i)).Success)
					{
						ParserHelper.CreateTextNode(ownerTemplate, container, text, num, match.Index - num);
						ParserHelper.CreateVariableTag(ownerTemplate, container, match);
					}
					else if (ParserHelper.IsTagStart(text, i) && (match = ParserRegex.TagRegex.Match(text, i)).Success)
					{
						ParserHelper.CreateTextNode(ownerTemplate, container, text, num, match.Index - num);
						bool flag;
						Tag tag = ParserHelper.CreateTag(ownerTemplate, match, out flag);
						stack.Push(tag);
						bool flag2 = tag.ProcessBeginTag(ownerTemplate, container, stack, text, ref match, flag);
						if (flag2)
						{
							tag.ProcessEndTag(ownerTemplate, tag, stack, text, ref match);
						}
						if (stack.Count > 0 && stack.Peek() == tag && flag)
						{
							stack.Pop();
						}
						if (stack.Count > 0)
						{
							container = stack.Peek();
						}
					}
					else
					{
						if (ParserHelper.IsCloseTagStart(text, i) && (match = ParserRegex.EndTagRegex.Match(text, i)).Success)
						{
							string value = match.Groups["tagname"].Value;
							throw new ParserException("无效的结束标签");
						}
						if (ParserHelper.IsVTExpressionStart(text, i))
						{
							char value2 = ParserHelper.ReadChar(text, i + "vt=".Length);
							int num2 = i + "vt=".Length + 1;
							int num3 = text.IndexOf(value2, i + "vt=".Length + 1);
							if (num3 == -1)
							{
								throw new ParserException(string.Format("无法找到VT表达式\"{0}\"的结束标记", "vt="));
							}
							string text2 = text.Substring(num2, num3 - num2);
							if (text2.Length > 0)
							{
								ParserHelper.CreateTextNode(ownerTemplate, container, text, num, i - num);
								new TemplateDocument(ownerTemplate, container, text2, ownerTemplate.OwnerDocument.DocumentConfig);
							}
							i = num3 + 1;
							num = i;
							continue;
						}
						else if (ParserHelper.IsCommentTagStart(text, i))
						{
							ParserHelper.CreateTextNode(ownerTemplate, container, text, num, i - num);
							i = text.IndexOf("]-->", i + "<!--vt[".Length);
							if (i == -1)
							{
								throw new ParserException("无法找到注释的结束标记");
							}
							i += "]-->".Length;
							num = i;
							continue;
						}
					}
					if (match != null && match.Success)
					{
						i = (num = match.Index + match.Length);
						match = null;
					}
					else
					{
						i++;
					}
				}
				ParserHelper.CreateTextNode(ownerTemplate, container, text, num, text.Length - num);
				if (stack.Count > 1)
				{
					throw new ParserException(string.Format("{0}标签未闭合", stack.Pop()));
				}
			}
			catch (ParserException ex)
			{
				if (ex.HaveLineAndColumnNumber || match == null || !match.Success)
				{
					throw;
				}
				string text3 = string.Empty;
				Tag tag2 = container;
				while (string.IsNullOrEmpty(text3) && tag2 != null)
				{
					if (tag2 is Template)
					{
						text3 = ((Template)tag2).File;
					}
					else if (tag2 is IncludeTag)
					{
						text3 = ((IncludeTag)tag2).File;
					}
					tag2 = tag2.Parent;
				}
				if (string.IsNullOrEmpty(text3))
				{
					throw new ParserException(Utility.GetLineAndColumnNumber(text, match.Index), match.ToString(), ex.Message);
				}
				throw new ParserException(text3, Utility.GetLineAndColumnNumber(text, match.Index), match.ToString(), ex.Message);
			}
			finally
			{
				stack.Clear();
				stack = null;
			}
		}

		/// <summary>
		/// 从文件缓存中构建模板文档对象
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="charset"></param>
		/// <returns></returns>
		// Token: 0x060001B9 RID: 441 RVA: 0x00008878 File Offset: 0x00006A78
		public static TemplateDocument FromFileCache(string fileName, Encoding charset)
		{
			return TemplateDocument.FromFileCache(fileName, charset, TemplateDocumentConfig.Default);
		}

		/// <summary>
		/// 从文件缓存中构建模板文档对象
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="charset"></param>
		/// <param name="documentConfig"></param>
		/// <returns></returns>
		// Token: 0x060001BA RID: 442 RVA: 0x00008888 File Offset: 0x00006A88
		public static TemplateDocument FromFileCache(string fileName, Encoding charset, TemplateDocumentConfig documentConfig)
		{
			Cache cache = HttpRuntime.Cache;
			if (documentConfig == null)
			{
				documentConfig = TemplateDocumentConfig.Default;
			}
			if (cache == null)
			{
				return new TemplateDocument(fileName, charset, documentConfig);
			}
			fileName = Path.GetFullPath(fileName);
			string key = string.Format("TEMPLATE_DOCUMENT_CACHE_ITEM_{0}_{1}_{2}", documentConfig.TagOpenMode, documentConfig.CompressText, fileName);
			TemplateDocument templateDocument = cache.Get(key) as TemplateDocument;
			if (templateDocument == null)
			{
				templateDocument = new TemplateDocument(fileName, charset, documentConfig);
				cache.Insert(key, templateDocument, new CacheDependency(templateDocument.FileDependencies));
			}
			return templateDocument.Clone();
		}

		/// <summary>
		/// 克隆模板文档对象
		/// </summary>
		/// <returns></returns>
		// Token: 0x060001BB RID: 443 RVA: 0x0000890D File Offset: 0x00006B0D
		private TemplateDocument Clone()
		{
			return (TemplateDocument)this.Clone(null);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060001BC RID: 444 RVA: 0x0000891C File Offset: 0x00006B1C
		internal override Element Clone(Template ownerTemplate)
		{
			TemplateDocument templateDocument = new TemplateDocument(this.DocumentConfig);
			foreach (Variable variable in base.Variables)
			{
				templateDocument.Variables.Add(variable.Clone(templateDocument));
			}
			templateDocument.Id = base.Id;
			templateDocument.Name = base.Name;
			foreach (Attribute attribute in base.Attributes)
			{
				templateDocument.Attributes.Add(attribute.Clone(templateDocument));
			}
			templateDocument.Charset = base.Charset;
			templateDocument.File = base.File;
			templateDocument.fileDependencies = this.fileDependencies;
			templateDocument.Visible = base.Visible;
			foreach (Element element in base.InnerElements)
			{
				Element element2 = element.Clone(templateDocument);
				templateDocument.AppendChild(element2);
				if (element is Template && this.DocumentElement == element)
				{
					templateDocument.DocumentElement = (Template)element2;
				}
			}
			if (templateDocument.DocumentElement == null)
			{
				templateDocument.DocumentElement = templateDocument;
			}
			return templateDocument;
		}

		/// <summary>
		///
		/// </summary>
		// Token: 0x04000051 RID: 81
		private Tag _CurrentRenderingTag;

		/// <summary>
		///
		/// </summary>
		// Token: 0x04000052 RID: 82
		[ThreadStatic]
		private static TemplateDocument _CurrentRenderingDocument;
	}
}
