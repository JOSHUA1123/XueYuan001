using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// 函数调用标签.如: &lt;vt:function var="MaxAge" method="Max" type="System.Math" args="$user1.age" args="$user2.age" /&gt;
	/// </summary>
	// Token: 0x02000028 RID: 40
	public class FunctionTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x06000206 RID: 518 RVA: 0x00009A96 File Offset: 0x00007C96
		internal FunctionTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.FunctionArgs = new ElementCollection<IExpression>();
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00009AAA File Offset: 0x00007CAA
		public override string TagName
		{
			get
			{
				return "function";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00009AB1 File Offset: 0x00007CB1
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 参与函数运算的参数列表
		/// </summary>
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00009AB4 File Offset: 0x00007CB4
		// (set) Token: 0x0600020A RID: 522 RVA: 0x00009ABC File Offset: 0x00007CBC
		public virtual ElementCollection<IExpression> FunctionArgs { get; protected set; }

		/// <summary>
		/// 调用的方法
		/// </summary>
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00009AC5 File Offset: 0x00007CC5
		public Attribute Method
		{
			get
			{
				return base.Attributes["Method"];
			}
		}

		/// <summary>
		/// 包含方法的类型
		/// </summary>
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00009AD7 File Offset: 0x00007CD7
		public new Attribute Type
		{
			get
			{
				return base.Attributes["Type"];
			}
		}

		/// <summary>
		/// 存储表达式结果的变量
		/// </summary>
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00009AE9 File Offset: 0x00007CE9
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00009AF1 File Offset: 0x00007CF1
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 是否输出此标签的结果值
		/// </summary>
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00009AFA File Offset: 0x00007CFA
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00009B02 File Offset: 0x00007D02
		public bool Output { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x06000211 RID: 529 RVA: 0x00009B0C File Offset: 0x00007D0C
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "args")
				{
					this.FunctionArgs.Add(item.Value);
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
		// Token: 0x06000212 RID: 530 RVA: 0x00009B84 File Offset: 0x00007D84
		protected override void RenderTagData(TextWriter writer)
		{
			object functionResult = this.GetFunctionResult();
			if (this.Variable != null)
			{
				this.Variable.Value = functionResult;
			}
			if (this.Output && functionResult != null)
			{
				writer.Write(functionResult);
			}
			base.RenderTagData(writer);
		}

		/// <summary>
		/// 获取函数的结果
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000213 RID: 531 RVA: 0x00009BC8 File Offset: 0x00007DC8
		private object GetFunctionResult()
		{
			object result = null;
			List<object> list = new List<object>();
			List<Type> list2 = new List<Type>();
			foreach (IExpression expression in this.FunctionArgs)
			{
				object value = expression.GetValue();
				list.Add(value);
				list2.Add((value == null) ? typeof(object) : value.GetType());
			}
			string textValue = this.Method.GetTextValue();
			if (this.Type == null)
			{
				UserDefinedFunction userDefinedFunction;
				if (base.OwnerTemplate.UserDefinedFunctions.TryGetValue(textValue, out userDefinedFunction))
				{
					result = userDefinedFunction(list.ToArray());
				}
			}
			else
			{
				object obj = (this.Type.Value is VariableExpression) ? this.Type.Value.GetValue() : Utility.CreateType(this.Type.Value.GetValue().ToString());
				if (obj != null)
				{
					Type type = (obj is Type) ? ((Type)obj) : obj.GetType();
					BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public;
					if (!(obj is Type))
					{
						bindingFlags |= BindingFlags.Instance;
					}
					MethodInfo method = type.GetMethod(textValue, bindingFlags, null, list2.ToArray(), null);
					if (method == null)
					{
						MemberInfo[] member = type.GetMember(textValue, bindingFlags | BindingFlags.InvokeMethod);
						MemberInfo[] array = member;
						int i = 0;
						while (i < array.Length)
						{
							MethodInfo methodInfo = (MethodInfo)array[i];
							ParameterInfo[] parameters = methodInfo.GetParameters();
							if (parameters.Length == 1 && parameters[0].ParameterType.IsArray && parameters[0].ParameterType.FullName == "System.Object[]")
							{
								try
								{
									result = methodInfo.Invoke((obj is Type) ? null : obj, new object[]
									{
										list.ToArray()
									});
									break;
								}
								catch
								{
									goto IL_297;
								}
								goto IL_1DB;
							}
							goto IL_1DB;
							IL_297:
							i++;
							continue;
							IL_1DB:
							if (parameters.Length == list.Count)
							{
								List<object> list3 = new List<object>();
								for (int j = 0; j < parameters.Length; j++)
								{
									object obj2 = list[j];
									if (parameters[j].ParameterType != list2[j] && obj2 != null)
									{
										obj2 = Utility.ConvertTo(list[j].ToString(), parameters[j].ParameterType);
										if (obj2 == null)
										{
											break;
										}
										list3.Add(obj2);
									}
									else
									{
										list3.Add(obj2);
									}
								}
								if (list3.Count == parameters.Length)
								{
									try
									{
										result = methodInfo.Invoke((obj is Type) ? null : obj, list3.ToArray());
										break;
									}
									catch
									{
									}
								}
								list3.Clear();
								goto IL_297;
							}
							goto IL_297;
						}
					}
					else
					{
						try
						{
							result = method.Invoke((obj is Type) ? null : obj, list.ToArray());
						}
						catch
						{
							result = null;
						}
					}
				}
			}
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
		// Token: 0x06000214 RID: 532 RVA: 0x00009ED8 File Offset: 0x000080D8
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Method == null || string.IsNullOrEmpty(this.Method.Text))
			{
				throw new ParserException(string.Format("{0}标签中缺少method属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x06000215 RID: 533 RVA: 0x00009F24 File Offset: 0x00008124
		internal override Element Clone(Template ownerTemplate)
		{
			FunctionTag functionTag = new FunctionTag(ownerTemplate);
			this.CopyTo(functionTag);
			functionTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			functionTag.Output = this.Output;
			foreach (IExpression expression in this.FunctionArgs)
			{
				functionTag.FunctionArgs.Add(expression.Clone(ownerTemplate));
			}
			return functionTag;
		}
	}
}
