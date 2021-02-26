using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

namespace VTemplate.Engine
{
	/// <summary>
	/// 变量元素.如变量: {$:member.name} 或带前缀与属性的变量: {$:#.member.name htmlencode='true'}
	/// </summary>
	// Token: 0x02000013 RID: 19
	public class VariableTag : Element, IAttributesElement
	{
		/// <summary>
		/// 带变量字段的初始化
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="varExp"></param>
		// Token: 0x060000C6 RID: 198 RVA: 0x00004650 File Offset: 0x00002850
		internal VariableTag(Template ownerTemplate, VariableExpression varExp) : base(ownerTemplate)
		{
			this.Attributes = new AttributeCollection(this);
			this.Attributes.Adding += this.OnAddingAttribute;
			this.VarExpression = varExp;
			this.CallFunctions = new List<IExpression>();
		}

		/// <summary>
		/// 此标签的属性集合
		/// </summary>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000468E File Offset: 0x0000288E
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00004696 File Offset: 0x00002896
		public AttributeCollection Attributes { get; protected set; }

		/// <summary>
		/// 变量元素中的变量表达式
		/// </summary>
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000469F File Offset: 0x0000289F
		// (set) Token: 0x060000CA RID: 202 RVA: 0x000046A7 File Offset: 0x000028A7
		public VariableExpression VarExpression { get; private set; }

		/// <summary>
		/// 是否需要对输出数据进行HTML数据格式化
		/// </summary>
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000CB RID: 203 RVA: 0x000046B0 File Offset: 0x000028B0
		public Attribute HtmlEncode
		{
			get
			{
				return this.Attributes["HtmlEncode"];
			}
		}

		/// <summary>
		/// 是否需要对输出数据进行JS脚本格式化
		/// </summary>
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000046C2 File Offset: 0x000028C2
		public Attribute JsEncode
		{
			get
			{
				return this.Attributes["JsEncode"];
			}
		}

		/// <summary>
		/// 是否需要对输出数据进行XML数据格式化
		/// </summary>
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000046D4 File Offset: 0x000028D4
		public Attribute XmlEncode
		{
			get
			{
				return this.Attributes["XmlEncode"];
			}
		}

		/// <summary>
		/// 是否需要对输出数据进行URL地址编码
		/// </summary>
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000046E6 File Offset: 0x000028E6
		public Attribute UrlEncode
		{
			get
			{
				return this.Attributes["UrlEncode"];
			}
		}

		/// <summary>
		/// 是否需要对输出数据进行文本数据格式化(先HtmlEncode格式化,再将"空格"转换为"&amp;nbsp;", "\n"转换为"&lt;br /&gt;")
		/// </summary>
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000046F8 File Offset: 0x000028F8
		public Attribute TextEncode
		{
			get
			{
				return this.Attributes["TextEncode"];
			}
		}

		/// <summary>
		/// 是否需要对输出数据进行文本压缩(删除换行符和换行符前后的空格)
		/// </summary>
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000470A File Offset: 0x0000290A
		public Attribute CompressText
		{
			get
			{
				return this.Attributes["CompressText"];
			}
		}

		/// <summary>
		/// 数据的输出长度
		/// </summary>
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000471C File Offset: 0x0000291C
		public Attribute Length
		{
			get
			{
				return this.Attributes["Length"];
			}
		}

		/// <summary>
		/// 附加文本(此属性只能配合Length属性使用.即当文本有被裁剪时才附加此文本)
		/// </summary>
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000472E File Offset: 0x0000292E
		public Attribute AppendText
		{
			get
			{
				return this.Attributes["AppendText"];
			}
		}

		/// <summary>
		/// 数据的编码
		/// </summary>
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004740 File Offset: 0x00002940
		public Attribute Charset
		{
			get
			{
				return this.Attributes["Charset"];
			}
		}

		/// <summary>
		/// 数据输出时的格式化表达式
		/// </summary>
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004752 File Offset: 0x00002952
		public Attribute Format
		{
			get
			{
				return this.Attributes["Format"];
			}
		}

		/// <summary>
		/// 数据输出时是否删除HTML代码
		/// </summary>
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004764 File Offset: 0x00002964
		public Attribute RemoveHtml
		{
			get
			{
				return this.Attributes["RemoveHtml"];
			}
		}

