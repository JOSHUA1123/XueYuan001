using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VTemplate.Engine.Evaluator;

namespace VTemplate.Engine
{
	/// <summary>
	/// If条件标签.
	/// </summary>
	// Token: 0x0200001B RID: 27
	public class IfConditionTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x0600012C RID: 300 RVA: 0x0000646C File Offset: 0x0000466C
		internal IfConditionTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.Values = new ElementCollection<IExpression>();
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00006480 File Offset: 0x00004680
		public override string TagName
		{
			get
			{
				return "elseif";
			}
		}

		/// <summary>
		/// 返回标签的结束标签名称.
		/// </summary>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006487 File Offset: 0x00004687
		public override string EndTagName
		{
			get
			{
				return "if";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000648E File Offset: 0x0000468E
		internal override bool IsSingleTag
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// 条件变量
		/// </summary>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006491 File Offset: 0x00004691
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00006499 File Offset: 0x00004699
		public virtual VariableExpression VarExpression { get; protected set; }

		/// <summary>
		/// 比较值列表
		/// </summary>
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000064A2 File Offset: 0x000046A2
		// (set) Token: 0x06000133 RID: 307 RVA: 0x000064AA File Offset: 0x000046AA
		public virtual ElementCollection<IExpression> Values { get; protected set; }

		/// <summary>
		/// 比较类型
		/// </summary>
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000064B3 File Offset: 0x000046B3
		public Attribute Compare
		{
			get
			{
				return base.Attributes["Compare"];
			}
		}

		/// <summary>
		/// 表达式.
		/// </summary>
		/// <remarks>表达式中可用"{0}"标记符表示条件变量的值</remarks>
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000064C5 File Offset: 0x000046C5
		public Attribute Expression
		{
			get
			{
				return base.Attributes["Expression"];
			}
		}

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x06000136 RID: 310 RVA: 0x000064D8 File Offset: 0x000046D8
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "var")
				{
					this.VarExpression = ParserHelper.CreateVariableExpression(base.OwnerTemplate, item.Text, false);
					return;
				}
				if (!(name == "value"))
				{
					return;
				}
				this.Values.Add(item.Value);
			}
		}

		/// <summary>
		/// 判断测试条件是否成功
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000137 RID: 311 RVA: 0x00006530 File Offset: 0x00004730
		internal virtual bool IsTestSuccess()
		{
			if (this.Values == null || this.VarExpression == null)
			{
				return true;
			}
			object obj = this.VarExpression.GetValue();
			IfConditionCompareType ifConditionCompareType = (this.Compare == null) ? IfConditionCompareType.Equal : Utility.GetIfConditionCompareType(this.Compare.GetTextValue());
			if (Utility.IsNothing(obj))
			{
				switch (ifConditionCompareType)
				{
				case IfConditionCompareType.Equal:
				case IfConditionCompareType.Contains:
					foreach (IExpression expression in this.Values)
					{
						object value = expression.GetValue();
						string a = Utility.IsNothing(value) ? string.Empty : value.ToString();
						if (a == string.Empty)
						{
							return true;
						}
					}
					return false;
				case IfConditionCompareType.LT:
				case IfConditionCompareType.LTAndEqual:
					return true;
				case IfConditionCompareType.UnEqual:
					foreach (IExpression expression2 in this.Values)
					{
						object value2 = expression2.GetValue();
						string a2 = Utility.IsNothing(value2) ? string.Empty : value2.ToString();
						if (a2 != string.Empty)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
			string text = (this.Expression == null) ? string.Empty : this.Expression.GetTextValue();
			if (!string.IsNullOrEmpty(text))
			{
				object obj2 = obj;
				try
				{
					obj = ExpressionEvaluator.Eval(string.Format(text, obj));
				}
				catch
				{
					obj = obj2;
				}
			}
			switch (ifConditionCompareType)
			{
			case IfConditionCompareType.LT:
				foreach (IExpression expression3 in this.Values)
				{
					object value3 = expression3.GetValue();
					bool flag;
					if (!Utility.IsNothing(value3) && Utility.CompareTo(obj, value3, out flag) < 0 && flag)
					{
						return true;
					}
				}
				return false;
			case IfConditionCompareType.LTAndEqual:
				foreach (IExpression expression4 in this.Values)
				{
					object value4 = expression4.GetValue();
					bool flag;
					if (!Utility.IsNothing(value4) && Utility.CompareTo(obj, value4, out flag) <= 0 && flag)
					{
						return true;
					}
				}
				return false;
			case IfConditionCompareType.GT:
				foreach (IExpression expression5 in this.Values)
				{
					object value5 = expression5.GetValue();
					bool flag;
					if (!Utility.IsNothing(value5) && Utility.CompareTo(obj, value5, out flag) > 0 && flag)
					{
						return true;
					}
				}
				return false;
			case IfConditionCompareType.GTAndEqual:
				foreach (IExpression expression6 in this.Values)
				{
					object value6 = expression6.GetValue();
					bool flag;
					if (!Utility.IsNothing(value6) && Utility.CompareTo(obj, value6, out flag) >= 0 && flag)
					{
						return true;
					}
				}
				return false;
			case IfConditionCompareType.UnEqual:
				foreach (IExpression expression7 in this.Values)
				{
					object value7 = expression7.GetValue();
					if (Utility.IsNothing(value7))
					{
						return true;
					}
					bool flag;
					if (obj is string || (value7 is string && !(obj is IConvertible)))
					{
						if (!string.Equals(value7.ToString(), obj.ToString(), StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
					}
					else if (!(obj is IComparable))
					{
						if (value7 is Type)
						{
							if (obj.GetType() != (Type)value7)
							{
								return true;
							}
						}
						else if (obj.GetType() == value7.GetType())
						{
							if (obj != value7)
							{
								return true;
							}
						}
						else if (Utility.CompareTo(obj, value7, out flag) != 0 && flag)
						{
							return true;
						}
					}
					else if (Utility.CompareTo(obj, value7, out flag) != 0 && flag)
					{
						return true;
					}
				}
				return false;
			case IfConditionCompareType.StartWith:
			{
				string text2 = obj.ToString();
				foreach (IExpression expression8 in this.Values)
				{
					object value8 = expression8.GetValue();
					if (!Utility.IsNothing(value8))
					{
						string value9 = value8.ToString();
						if (!string.IsNullOrEmpty(value9) && text2.StartsWith(value9, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
				return false;
			}
			case IfConditionCompareType.EndWith:
			{
				string text3 = obj.ToString();
				foreach (IExpression expression9 in this.Values)
				{
					object value10 = expression9.GetValue();
					if (!Utility.IsNothing(value10))
					{
						string value11 = value10.ToString();
						if (!string.IsNullOrEmpty(value11) && text3.EndsWith(value11, StringComparison.OrdinalIgnoreCase))
						{
							return true;
						}
					}
				}
				return false;
			}
			case IfConditionCompareType.Contains:
			{
				string text4 = obj.ToString();
				foreach (IExpression expression10 in this.Values)
				{
					object value12 = expression10.GetValue();
					if (!Utility.IsNothing(value12))
					{
						string value13 = value12.ToString();
						if (!string.IsNullOrEmpty(value13) && text4.IndexOf(value13, StringComparison.OrdinalIgnoreCase) != -1)
						{
							return true;
						}
					}
				}
				return false;
			}
			default:
				foreach (IExpression expression11 in this.Values)
				{
					object value14 = expression11.GetValue();
					if (!Utility.IsNothing(value14))
					{
						bool flag;
						if (obj is string || (value14 is string && !(obj is IConvertible)))
						{
							if (string.Equals(value14.ToString(), obj.ToString(), StringComparison.InvariantCultureIgnoreCase))
							{
								return true;
							}
						}
						else if (!(obj is IComparable))
						{
							if (value14 is Type)
							{
								if (obj.GetType() == (Type)value14)
								{
									return true;
								}
							}
							else if (obj.GetType() == value14.GetType())
							{
								if (obj == value14)
								{
									return true;
								}
							}
							else if (Utility.CompareTo(obj, value14, out flag) == 0 && flag)
							{
								return true;
							}
						}
						else if (Utility.CompareTo(obj, value14, out flag) == 0 && flag)
						{
							return true;
						}
					}
				}
				return false;
			}
			bool result;
			return result;
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
		// Token: 0x06000138 RID: 312 RVA: 0x00006C50 File Offset: 0x00004E50
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Values.Count == 0)
			{
				throw new ParserException(string.Format("{0}标签中缺少value属性", this.TagName));
			}
			if (!(container is IfTag))
			{
				throw new ParserException(string.Format("未找到和{0}标签对应的{1}标签", this.TagName, this.EndTagName));
			}
			IfTag ifTag = (IfTag)container;
			if (this.VarExpression == null)
			{
				this.VarExpression = ifTag.VarExpression;
			}
			ifTag.AddElseCondition(this);
			return true;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tag"></param>
		// Token: 0x06000139 RID: 313 RVA: 0x00006CC8 File Offset: 0x00004EC8
		protected void CopyTo(IfConditionTag tag)
		{
			base.CopyTo(tag);
			tag.VarExpression = ((this.VarExpression == null) ? null : ((VariableExpression)this.VarExpression.Clone(tag.OwnerTemplate)));
			if (this.Values != null)
			{
				foreach (IExpression expression in this.Values)
				{
					tag.Values.Add(expression.Clone(tag.OwnerTemplate));
				}
			}
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x0600013A RID: 314 RVA: 0x00006D5C File Offset: 0x00004F5C
		internal override Element Clone(Template ownerTemplate)
		{
			IfConditionTag ifConditionTag = new IfConditionTag(ownerTemplate);
			this.CopyTo(ifConditionTag);
			return ifConditionTag;
		}
	}
}
