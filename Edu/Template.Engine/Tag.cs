using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace VTemplate.Engine
{
	/// <summary>
	/// 标签元素
	/// </summary>
	// Token: 0x02000004 RID: 4
	public abstract class Tag : Element, IAttributesElement
	{
		/// <summary>
		/// 标签元素
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		// Token: 0x0600000A RID: 10 RVA: 0x00002118 File Offset: 0x00000318
		protected Tag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.InnerElements = new ElementCollection<Element>();
			this.Attributes = new AttributeCollection(this);
			this.Attributes.Adding += this.OnAddingAttribute;
		}

		/// <summary>
		/// 返回标签的名称.如for,foreach等等
		/// </summary>
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000B RID: 11
		public abstract string TagName { get; }

		/// <summary>
		/// 返回标签的结束标签名称.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000214F File Offset: 0x0000034F
		public virtual string EndTagName
		{
			get
			{
				return this.TagName;
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签元素.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13
		internal abstract bool IsSingleTag { get; }

		/// <summary>
		/// 标签的Id
		/// </summary>
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002157 File Offset: 0x00000357
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000215F File Offset: 0x0000035F
		public string Id { get; protected set; }

		/// <summary>
		/// 标签的名称
		/// </summary>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002168 File Offset: 0x00000368
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002170 File Offset: 0x00000370
		public string Name { get; protected set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002179 File Offset: 0x00000379
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002181 File Offset: 0x00000381
		public string Type { get; protected set; }

		/// <summary>
		/// 此标签包含的子元素集合
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000218A File Offset: 0x0000038A
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002192 File Offset: 0x00000392
		public ElementCollection<Element> InnerElements { get; private set; }

		/// <summary>
		/// 此标签的属性集合
		/// </summary>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000219B File Offset: 0x0000039B
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000021A3 File Offset: 0x000003A3
		public AttributeCollection Attributes { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发事件函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		// Token: 0x06000018 RID: 24 RVA: 0x000021AC File Offset: 0x000003AC
		private void OnAddingAttribute(object sender, AttributeCollection.AttributeAddingEventArgs e)
		{
			string text = e.Item.Name.ToLower();
			string a;
			if ((a = text) != null)
			{
				if (!(a == "id"))
				{
					if (!(a == "name"))
					{
						if (a == "type")
						{
							this.Type = e.Item.Text.Trim();
							this.Type = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(this.Type);
						}
					}
					else
					{
						this.Name = e.Item.Text.Trim();
					}
				}
				else
				{
					this.Id = e.Item.Text.Trim();
				}
			}
			this.OnAddingAttribute(text, e.Item);
		}

		/// <summary>
		/// 添加标签属性时的触发函数
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x06000019 RID: 25 RVA: 0x0000226E File Offset: 0x0000046E
		protected virtual void OnAddingAttribute(string name, Attribute item)
		{
		}

		/// <summary>
		/// 添加子元素
		/// </summary>
		/// <param name="element"></param>
		// Token: 0x0600001A RID: 26 RVA: 0x00002270 File Offset: 0x00000470
		internal virtual void AppendChild(Element element)
		{
			element.Parent = this;
			this.InnerElements.Add(element);
		}

		/// <summary>
		/// 根据Id获取某个子元素标签
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		// Token: 0x0600001B RID: 27 RVA: 0x00002288 File Offset: 0x00000488
		public virtual Tag GetChildTagById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id");
			}
			List<Tag> list = new List<Tag>();
			foreach (Element element in this.InnerElements)
			{
				if (element is Tag)
				{
					Tag tag = (Tag)element;
					list.Add(tag);
					if (id.Equals(tag.Id, StringComparison.InvariantCultureIgnoreCase))
					{
						return tag;
					}
				}
			}
			foreach (Tag tag2 in list)
			{
				Tag childTagById = tag2.GetChildTagById(id);
				if (childTagById != null)
				{
					return childTagById;
				}
			}
			return null;
		}

		/// <summary>
		/// 根据name获取所有同名的子元素标签
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		// Token: 0x0600001C RID: 28 RVA: 0x00002364 File Offset: 0x00000564
		public virtual ElementCollection<Tag> GetChildTagsByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			ElementCollection<Tag> elementCollection = new ElementCollection<Tag>();
			foreach (Element element in this.InnerElements)
			{
				if (element is Tag)
				{
					Tag tag = (Tag)element;
					if (name.Equals(tag.Name, StringComparison.InvariantCultureIgnoreCase))
					{
						elementCollection.Add(tag);
					}
					elementCollection.AddRange(tag.GetChildTagsByName(name));
				}
			}
			return elementCollection;
		}

		/// <summary>
		/// 根据标签名获取所有同标签名的子元素标签
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		// Token: 0x0600001D RID: 29 RVA: 0x000023F8 File Offset: 0x000005F8
		public virtual ElementCollection<Tag> GetChildTagsByTagName(string tagName)
		{
			if (string.IsNullOrEmpty(tagName))
			{
				throw new ArgumentNullException("tagName");
			}
			ElementCollection<Tag> elementCollection = new ElementCollection<Tag>();
			foreach (Element element in this.InnerElements)
			{
				if (element is Tag)
				{
					Tag tag = (Tag)element;
					if (tagName.Equals(tag.TagName, StringComparison.InvariantCultureIgnoreCase))
					{
						elementCollection.Add(tag);
					}
					elementCollection.AddRange(tag.GetChildTagsByTagName(tagName));
				}
			}
			return elementCollection;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual ElementCollection<Tag> GetCustomTags(string tagNames)
		{
			if (string.IsNullOrEmpty(tagNames))
			{
				return null;
			}
			string[] array = tagNames.Split(new char[]
			{
				','
			});
			ElementCollection<Tag> elementCollection = new ElementCollection<Tag>();
			foreach (Element element in this.InnerElements)
			{
				if (element is Tag)
				{
					Tag tag = (Tag)element;
					bool flag = false;
					foreach (string text in array)
					{
						if (text.Equals(tag.TagName, StringComparison.InvariantCultureIgnoreCase))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						elementCollection.Add(tag);
					}
					elementCollection.AddRange(tag.GetCustomTags(tagNames));
				}
			}
			return elementCollection;
		}

		/// <summary>
		/// 开始解析标签数据
		/// </summary>
		/// <param name="ownerTemplate">宿主模板</param>
		/// <param name="container">标签的容器</param>
		/// <param name="tagStack">标签堆栈</param>
		/// <param name="text"></param>
		/// <param name="match"></param>
		/// <param name="isClosedTag">是否闭合标签</param>
		/// <returns>如果需要继续处理EndTag则返回true.否则请返回false</returns>
		// Token: 0x0600001F RID: 31 RVA: 0x0000255C File Offset: 0x0000075C
		internal virtual bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			container.AppendChild(this);
			return !isClosedTag;
		}

		/// <summary>
		/// 结束标签的解析
		/// </summary>
		/// <param name="ownerTemplate">模板宿主</param>
		/// <param name="container">元素容器</param>
		/// <param name="tagStack">标签堆栈</param>
		/// <param name="text"></param>
		/// <param name="match"></param>
		// Token: 0x06000020 RID: 32 RVA: 0x0000256C File Offset: 0x0000076C
		internal virtual void ProcessEndTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match)
		{
			int num;
			int i = num = match.Index + match.Length;
			match = null;
			while (i < text.Length)
			{
				if (ParserHelper.IsVariableTagStart(text, i))
				{
					Match match2;
					match = (match2 = ParserRegex.VarTagRegex.Match(text, i));
					if (match2.Success)
					{
						ParserHelper.CreateTextNode(ownerTemplate, container, text, num, match.Index - num);
						ParserHelper.CreateVariableTag(ownerTemplate, container, match);
						goto IL_2FF;
					}
				}
				if (ParserHelper.IsTagStart(text, i))
				{
					Match match3;
					match = (match3 = ParserRegex.TagRegex.Match(text, i));
					if (match3.Success)
					{
						ParserHelper.CreateTextNode(ownerTemplate, container, text, num, match.Index - num);
						bool flag;
						Tag tag = ParserHelper.CreateTag(ownerTemplate, match, out flag);
						if (container.IsSingleTag && tag.IsSingleTag)
						{
							while (tagStack.Count > 0)
							{
								container = tagStack.Peek();
								if (!container.IsSingleTag)
								{
									break;
								}
								tagStack.Pop();
							}
						}
						tagStack.Push(tag);
						bool flag2 = tag.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, flag);
						if (flag2)
						{
							tag.ProcessEndTag(ownerTemplate, tag, tagStack, text, ref match);
						}
						if (tagStack.Count > 0 && tagStack.Peek() == tag && flag)
						{
							tagStack.Pop();
						}
						if (tag.IsSingleTag)
						{
							break;
						}
						if (tagStack.Count > 0)
						{
							container = tagStack.Peek();
							goto IL_2FF;
						}
						goto IL_2FF;
					}
				}
				if (ParserHelper.IsCloseTagStart(text, i))
				{
					Match match4;
					match = (match4 = ParserRegex.EndTagRegex.Match(text, i));
					if (match4.Success)
					{
						ParserHelper.CreateTextNode(ownerTemplate, container, text, num, match.Index - num);
						string value = match.Groups["tagname"].Value;
						while (tagStack.Count > 0)
						{
							Tag tag2 = tagStack.Pop();
							if (value.Equals(tag2.TagName, StringComparison.InvariantCultureIgnoreCase))
							{
								break;
							}
							if (!value.Equals(tag2.EndTagName, StringComparison.InvariantCultureIgnoreCase))
							{
								throw new ParserException(string.Format("无效的结束标签,原期望的是{0}结束标签", tag2.EndTagName));
							}
						}
						break;
					}
				}
				if (ParserHelper.IsVTExpressionStart(text, i))
				{
					char c = ParserHelper.ReadChar(text, i + "vt=".Length);
					int num2 = i + "vt=".Length + 1;
					int num3 = text.IndexOf(c, i + "vt=".Length + 1);
					if (num3 == -1)
					{
						throw new ParserException(string.Format("无法找到VT表达式[{0}{1}]的结束标记[{1}]", "vt=", c));
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
				IL_2FF:
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
			if (match == null)
			{
				throw new ParserException(string.Format("{0}标签未闭合", container.TagName));
			}
		}

		/// <summary>
		/// 在呈现元素标签数据之前触发的事件
		/// </summary>
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000021 RID: 33 RVA: 0x000028D0 File Offset: 0x00000AD0
		// (remove) Token: 0x06000022 RID: 34 RVA: 0x00002908 File Offset: 0x00000B08
		public event CancelEventHandler BeforeRender;

		/// <summary>
		/// 在呈现元素标签数据之后触发的事件
		/// </summary>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000023 RID: 35 RVA: 0x00002940 File Offset: 0x00000B40
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x00002978 File Offset: 0x00000B78
		public event EventHandler AfterRender;

		/// <summary>
		/// 触发呈现元素标签数据之前的事件
		/// </summary>
		/// <param name="args"></param>
		// Token: 0x06000025 RID: 37 RVA: 0x000029B0 File Offset: 0x00000BB0
		protected virtual void OnBeforeRender(CancelEventArgs args)
		{
			CancelEventHandler beforeRender = this.BeforeRender;
			if (beforeRender != null)
			{
				beforeRender(this, args);
			}
		}

		/// <summary>
		/// 触发呈现元素标签数据之前的事件
		/// </summary>
		/// <param name="args"></param>
		// Token: 0x06000026 RID: 38 RVA: 0x000029D0 File Offset: 0x00000BD0
		protected virtual void OnAfterRender(EventArgs args)
		{
			EventHandler afterRender = this.AfterRender;
			if (afterRender != null)
			{
				afterRender(this, args);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000027 RID: 39 RVA: 0x000029F0 File Offset: 0x00000BF0
		public override void Render(TextWriter writer)
		{
			Tag currentRenderingTag = this.OwnerDocument.CurrentRenderingTag;
			this.OwnerDocument.RegisterCurrentRenderingTag(this);
			this.RenderTagData(writer);
			this.OwnerDocument.RegisterCurrentRenderingTag(currentRenderingTag);
		}

		/// <summary>
		/// 呈现本标签元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000028 RID: 40 RVA: 0x00002A28 File Offset: 0x00000C28
		protected virtual void RenderTagData(TextWriter writer)
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs();
			this.OnBeforeRender(cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				foreach (Element element in this.InnerElements)
				{
					element.Render(writer);
				}
			}
			this.OnAfterRender(EventArgs.Empty);
		}

		/// <summary>
		/// 将本标签的呈现数据保存到文件,采用宿主模板的编码
		/// </summary>
		/// <param name="fileName">文件地址</param>
		// Token: 0x06000029 RID: 41 RVA: 0x00002A98 File Offset: 0x00000C98
		public virtual void RenderTo(string fileName)
		{
			this.RenderTo(fileName, (this.OwnerDocument == null) ? Encoding.Default : this.OwnerDocument.Charset);
		}

		/// <summary>
		/// 将本标签的呈现数据保存到文件
		/// </summary>
		/// <param name="fileName">文件地址</param>
		/// <param name="charset">文件编码</param>
		// Token: 0x0600002A RID: 42 RVA: 0x00002ABC File Offset: 0x00000CBC
		public virtual void RenderTo(string fileName, Encoding charset)
		{
			using (StreamWriter streamWriter = new StreamWriter(fileName, false, charset))
			{
				this.Render(streamWriter);
			}
		}

		/// <summary>
		/// 输出标签的原字符串数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600002B RID: 43 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<vt:{0}", this.TagName);
			foreach (Attribute attribute in this.Attributes)
			{
				stringBuilder.AppendFormat(" {0}=\"{1}\"", attribute.Name, HttpUtility.HtmlEncode(attribute.Text));
			}
			stringBuilder.AppendFormat(">", new object[0]);
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 拷贝自身数据对某个新对象上
		/// </summary>
		/// <param name="tag"></param>
		// Token: 0x0600002C RID: 44 RVA: 0x00002B8C File Offset: 0x00000D8C
		protected virtual void CopyTo(Tag tag)
		{
			tag.Id = this.Id;
			tag.Name = this.Name;
			tag.Type = this.Type;
			foreach (Attribute attribute in this.Attributes)
			{
				tag.Attributes.Add(attribute.Clone(tag.OwnerTemplate));
			}
			foreach (Element element in this.InnerElements)
			{
				tag.AppendChild(element.Clone(tag.OwnerTemplate));
			}
		}
	}
}
