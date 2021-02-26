using System;
using System.Collections;
using System.Collections.Generic;

namespace VTemplate.Engine.Evaluator
{
	/// <summary>
	/// 表达式计算器
	/// </summary>
	// Token: 0x02000037 RID: 55
	public class ExpressionEvaluator
	{
		/// <summary>
		///
		/// </summary>
		// Token: 0x0600028D RID: 653 RVA: 0x0000B430 File Offset: 0x00009630
		private ExpressionEvaluator()
		{
		}

		/// <summary>
		/// 将算术表达式转换为逆波兰表达式
		/// </summary>
		/// <param name="expression">要计算的表达式,如"1+2+3+4"</param>
		// Token: 0x0600028E RID: 654 RVA: 0x0000B438 File Offset: 0x00009638
		private static List<ExpressionNode> ParseExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				return new List<ExpressionNode>();
			}
			List<ExpressionNode> list = new List<ExpressionNode>(10);
			Stack<ExpressionNode> stack = new Stack<ExpressionNode>(5);
			ExpressionParser expressionParser = new ExpressionParser(expression);
			ExpressionNode expressionNode = null;
			bool flag = false;
			ExpressionNode expressionNode2;
			while ((expressionNode2 = expressionParser.ReadNode()) != null)
			{
				if (expressionNode2.Type == ExpressionNodeType.Numeric)
				{
					if (expressionNode != null)
					{
						expressionNode2.UnitaryNode = expressionNode;
						expressionNode = null;
					}
					list.Add(expressionNode2);
					flag = false;
				}
				else if (expressionNode2.Type == ExpressionNodeType.LParentheses)
				{
					stack.Push(expressionNode2);
				}
				else if (expressionNode2.Type == ExpressionNodeType.RParentheses)
				{
					ExpressionNode expressionNode3 = null;
					while (stack.Count > 0)
					{
						expressionNode3 = stack.Pop();
						if (expressionNode3.Type == ExpressionNodeType.LParentheses)
						{
							break;
						}
						list.Add(expressionNode3);
					}
					if (expressionNode3 == null || expressionNode3.Type != ExpressionNodeType.LParentheses)
					{
						throw new ExpressionException(string.Format("在表达式\"{0}\"中没有与在位置({1})上\")\"匹配的\"(\"字符!", expressionParser.Expression, expressionParser.Position));
					}
				}
				else if (stack.Count == 0)
				{
					if (list.Count == 0 && expressionNode2.Type != ExpressionNodeType.LParentheses && expressionNode2.Type != ExpressionNodeType.Not)
					{
						if (!ExpressionNode.IsUnitaryNode(expressionNode2.Type))
						{
							throw new ExpressionException(string.Format("表达式\"{0}\"在位置({1})上缺少操作数!", expressionParser.Expression, expressionParser.Position));
						}
						expressionNode = expressionNode2;
					}
					else
					{
						stack.Push(expressionNode2);
					}
					flag = true;
				}
				else if (flag)
				{
					if (!ExpressionNode.IsUnitaryNode(expressionNode2.Type) || expressionNode != null)
					{
						throw new ExpressionException(string.Format("表达式\"{0}\"在位置({1})上缺少操作数!", expressionParser.Expression, expressionParser.Position));
					}
					expressionNode = expressionNode2;
				}
				else
				{
					do
					{
						ExpressionNode expressionNode4 = stack.Peek();
						if (expressionNode4.Type == ExpressionNodeType.LParentheses || expressionNode4.PRI - expressionNode2.PRI < 0)
						{
							break;
						}
						list.Add(stack.Pop());
					}
					while (stack.Count > 0);
					stack.Push(expressionNode2);
					flag = true;
				}
			}
			if (flag)
			{
				throw new ExpressionException(string.Format("表达式\"{0}\"在位置({1})上缺少操作数!", expressionParser.Expression, expressionParser.Position));
			}
			while (stack.Count > 0)
			{
				ExpressionNode expressionNode4 = stack.Pop();
				if (expressionNode4.Type == ExpressionNodeType.LParentheses)
				{
					throw new ExpressionException(string.Format("表达式\"{0}\"中括号不匹配,丢失右括号!", expressionParser.Expression, expressionParser.Position));
				}
				list.Add(expressionNode4);
			}
			return list;
		}

		/// <summary>
		/// 对逆波兰表达式进行计算
		/// </summary>
		/// <param name="nodes"></param>
		/// <returns></returns>
		// Token: 0x0600028F RID: 655 RVA: 0x0000B678 File Offset: 0x00009878
		private static object CalcExpression(List<ExpressionNode> nodes)
		{
			if (nodes == null || nodes.Count == 0)
			{
				return null;
			}
			if (nodes.Count > 1)
			{
				int i = 0;
				ArrayList arrayList = new ArrayList();
				while (i < nodes.Count)
				{
					ExpressionNode expressionNode = nodes[i];
					ExpressionNodeType type = expressionNode.Type;
					if (type == ExpressionNodeType.Numeric)
					{
						arrayList.Add(expressionNode.Numeric);
						i++;
					}
					else
					{
						int num = 2;
						if (expressionNode.Type == ExpressionNodeType.Not)
						{
							num = 1;
						}
						if (arrayList.Count < num)
						{
							throw new ExpressionException("缺少操作数");
						}
						object[] array = new object[num];
						for (int j = 0; j < num; j++)
						{
							array[j] = arrayList[i - num + j];
						}
						expressionNode.Numeric = ExpressionEvaluator.Calculate(expressionNode.Type, array);
						expressionNode.Type = ExpressionNodeType.Numeric;
						for (int k = 0; k < num; k++)
						{
							nodes.RemoveAt(i - k - 1);
							arrayList.RemoveAt(i - k - 1);
						}
						i -= num;
					}
				}
			}
			if (nodes.Count != 1)
			{
				throw new ExpressionException("缺少操作符或操作数");
			}
			ExpressionNodeType type2 = nodes[0].Type;
			if (type2 == ExpressionNodeType.Numeric)
			{
				return nodes[0].Numeric;
			}
			throw new ExpressionException("缺少操作数");
		}

		/// <summary>
		/// 计算节点的值
		/// </summary>
		/// <param name="nodeType">节点的类型</param>
		/// <param name="data">要计算的值,有可能是两位或一位数</param>
		/// <returns></returns>
		// Token: 0x06000290 RID: 656 RVA: 0x0000B7B4 File Offset: 0x000099B4
		private static object Calculate(ExpressionNodeType nodeType, object[] data)
		{
			switch (nodeType)
			{
			case ExpressionNodeType.Plus:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num + num2;
			}
			case ExpressionNodeType.Subtract:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num - num2;
			}
			case ExpressionNodeType.MultiPly:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num * num2;
			}
			case ExpressionNodeType.Divide:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				if (num2 == 0m)
				{
					throw new DivideByZeroException();
				}
				return num / num2;
			}
			case ExpressionNodeType.Mod:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				if (num2 == 0m)
				{
					throw new DivideByZeroException();
				}
				return num % num2;
			}
			case ExpressionNodeType.Power:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return Math.Pow((double)num, (double)num2);
			}
			case ExpressionNodeType.BitwiseAnd:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return (int)num & (int)num2;
			}
			case ExpressionNodeType.BitwiseOr:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return (int)num | (int)num2;
			}
			case ExpressionNodeType.And:
			{
				bool flag = ExpressionEvaluator.ConvertToBool(data[0]);
				bool flag2 = ExpressionEvaluator.ConvertToBool(data[1]);
				return flag && flag2;
			}
			case ExpressionNodeType.Or:
			{
				bool flag = ExpressionEvaluator.ConvertToBool(data[0]);
				bool flag2 = ExpressionEvaluator.ConvertToBool(data[1]);
				return flag || flag2;
			}
			case ExpressionNodeType.Not:
			{
				bool flag = ExpressionEvaluator.ConvertToBool(data[0]);
				return !flag;
			}
			case ExpressionNodeType.Equal:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num == num2;
			}
			case ExpressionNodeType.Unequal:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num != num2;
			}
			case ExpressionNodeType.GT:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num > num2;
			}
			case ExpressionNodeType.LT:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num < num2;
			}
			case ExpressionNodeType.GTOrEqual:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num >= num2;
			}
			case ExpressionNodeType.LTOrEqual:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return num <= num2;
			}
			case ExpressionNodeType.LShift:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return (long)num << (int)num2;
			}
			case ExpressionNodeType.RShift:
			{
				decimal num = ExpressionEvaluator.ConvertToDecimal(data[0]);
				decimal num2 = ExpressionEvaluator.ConvertToDecimal(data[1]);
				return (long)num >> (int)num2;
			}
			}
			return 0;
		}

		/// <summary>
		/// 将某个值转换为bool值
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x06000291 RID: 657 RVA: 0x0000BABF File Offset: 0x00009CBF
		private static bool ConvertToBool(object value)
		{
			if (value is bool)
			{
				return (bool)value;
			}
			return Convert.ToDecimal(value) == 1m;
		}

		/// <summary>
		/// 将某个值转换为decimal值
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		// Token: 0x06000292 RID: 658 RVA: 0x0000BAE1 File Offset: 0x00009CE1
		private static decimal ConvertToDecimal(object value)
		{
			if (value is bool)
			{
				return ((bool)value) ? 1 : 0;
			}
			return Convert.ToDecimal(value);
		}

		/// <summary>
		/// 对表达式进行计算
		/// </summary>
		/// <param name="expression">要计算的表达式,如"1+2+3+4"</param>
		/// <returns>返回计算结果,如果带有逻辑运算符则返回true/false,否则返回数值</returns>
		// Token: 0x06000293 RID: 659 RVA: 0x0000BB03 File Offset: 0x00009D03
		public static object Eval(string expression)
		{
			return ExpressionEvaluator.CalcExpression(ExpressionEvaluator.ParseExpression(expression));
		}
	}
}
