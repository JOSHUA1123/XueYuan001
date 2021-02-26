using System;
using System.Text;

namespace VTemplate.Engine
{
	/// <summary>
	/// 变量标识:变量标识由两部分组成,变量前缀(以#号开头)与变量名.如"#.member"或"#user.member".其中变量前缀是可以省略.如"member"
	/// </summary>
	// Token: 0x0200002E RID: 46
	public class VariableIdentity : ICloneableElement<VariableIdentity>
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="variable"></param>
		/// <param name="prefix"></param>
		// Token: 0x06000247 RID: 583 RVA: 0x0000A7D9 File Offset: 0x000089D9
		public VariableIdentity(Template ownerTemplate, Variable variable, string prefix)
		{
			this.OwnerTemplate = ownerTemplate;
			this.Variable = variable;
			this.Prefix = prefix;
		}

		/// <summary>
		/// 宿主模板
		/// </summary>
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000A7F6 File Offset: 0x000089F6
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000A7FE File Offset: 0x000089FE
		public Template OwnerTemplate { get; private set; }

		/// <summary>
		/// 变量
		/// </summary>
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000A807 File Offset: 0x00008A07
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000A80F File Offset: 0x00008A0F
		public Variable Variable { get; private set; }

		/// <summary>
		/// 变量前缀.
		/// </summary>
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000A818 File Offset: 0x00008A18
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000A820 File Offset: 0x00008A20
		public string Prefix { get; private set; }

		/// <summary>
		/// 设置或返回变量的值
		/// </summary>
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000A829 File Offset: 0x00008A29
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000A836 File Offset: 0x00008A36
		public object Value
		{
			get
			{
				return this.Variable.Value;
			}
			set
			{
				this.Variable.Value = value;
			}
		}

		/// <summary>
		/// 设置变量中某种表达式所表示的值
		/// </summary>
		/// <param name="exp">变量表达式.如"age"则表示此变量下的age属性/字段值,"age.tostring()"则表示此变量下的age属性/字段的值的tostring方法所返回的值</param>
		/// <param name="value"></param>
		// Token: 0x06000250 RID: 592 RVA: 0x0000A844 File Offset: 0x00008A44
		public void SetExpValue(string exp, object value)
		{
			this.Variable.SetExpValue(exp, value);
		}

		/// <summary>
		/// 重设(清空)当前变量中已缓存的表达式值
		/// </summary>
		// Token: 0x06000251 RID: 593 RVA: 0x0000A853 File Offset: 0x00008A53
		public void Reset()
		{
			this.Variable.Reset();
		}

		/// <summary>
		/// 克隆当前变量对象到新的宿主模板
		/// </summary>
		/// <param name="template"></param>
		/// <returns></returns>
		// Token: 0x06000252 RID: 594 RVA: 0x0000A860 File Offset: 0x00008A60
		public VariableIdentity Clone(Template template)
		{
			Template ownerTemplateByPrefix = Utility.GetOwnerTemplateByPrefix(template, this.Prefix);
			Variable variableOrAddNew = Utility.GetVariableOrAddNew(ownerTemplateByPrefix, this.Variable.Name);
			return new VariableIdentity(template, variableOrAddNew, this.Prefix);
		}

		/// <summary>
		/// 输出变量标识的原字符串数据,如#.member
		/// </summary>
		/// <returns></returns>
		// Token: 0x06000253 RID: 595 RVA: 0x0000A89C File Offset: 0x00008A9C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Prefix != null)
			{
				stringBuilder.AppendFormat("#{0}.", this.Prefix);
			}
			stringBuilder.Append(this.Variable.Name);
			return stringBuilder.ToString();
		}
	}
}
