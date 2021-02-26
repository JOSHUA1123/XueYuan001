using System;
using System.Collections.Generic;
using System.Text;

namespace VTemplate.Engine
{
	/// <summary>
	/// 变量表达式,如:{$:name.age} 变量元素中的变量表达式则是".age"
	/// </summary>
	// Token: 0x02000026 RID: 38
	public class VariableExpression : IExpression, ICloneableElement<IExpression>
	{
		/// <summary>
		/// 变量表达式
		/// </summary>
		/// <param name="variableId"></param>
		/// <param name="needCacheData"></param>
		// Token: 0x060001E3 RID: 483 RVA: 0x00009248 File Offset: 0x00007448
		internal VariableExpression(VariableIdentity variableId, bool needCacheData) : this(variableId, null, false, needCacheData)
		{
		}

		/// <summary>
		/// 变量表达式
		/// </summary>
		/// <param name="variableId"></param>
		/// <param name="fieldName"></param>
		/// <param name="isMethod"></param>
		/// <param name="needCacheData"></param>
		// Token: 0x060001E4 RID: 484 RVA: 0x00009254 File Offset: 0x00007454
		internal VariableExpression(VariableIdentity variableId, string fieldName, bool isMethod, bool needCacheData)
		{
			this.VariableId = variableId;
			this.FieldName = fieldName;
			this.IsMethod = isMethod;
			this.NeedCacheData = needCacheData;
		}

		/// <summary>
		/// 变量表达式
		/// </summary>
		/// <param name="parentExp"></param>
		/// <param name="fieldName"></param>
		/// <param name="isMethod"></param>
		// Token: 0x060001E5 RID: 485 RVA: 0x00009279 File Offset: 0x00007479
		internal VariableExpression(VariableExpression parentExp, string fieldName, bool isMethod)
		{
			parentExp.NextExpression = this;
			this.ParentExpression = parentExp;
			this.VariableId = parentExp.VariableId;
			this.FieldName = fieldName;
			this.IsMethod = isMethod;
			this.NeedCacheData = parentExp.NeedCacheData;
		}

		/// <summary>
		/// 变量标识
		/// </summary>
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000092B5 File Offset: 0x000074B5
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x000092BD File Offset: 0x000074BD
		public VariableIdentity VariableId { get; private set; }

		/// <summary>
		/// 字段名
		/// </summary>
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000092C6 File Offset: 0x000074C6
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x000092CE File Offset: 0x000074CE
		public string FieldName { get; private set; }

		/// <summary>
		/// 是否是方法
		/// </summary>
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000092D7 File Offset: 0x000074D7
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000092DF File Offset: 0x000074DF
		public bool IsMethod { get; private set; }

		/// <summary>
		/// 是否需要缓存数据
		/// </summary>
		/// <remarks>一般在变量标签出现的变量表达式的值都需要缓存.其它地方出现的则不需要缓存</remarks>
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000092E8 File Offset: 0x000074E8
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000092F0 File Offset: 0x000074F0
		private bool NeedCacheData { get; set; }

		/// <summary>
		/// 取得父级表达式
		/// </summary>
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000092F9 File Offset: 0x000074F9
		// (set) Token: 0x060001EF RID: 495 RVA: 0x00009301 File Offset: 0x00007501
		private VariableExpression ParentExpression { get; set; }

		/// <summary>
		/// 取得下一个表达式
		/// </summary>
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000930A File Offset: 0x0000750A
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00009312 File Offset: 0x00007512
		private VariableExpression NextExpression { get; set; }

		/// <summary>
		/// 取得此变量字段的值
		/// </summary>
		/// <returns></returns>
		// Token: 0x060001F2 RID: 498 RVA: 0x0000931B File Offset: 0x0000751B
		public object GetValue()
		{
			return this.GetValue(this.VariableId.Value);
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		// Token: 0x060001F3 RID: 499 RVA: 0x00009330 File Offset: 0x00007530
		private object GetValue(object data)
		{
			if (Utility.IsNothing(data) && this.VariableId.Variable.GetCacheCount() == 0)
			{
				return data;
			}
			object obj = data;
			if (!string.IsNullOrEmpty(this.FieldName))
			{
				List<string> list = new List<string>();
				for (VariableExpression variableExpression = this; variableExpression != null; variableExpression = variableExpression.ParentExpression)
				{
					list.Add(variableExpression.IsMethod ? (variableExpression.FieldName + "()") : variableExpression.FieldName);
				}
				list.Reverse();
				string exp = string.Join(".", list.ToArray());
				bool flag;
				obj = this.VariableId.Variable.GetExpValue(exp, out flag);
				if (!flag && !Utility.IsNothing(data))
				{
					if (this.IsMethod)
					{
						obj = Utility.GetMethodResult(data, this.FieldName, out flag);
					}
					else
					{
						obj = Utility.GetPropertyValue(data, this.FieldName, out flag);
					}
					if (this.NeedCacheData)
					{
						this.VariableId.Variable.AddExpValue(exp, obj);
					}
				}
			}
			if (this.NextExpression != null)
			{
				return this.NextExpression.GetValue(obj);
			}
			return obj;
		}

		/// <summary>
		/// 输出表达式的原字符串数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x060001F4 RID: 500 RVA: 0x00009438 File Offset: 0x00007638
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.VariableId.ToString());
			for (VariableExpression variableExpression = this; variableExpression != null; variableExpression = variableExpression.NextExpression)
			{
				if (!string.IsNullOrEmpty(variableExpression.FieldName))
				{
					stringBuilder.Append(".");
					stringBuilder.Append(variableExpression.FieldName);
					if (variableExpression.IsMethod)
					{
						stringBuilder.Append("()");
					}
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 克隆表达式
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060001F5 RID: 501 RVA: 0x000094AC File Offset: 0x000076AC
		public IExpression Clone(Template ownerTemplate)
		{
			VariableIdentity variableId = this.VariableId.Clone(ownerTemplate);
			VariableExpression variableExpression = new VariableExpression(variableId, this.FieldName, this.IsMethod, this.NeedCacheData);
			if (this.NextExpression != null)
			{
				variableExpression.NextExpression = (VariableExpression)this.NextExpression.Clone(ownerTemplate);
				variableExpression.NextExpression.ParentExpression = variableExpression;
			}
			return variableExpression;
		}
	}
}
