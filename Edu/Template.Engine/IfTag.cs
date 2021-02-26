using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// If条件标签,如: &lt;vt:if var="member.age" value="20" compare="&lt;="&gt;..&lt;vt:elseif value="30"&gt;..&lt;/vt:if&gt;
	/// </summary>
	// Token: 0x0200001C RID: 28
	public class IfTag : IfConditionTag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x0600013B RID: 315 RVA: 0x00006D78 File Offset: 0x00004F78
		internal IfTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.ElseIfs = new ElementCollection<IfConditionTag>();
		}

		/// <summary>
		/// ElseIf节点列表
		/// </summary>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006D8C File Offset: 0x00004F8C
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00006D94 File Offset: 0x00004F94
		public ElementCollection<IfConditionTag> ElseIfs { get; protected set; }

		/// <summary>
		/// Else节点
		/// </summary>
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006D9D File Offset: 0x00004F9D
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00006DA5 File Offset: 0x00004FA5
		public ElseTag Else
		{
			get
			{
				return this._Else;
			}
			internal set
			{
				if (value != null)
				{
					value.Parent = this;
				}
				this._Else = value;
			}
		}

		/// <summary>
		/// 添加条件
		/// </summary>
		/// <param name="conditionTag"></param>
		// Token: 0x06000140 RID: 320 RVA: 0x00006DB8 File Offset: 0x00004FB8
		internal virtual void AddElseCondition(IfConditionTag conditionTag)
		{
			conditionTag.Parent = this;
			this.ElseIfs.Add(conditionTag);
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00006DCD File Offset: 0x00004FCD
		public override string TagName
		{
			get
			{
				return "if";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00006DD4 File Offset: 0x00004FD4
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 根据Id获取某个子元素标签
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		// Token: 0x06000143 RID: 323 RVA: 0x00006DD8 File Offset: 0x00004FD8
		public override Tag GetChildTagById(string id)
		{
			Tag tag = base.GetChildTagById(id);
			if (tag == null)
			{
				foreach (IfConditionTag ifConditionTag in this.ElseIfs)
				{
					if (id.Equals(ifConditionTag.Id, StringComparison.InvariantCultureIgnoreCase))
					{
						tag = ifConditionTag;
					}
					else
					{
						tag = ifConditionTag.GetChildTagById(id);
					}
					if (tag != null)
					{
						break;
					}
				}
				if (tag == null && this.Else != null)
				{
					if (id.Equals(this.Else.Id, StringComparison.InvariantCultureIgnoreCase))
					{
						tag = this.Else;
					}
					else
					{
						tag = this.Else.GetChildTagById(id);
					}
				}
			}
			return tag;
		}

		/// <summary>
		/// 根据name获取所有同名的子元素标签
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		// Token: 0x06000144 RID: 324 RVA: 0x00006E80 File Offset: 0x00005080
		public override ElementCollection<Tag> GetChildTagsByName(string name)
		{
			ElementCollection<Tag> childTagsByName = base.GetChildTagsByName(name);
			foreach (IfConditionTag ifConditionTag in this.ElseIfs)
			{
				if (name.Equals(ifConditionTag.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					childTagsByName.Add(ifConditionTag);
				}
				childTagsByName.AddRange(ifConditionTag.GetChildTagsByName(name));
			}
			if (this.Else != null)
			{
				if (name.Equals(this.Else.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					childTagsByName.Add(this.Else);
				}
				childTagsByName.AddRange(this.Else.GetChildTagsByName(name));
			}
			return childTagsByName;
		}

		/// <summary>
		/// 根据标签名获取所有同标签名的子元素标签
		/// </summary>
		/// <param name="tagName"></param>
		/// <returns></returns>
		// Token: 0x06000145 RID: 325 RVA: 0x00006F2C File Offset: 0x0000512C
		public override ElementCollection<Tag> GetChildTagsByTagName(string tagName)
		{
			ElementCollection<Tag> childTagsByTagName = base.GetChildTagsByTagName(tagName);
			foreach (IfConditionTag ifConditionTag in this.ElseIfs)
			{
				if (tagName.Equals(ifConditionTag.TagName, StringComparison.InvariantCultureIgnoreCase))
				{
					childTagsByTagName.Add(ifConditionTag);
				}
				childTagsByTagName.AddRange(ifConditionTag.GetChildTagsByTagName(tagName));
			}
			if (this.Else != null)
			{
				if (tagName.Equals(this.Else.TagName, StringComparison.InvariantCultureIgnoreCase))
				{
					childTagsByTagName.Add(this.Else);
				}
				childTagsByTagName.AddRange(this.Else.GetChildTagsByTagName(tagName));
			}
			return childTagsByTagName;
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000146 RID: 326 RVA: 0x00006FD8 File Offset: 0x000051D8
		protected override void RenderTagData(TextWriter writer)
		{
			if (this.IsTestSuccess())
			{
				base.RenderTagData(writer);
				return;
			}
			bool flag = false;
			foreach (IfConditionTag ifConditionTag in this.ElseIfs)
			{
				if (ifConditionTag.IsTestSuccess())
				{
					ifConditionTag.Render(writer);
					flag = true;
					break;
				}
			}
			if (!flag && this.Else != null)
			{
				this.Else.Render(writer);
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
		// Token: 0x06000147 RID: 327 RVA: 0x0000705C File Offset: 0x0000525C
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.VarExpression == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少var属性", this.TagName));
			}
			if (this.Values.Count == 0)
			{
				throw new ParserException(string.Format("{0}标签中缺少value属性", this.TagName));
			}
			if (!isClosedTag)
			{
				container.AppendChild(this);
			}
			return !isClosedTag;
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000148 RID: 328 RVA: 0x000070BC File Offset: 0x000052BC
		internal override Element Clone(Template ownerTemplate)
		{
			IfTag ifTag = new IfTag(ownerTemplate);
			base.CopyTo(ifTag);
			ifTag.Else = ((this.Else == null) ? null : ((ElseTag)this.Else.Clone(ownerTemplate)));
			foreach (IfConditionTag ifConditionTag in this.ElseIfs)
			{
				ifTag.AddElseCondition((IfConditionTag)ifConditionTag.Clone(ownerTemplate));
			}
			return ifTag;
		}

		/// <summary>
		/// Else节点
		/// </summary>
		// Token: 0x0400003F RID: 63
		private ElseTag _Else;
	}
}
