using System;
using System.Text.RegularExpressions;

namespace VTemplate.Engine.Evaluator
{
	/// <summary>
	/// 表达式的节点(如操作数或运算符)
	/// </summary>
	// Token: 0x02000023 RID: 35
	public class ExpressionNode
	{
		/// <summary>
		/// 构造节点实例
		/// </summary>
		/// <param name="value">操作数或运算符</param>
		// Token: 0x060001CB RID: 459 RVA: 0x00008D64 File Offset: 0x00006F64
		public ExpressionNode(string value)
		{
			this._Value = value;
			this._Type = ExpressionNode.ParseNodeType(value);
			this._PRI = ExpressionNode.GetNodeTypePRI(this.Type);
			this._Numeric = null;
		}

		/// <summary>
		/// 返回当前节点的操作数
		/// </summary>
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00008D97 File Offset: 0x00006F97
		public string Value
		{
			get
			{
				return this._Value;
			}
		}

		/// <summary>
		/// 返回当前节点的类型
		/// </summary>
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00008D9F File Offset: 0x00006F9F
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00008DA7 File Offset: 0x00006FA7
		public ExpressionNodeType Type
		{
			get
			{
				return this._Type;
			}
			internal set
			{
				this._Type = value;
			}
		}

		/// <summary>
		/// 返回当前节点的优先级
		/// </summary>
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00008DB0 File Offset: 0x00006FB0
		public int PRI
		{
			get
			{
				return this._PRI;
			}
		}

		/// <summary>
		/// 返回此节点的数值
		/// </summary>
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00008DB8 File Offset: 0x00006FB8
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x00008E21 File Offset: 0x00007021
		public object Numeric
		{
			get
			{
				if (this._Numeric == null)
				{
					if (this.Type != ExpressionNodeType.Numeric)
					{
						return 0;
					}
					decimal num = Convert.ToDecimal(this.Value);
					if (this.UnitaryNode != null)
					{
						ExpressionNodeType type = this.UnitaryNode.Type;
						if (type == ExpressionNodeType.Subtract)
						{
							num = 0m - num;
						}
					}
					this._Numeric = num;
				}
				return this._Numeric;
			}
			internal set
			{
				this._Numeric = value;
				this._Value = this._Numeric.ToString();
			}
		}

		/// <summary>
		/// 设置或返回与当前节点相关联的一元操作符节点
		/// </summary>
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00008E3B File Offset: 0x0000703B
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00008E43 File Offset: 0x00007043
		public ExpressionNode UnitaryNode
		{
			get
			{
				return this._UnitaryNode;
			}
			set
			{
				this._UnitaryNode = value;
			}
		}

		/// <summary>
		/// 解析节点类型
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x060001D4 RID: 468 RVA: 0x00008E4C File Offset: 0x0000704C
		private static ExpressionNodeType ParseNodeType(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return ExpressionNodeType.Unknown;
			}
			switch (value)
			{
			case "+":
				return ExpressionNodeType.Plus;
			case "-":
				return ExpressionNodeType.Subtract;
			case "*":
				return ExpressionNodeType.MultiPly;
			case "/":
				return ExpressionNodeType.Divide;
			case "%":
				return ExpressionNodeType.Mod;
			case "^":
				return ExpressionNodeType.Power;
			case "(":
				return ExpressionNodeType.LParentheses;
			case ")":
				return ExpressionNodeType.RParentheses;
			case "&":
				return ExpressionNodeType.BitwiseAnd;
			case "|":
				return ExpressionNodeType.BitwiseOr;
			case "&&":
				return ExpressionNodeType.And;
			case "||":
				return ExpressionNodeType.Or;
			case "!":
				return ExpressionNodeType.Not;
			case "==":
				return ExpressionNodeType.Equal;
			case "!=":
			case "<>":
				return ExpressionNodeType.Unequal;
			case ">":
				return ExpressionNodeType.GT;
			case "<":
				return ExpressionNodeType.LT;
			case ">=":
				return ExpressionNodeType.GTOrEqual;
			case "<=":
				return ExpressionNodeType.LTOrEqual;
			case "<<":
				return ExpressionNodeType.LShift;
			case ">>":
				return ExpressionNodeType.RShift;
			}
			if (ExpressionNode.IsNumerics(value))
			{
				return ExpressionNodeType.Numeric;
			}
			return ExpressionNodeType.Unknown;
		}