		/// <summary>
		/// 要调用的函数列表
		/// </summary>
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004776 File Offset: 0x00002976
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x0000477E File Offset: 0x0000297E
		public List<IExpression> CallFunctions { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发事件函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		// Token: 0x060000D8 RID: 216 RVA: 0x00004788 File Offset: 0x00002988
		private void OnAddingAttribute(object sender, AttributeCollection.AttributeAddingEventArgs e)
		{
			string a;
			if ((a = e.Item.Name.ToLower()) != null)
			{
				if (!(a == "call"))
				{
					return;
				}
				this.CallFunctions.Add(e.Item.Value);
			}
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x060000D9 RID: 217 RVA: 0x000047D0 File Offset: 0x000029D0
		public override void Render(TextWriter writer)
		{
			object obj = this.VarExpression.GetValue();
			if (this.CallFunctions.Count > 0)
			{
				foreach (IExpression expression in this.CallFunctions)
				{
					object value = expression.GetValue();
					UserDefinedFunction userDefinedFunction;
					if (!Utility.IsNothing(value) && base.OwnerTemplate.UserDefinedFunctions.TryGetValue(value.ToString(), out userDefinedFunction))
					{
						obj = userDefinedFunction(new object[]
						{
							obj
						});
					}
				}
			}
			if (Utility.IsNothing(obj))
			{
				return;
			}
			bool flag = false;
			string text = string.Empty;
			string text2 = (this.Format == null) ? string.Empty : this.Format.GetTextValue();
			if (obj is string)
			{
				text = (string)obj;
			}
			else
			{
				if (!string.IsNullOrEmpty(text2))
				{
					IFormattable formattable = obj as IFormattable;
					if (formattable != null)
					{
						text = formattable.ToString(text2, CultureInfo.InvariantCulture);
						flag = true;
					}
				}
				if (!flag)
				{
					IEnumerator enumerator2 = null;
					if (obj is IEnumerable)
					{
						enumerator2 = ((IEnumerable)obj).GetEnumerator();
					}
					else if (obj is IEnumerator)
					{
						enumerator2 = (IEnumerator)obj;
					}
					if (enumerator2 != null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						enumerator2.Reset();
						while (enumerator2.MoveNext())
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(",");
							}
							stringBuilder.Append(enumerator2.Current);
						}
						text = stringBuilder.ToString();
					}
					else
					{
						text = obj.ToString();
					}
				}
			}
			if (text.Length > 0)
			{
				bool flag2 = this.RemoveHtml != null && Utility.ConverToBoolean(this.RemoveHtml.GetTextValue());
				bool flag3 = this.TextEncode != null && Utility.ConverToBoolean(this.TextEncode.GetTextValue());
				bool flag4 = this.HtmlEncode != null && Utility.ConverToBoolean(this.HtmlEncode.GetTextValue());
				bool flag5 = this.XmlEncode != null && Utility.ConverToBoolean(this.XmlEncode.GetTextValue());
				bool flag6 = this.JsEncode != null && Utility.ConverToBoolean(this.JsEncode.GetTextValue());
				bool flag7 = this.UrlEncode != null && Utility.ConverToBoolean(this.UrlEncode.GetTextValue());
				bool flag8 = this.CompressText != null && Utility.ConverToBoolean(this.CompressText.GetTextValue());
				int num = (this.Length == null) ? 0 : Utility.ConverToInt32(this.Length.GetTextValue());
				string appendText = (this.AppendText == null) ? string.Empty : this.AppendText.GetTextValue();
				Encoding encoding = (this.Charset == null) ? base.OwnerTemplate.Charset : Utility.GetEncodingFromCharset(this.Charset.GetTextValue(), base.OwnerTemplate.Charset);
				if (flag2)
				{
					text = Utility.RemoveHtmlCode(text);
				}
				if (num > 0)
				{
					text = Utility.CutString(text, num, encoding, appendText);
				}
				if (flag3)
				{
					text = Utility.TextEncode(text);
				}
				else if (flag4)
				{
					text = HttpUtility.HtmlEncode(text);
				}
				if (flag5)
				{
					text = Utility.XmlEncode(text);
				}
				if (flag6)
				{
					text = Utility.JsEncode(text);
				}
				if (flag7)
				{
					text = HttpUtility.UrlEncode(text, encoding);
				}
				if (flag8)
				{
					text = Utility.CompressText(text);
				}
				if (!flag && !string.IsNullOrEmpty(text2))
				{
					text = string.Format(text2, text);
				}
				writer.Write(text);
			}
		}

		/// <summary>
		/// 输出变量元素的原字符串数据
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000DA RID: 218 RVA: 0x00004B3C File Offset: 0x00002D3C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{$:");
			stringBuilder.Append(this.VarExpression.ToString());
			foreach (Attribute attribute in this.Attributes)
			{
				stringBuilder.AppendFormat(" {0}=\"{1}\"", attribute.Name, HttpUtility.HtmlEncode(attribute.Text));
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060000DB RID: 219 RVA: 0x00004BD8 File Offset: 0x00002DD8
		internal override Element Clone(Template ownerTemplate)
		{
			VariableTag variableTag = new VariableTag(ownerTemplate, (VariableExpression)this.VarExpression.Clone(ownerTemplate));
			foreach (Attribute attribute in this.Attributes)
			{
				variableTag.Attributes.Add(attribute.Clone(ownerTemplate));
			}
			foreach (IExpression expression in this.CallFunctions)
			{
				variableTag.CallFunctions.Add(expression.Clone(ownerTemplate));
			}
			return variableTag;
		}
	}
}
