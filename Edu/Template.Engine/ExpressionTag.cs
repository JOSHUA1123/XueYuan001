using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using VTemplate.Engine.Evaluator;

namespace VTemplate.Engine
{
	/// <summary>
	/// 表达式标签.如: &lt;vt:expression var="totalAge" args="user1.age" args="user2.age" expression="{0}+{1}" /&gt;
	/// </summary>
	// Token: 0x0200002C RID: 44
	public class ExpressionTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x06000226 RID: 550 RVA: 0x0000A22D File Offset: 0x0000842D
		internal ExpressionTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.ExpArgs = new ElementCollection<IExpression>();
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000A241 File Offset: 0x00008441
		public override string TagName
		{
			get
			{
				return "expression";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000A248 File Offset: 0x00008448
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 参与表达式运算的变量参数列表
		/// </summary>
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000A24B File Offset: 0x0000844B
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000A253 File Offset: 0x00008453
		public virtual ElementCollection<IExpression> ExpArgs { get; protected set; }

		/// <summary>
		/// 表达式.
		/// </summary>
		/// <remarks>表达式中可用"{0}","{1}"..之类的标记符表示变量参数的值</remarks>
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000A25C File Offset: 0x0000845C
		public Attribute Expression
		{
			get
			{
				return base.Attributes["Expression"];
			}
		}

		/// <summary>
		/// 存储表达式结果的变量
		/// </summary>
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000A26E File Offset: 0x0000846E
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000A276 File Offset: 0x00008476
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 是否输出此标签的结果值
		/// </summary>
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000A27F File Offset: 0x0000847F
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000A287 File Offset: 0x00008487
		public bool Output { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x06000230 RID: 560 RVA: 0x0000A290 File Offset: 0x00008490
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "args")
				{
					IExpression expression = item.Value;
					if (this.OwnerDocument.DocumentConfig.CompatibleMode && !(expression is VariableExpression))
					{
						expression = ParserHelper.CreateVariableExpression(base.OwnerTemplate, item.Text, false);
					}
					this.ExpArgs.Add(expression);
					return;
				}
				if (name == "var")
				{
					this.Variable = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (!(name == "output"))
				{
					return;
				}
				this.Output = Utility.ConverToBoolean(item.Text);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x06000231 RID: 561 RVA: 0x0000A338 File Offset: 0x00008538
		protected override void RenderTagData(TextWriter writer)
		{
			object obj = null;
			List<object> list = new List<object>();
			foreach (IExpression expression in this.ExpArgs)
			{
				list.Add(expression.GetValue());
			}
			try
			{
				obj = ExpressionEvaluator.Eval(string.Format(this.Expression.GetTextValue(), list.ToArray()));
			}
			catch
			{
				obj = null;
			}
			if (this.Variable != null)
			{
				this.Variable.Value = obj;
			}
			if (this.Output && obj != null)
			{
				writer.Write(obj);
			}
			base.RenderTagData(writer);
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
		// Token: 0x06000232 RID: 562 RVA: 0x0000A3F0 File Offset: 0x000085F0
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Variable == null && !this.Output)
			{
				throw new ParserException(string.Format("{0}标签中如果未定义Output属性为true则必须定义var属性", this.TagName));
			}
			if (this.Expression == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少expression属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000233 RID: 563 RVA: 0x0000A450 File Offset: 0x00008650
		internal override Element Clone(Template ownerTemplate)
		{
			ExpressionTag expressionTag = new ExpressionTag(ownerTemplate);
			this.CopyTo(expressionTag);
			expressionTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			expressionTag.Output = this.Output;
			foreach (IExpression expression in this.ExpArgs)
			{
				expressionTag.ExpArgs.Add(expression.Clone(ownerTemplate));
			}
			return expressionTag;
		}
	}
}
