using System;
using System.Text;

namespace VTemplate.Engine.Evaluator
{
	/// <summary>
	/// 表达式解析器
	/// </summary>
	// Token: 0x02000034 RID: 52
	public class ExpressionParser
	{
		/// <summary>
		/// 构造表达式解析器
		/// </summary>
		/// <param name="expression">要分析的表达式,如"1+2+3+4"</param>
		// Token: 0x0600027B RID: 635 RVA: 0x0000B1A1 File Offset: 0x000093A1
		public ExpressionParser(string expression)
		{
			this._Expression = expression;
			this._Position = 0;
		}

		/// <summary>
		/// 返回当前分析的表达式
		/// </summary>
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000B1B7 File Offset: 0x000093B7
		public string Expression
		{
			get
			{
				return this._Expression;
			}
		}

		/// <summary>
		/// 返回当前读取的位置
		/// </summary>
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000B1BF File Offset: 0x000093BF
		public int Position
		{
			get
			{
				return this._Position;
			}
		}

		/// <summary>
		/// 读取下一个表达式节点,如果读取失败则返回null
		/// </summary>
		/// <returns></returns>
		// Token: 0x0600027E RID: 638 RVA: 0x0000B1C8 File Offset: 0x000093C8
		public ExpressionNode ReadNode()
		{
			int num = -1;
			StringBuilder stringBuilder = new StringBuilder(10);
			while (this._Position < this._Expression.Length)
			{
				char c = this._Expression[this._Position];
				if (ExpressionNode.IsWhileSpace(c))
				{
					if (num >= 0 && this._Position - num > 1)
					{
						throw new ExpressionException(string.Format("表达式\"{0}\"在位置({1})上的字符非法!", this._Expression, this._Position));
					}
					if (stringBuilder.Length == 0)
					{
						num = -1;
					}
					else
					{
						num = this._Position;
					}
					this._Position++;
				}
				else
				{
					if (stringBuilder.Length != 0 && !ExpressionNode.IsCongener(c, stringBuilder[stringBuilder.Length - 1]))
					{
						break;
					}
					this._Position++;
					stringBuilder.Append(c);
					if (!ExpressionNode.NeedMoreOperator(c))
					{
						break;
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			ExpressionNode expressionNode = new ExpressionNode(stringBuilder.ToString());
			if (expressionNode.Type == ExpressionNodeType.Unknown)
			{
				throw new ExpressionException(string.Format("表达式\"{0}\"在位置({1})上的字符\"{2}\"非法!", this._Expression, this._Position - expressionNode.Value.Length, expressionNode.Value));
			}
			return expressionNode;
		}

		/// <summary>
		/// 当前分析的表达式
		/// </summary>
		// Token: 0x0400009B RID: 155
		private string _Expression;

		/// <summary>
		/// 当前读取的位置
		/// </summary>
		// Token: 0x0400009C RID: 156
		private int _Position;
	}
}
