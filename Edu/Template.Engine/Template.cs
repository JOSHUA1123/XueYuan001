using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 模板块标签.如: &lt;vt:template id="member"&gt;.......&lt;/vt:template&gt; 或自闭合的模板:&lt;vt:template id="member" file="member.html" /&gt;
	/// </summary>
	// Token: 0x0200001F RID: 31
	public class Template : Tag
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x0600015E RID: 350 RVA: 0x000073EE File Offset: 0x000055EE
		internal Template() : this(null)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x0600015F RID: 351 RVA: 0x000073F8 File Offset: 0x000055F8
		internal Template(Template ownerTemplate) : base(ownerTemplate)
		{
			this.Visible = true;
			this.Charset = ((ownerTemplate == null) ? Encoding.Default : ownerTemplate.Charset);
			this.Variables = new VariableCollection();
			this.ChildTemplates = new ElementCollection<Template>();
			this.fileDependencies = new List<string>();
			this.UserDefinedFunctions = new UserDefinedFunctionCollection();
			this.TagContainer = this;
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000745C File Offset: 0x0000565C
		public override string TagName
		{
			get
			{
				return "template";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007463 File Offset: 0x00005663
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 模板的关联文件
		/// </summary>
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007466 File Offset: 0x00005666
		// (set) Token: 0x06000163 RID: 355 RVA: 0x0000746E File Offset: 0x0000566E
		public string File { get; internal set; }

		/// <summary>
		/// 模板的宿主文件，即用户打开的页面
		/// </summary>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007477 File Offset: 0x00005677
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000747F File Offset: 0x0000567F
		public string Page { get; set; }

		/// <summary>
		/// 模板数据采用的编码
		/// </summary>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007488 File Offset: 0x00005688
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007490 File Offset: 0x00005690
		public Encoding Charset { get; internal set; }

		/// <summary>
		/// 设置或返回此模板是否可见
		/// </summary>
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007499 File Offset: 0x00005699
		// (set) Token: 0x06000169 RID: 361 RVA: 0x000074A1 File Offset: 0x000056A1
		public bool Visible { get; set; }

		/// <summary>
		/// 返回此模板下的变量集合
		/// </summary>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600016A RID: 362 RVA: 0x000074AA File Offset: 0x000056AA
		// (set) Token: 0x0600016B RID: 363 RVA: 0x000074B2 File Offset: 0x000056B2
		public VariableCollection Variables { get; private set; }

		/// <summary>
		/// 自定义函数集合
		/// </summary>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600016C RID: 364 RVA: 0x000074BB File Offset: 0x000056BB
		// (set) Token: 0x0600016D RID: 365 RVA: 0x000074C3 File Offset: 0x000056C3
		public UserDefinedFunctionCollection UserDefinedFunctions { get; private set; }

		/// <summary>
		/// 返回此模板下的子模板元素
		/// </summary>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600016E RID: 366 RVA: 0x000074CC File Offset: 0x000056CC
		// (set) Token: 0x0600016F RID: 367 RVA: 0x000074D4 File Offset: 0x000056D4
		public ElementCollection<Template> ChildTemplates { get; set; }

		/// <summary>
		/// 标签容器
		/// </summary>
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000170 RID: 368 RVA: 0x000074DD File Offset: 0x000056DD
		// (set) Token: 0x06000171 RID: 369 RVA: 0x000074E5 File Offset: 0x000056E5
		protected Template TagContainer { get; set; }

		/// <summary>
		/// 返回处理模板数据的实例
		/// </summary>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000074EE File Offset: 0x000056EE
		public Attribute RenderInstance
		{
			get
			{
				return base.Attributes["render"];
			}
		}

		/// <summary>
		/// 返回处理模板数据的特性方法
		/// </summary>
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00007500 File Offset: 0x00005700
		public Attribute RenderMethod
		{
			get
			{
				return base.Attributes["rendermethod"];
			}
		}

		/// <summary>
		/// 返回模板的依赖文件
		/// </summary>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007512 File Offset: 0x00005712
		public string[] FileDependencies
		{
			get
			{
				return this.fileDependencies.ToArray();
			}
		}

		/// <summary>
		/// 添加模板的依赖文件
		/// </summary>
		/// <param name="fileName"></param>
		// Token: 0x06000175 RID: 373 RVA: 0x00007520 File Offset: 0x00005720
		internal void AddFileDependency(string fileName)
		{
			foreach (string text in this.fileDependencies)
			{
				if (text.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
				{
					return;
				}
			}
			this.fileDependencies.Add(fileName);
			if (base.OwnerTemplate != null)
			{
				base.OwnerTemplate.AddFileDependency(fileName);
			}
		}

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x06000176 RID: 374 RVA: 0x00007598 File Offset: 0x00005798
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "file")
				{
					this.File = item.Text;
					return;
				}
				if (!(name == "charset"))
				{
					return;
				}
				this.Charset = Utility.GetEncodingFromCharset(item.Text, base.OwnerTemplate.Charset);
			}
		}

		/// <summary>
		/// 注册全局的自定义函数
		/// </summary>
		/// <param name="function">函数</param>
		// Token: 0x06000177 RID: 375 RVA: 0x000075EE File Offset: 0x000057EE
		public void RegisterGlobalFunction(UserDefinedFunction function)
		{
			this.RegisterGlobalFunction(function.Method.Name, function);
		}

		/// <summary>
		/// 注册全局的自定义函数
		/// </summary>
		/// <param name="functionName">函数名称</param>
		/// <param name="function">函数</param>
		// Token: 0x06000178 RID: 376 RVA: 0x00007604 File Offset: 0x00005804
		public void RegisterGlobalFunction(string functionName, UserDefinedFunction function)
		{
			if (string.IsNullOrEmpty(functionName))
			{
				throw new ArgumentNullException("functionName");
			}
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			this.TagContainer.UserDefinedFunctions.Add(functionName, function);
			foreach (Template template in this.TagContainer.ChildTemplates)
			{
				template.RegisterGlobalFunction(functionName, function);
			}
		}

		/// <summary>
		/// 获取某个Id的子模板.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		// Token: 0x06000179 RID: 377 RVA: 0x0000768C File Offset: 0x0000588C
		public Template GetChildTemplateById(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id");
			}
			foreach (Template template in this.TagContainer.ChildTemplates)
			{
				if (id.Equals(template.Id, StringComparison.InvariantCultureIgnoreCase))
				{
					return template;
				}
				Template childTemplateById = template.GetChildTemplateById(id);
				if (childTemplateById != null)
				{
					return childTemplateById;
				}
			}
			return null;
		}

		/// <summary>
		/// 获取所有具有同名称的模板列表.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		// Token: 0x0600017A RID: 378 RVA: 0x00007710 File Offset: 0x00005910
		public ElementCollection<Template> GetChildTemplatesByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			ElementCollection<Template> elementCollection = new ElementCollection<Template>();
			foreach (Template template in this.TagContainer.ChildTemplates)
			{
				if (name.Equals(template.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					elementCollection.Add(template);
				}
				elementCollection.AddRange(template.GetChildTemplatesByName(name));
			}
			return elementCollection;
		}

		/// <summary>
		/// 设置当前模板块和其下所有子模板块下某个同名称的变量或变量表达式的值
		/// </summary>
		/// <param name="varExp"></param>
		/// <param name="value"></param>
		// Token: 0x0600017B RID: 379 RVA: 0x00007798 File Offset: 0x00005998
		public void SetValue(string varExp, object value)
		{
			this.TagContainer.Variables.SetValue(varExp, value);
			foreach (Template template in this.TagContainer.ChildTemplates)
			{
				template.SetValue(varExp, value);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x0600017C RID: 380 RVA: 0x00007800 File Offset: 0x00005A00
		protected override void RenderTagData(TextWriter writer)
		{
			string text = (this.RenderInstance == null) ? null : this.RenderInstance.GetTextValue();
			string text2 = (this.RenderMethod == null) ? null : this.RenderMethod.GetTextValue();
			if (!string.IsNullOrEmpty(text))
			{
				if (!string.IsNullOrEmpty(text2))
				{
					Utility.PreRenderTemplateByAttributeMethod(text, text2, this);
				}
				else
				{
					Utility.PreRenderTemplate(text, this);
				}
			}
			if (this.Visible)
			{
				base.RenderTagData(writer);
			}
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
		// Token: 0x0600017D RID: 381 RVA: 0x0000786C File Offset: 0x00005A6C
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			ownerTemplate.ChildTemplates.Add(this);
			container.AppendChild(this);
			if (!string.IsNullOrEmpty(this.File))
			{
				this.File = Utility.ResolveFilePath(base.Parent, this.File);
				if (System.IO.File.Exists(this.File))
				{
					base.OwnerTemplate.AddFileDependency(this.File);
					new TemplateDocument(this, System.IO.File.ReadAllText(this.File, this.Charset), ownerTemplate.OwnerDocument.DocumentConfig);
				}
			}
			if (!isClosedTag)
			{
				this.ProcessEndTag(this, this, tagStack, text, ref match);
			}
			return false;
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600017E RID: 382 RVA: 0x00007904 File Offset: 0x00005B04
		internal override Element Clone(Template ownerTemplate)
		{
			Template template = new Template(ownerTemplate);
			ownerTemplate.ChildTemplates.Add(template);
			foreach (Variable variable in this.Variables)
			{
				template.Variables.Add(variable.Clone(template));
			}
			template.Id = base.Id;
			template.Name = base.Name;
			foreach (Attribute attribute in base.Attributes)
			{
				template.Attributes.Add(attribute.Clone(template));
			}
			template.Charset = this.Charset;
			template.File = this.File;
			template.fileDependencies = this.fileDependencies;
			template.Visible = this.Visible;
			foreach (Element element in base.InnerElements)
			{
				template.AppendChild(element.Clone(template));
			}
			return template;
		}

		/// <summary>
		/// 模板的依赖文件列表
		/// </summary>
		// Token: 0x04000045 RID: 69
		protected List<string> fileDependencies;
	}
}