		/// <summary>
		/// 获取各节点类型的优先级
		/// </summary>
		/// <param name="nodeType"></param>
		/// <returns></returns>
		// Token: 0x060001D5 RID: 469 RVA: 0x00009054 File Offset: 0x00007254
		private static int GetNodeTypePRI(ExpressionNodeType nodeType)
		{
			switch (nodeType)
			{
			case ExpressionNodeType.Plus:
			case ExpressionNodeType.Subtract:
				return 5;
			case ExpressionNodeType.MultiPly:
			case ExpressionNodeType.Divide:
			case ExpressionNodeType.Power:
				return 6;
			case ExpressionNodeType.LParentheses:
			case ExpressionNodeType.RParentheses:
				return 9;
			case ExpressionNodeType.Mod:
				return 7;
			case ExpressionNodeType.BitwiseAnd:
			case ExpressionNodeType.BitwiseOr:
				return 3;
			case ExpressionNodeType.And:
			case ExpressionNodeType.Or:
				return 1;
			case ExpressionNodeType.Not:
				return 8;
			case ExpressionNodeType.Equal:
			case ExpressionNodeType.Unequal:
			case ExpressionNodeType.GT:
			case ExpressionNodeType.LT:
			case ExpressionNodeType.GTOrEqual:
			case ExpressionNodeType.LTOrEqual:
				return 2;
			case ExpressionNodeType.LShift:
			case ExpressionNodeType.RShift:
				return 4;
			default:
				return 0;
			}
		}

		/// <summary>
		/// 判断某个操作数是否是数值
		/// </summary>
		/// <param name="op"></param>
		/// <returns></returns>
		// Token: 0x060001D6 RID: 470 RVA: 0x000090D5 File Offset: 0x000072D5
		public static bool IsNumerics(string op)
		{
			return ExpressionNode.Numerics.IsMatch(op);
		}

		/// <summary>
		/// 判断某个字符后是否需要更多的操作符
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		// Token: 0x060001D7 RID: 471 RVA: 0x000090E4 File Offset: 0x000072E4
		public static bool NeedMoreOperator(char c)
		{
			if (c <= '&')
			{
				if (c != '!' && c != '&')
				{
					goto IL_34;
				}
			}
			else if (c != '.')
			{
				switch (c)
				{
				case '<':
				case '=':
				case '>':
					break;
				default:
					if (c != '|')
					{
						goto IL_34;
					}
					break;
				}
			}
			return true;
			IL_34:
			return char.IsDigit(c);
		}

		/// <summary>
		/// 判断两个字符是否是同一类
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <returns></returns>
		// Token: 0x060001D8 RID: 472 RVA: 0x0000912C File Offset: 0x0000732C
		public static bool IsCongener(char c1, char c2)
		{
			if (c1 == '(' || c2 == '(')
			{
				return false;
			}
			if (c1 == ')' || c2 == ')')
			{
				return false;
			}
			if (char.IsDigit(c1) || c1 == '.')
			{
				return char.IsDigit(c2) || c2 == '.';
			}
			return !char.IsDigit(c2) && c2 != '.';
		}

		/// <summary>
		/// 判断某个字符是否是空白字符
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		// Token: 0x060001D9 RID: 473 RVA: 0x00009180 File Offset: 0x00007380
		public static bool IsWhileSpace(char c)
		{
			return c == ' ' || c == '\t';
		}

		/// <summary>
		/// 判断是否是一元操作符节点
		/// </summary>
		/// <param name="nodeType"></param>
		/// <returns></returns>
		// Token: 0x060001DA RID: 474 RVA: 0x0000918E File Offset: 0x0000738E
		public static bool IsUnitaryNode(ExpressionNodeType nodeType)
		{
			return nodeType == ExpressionNodeType.Plus || nodeType == ExpressionNodeType.Subtract;
		}

		// Token: 0x04000059 RID: 89
		private string _Value;

		// Token: 0x0400005A RID: 90
		private ExpressionNodeType _Type;

		// Token: 0x0400005B RID: 91
		private int _PRI;

		// Token: 0x0400005C RID: 92
		private object _Numeric;

		// Token: 0x0400005D RID: 93
		private ExpressionNode _UnitaryNode;

		/// <summary>
		/// 操作数的正则表达式
		/// </summary>
		// Token: 0x0400005E RID: 94
		private static Regex Numerics = new Regex("^[\\+\\-]?(0|[1-9]\\d*|[1-9]\\d*\\.\\d+|0\\.\\d+)$", RegexOptions.Compiled);
	}
}
