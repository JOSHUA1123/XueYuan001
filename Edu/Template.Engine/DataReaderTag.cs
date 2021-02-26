using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;

namespace VTemplate.Engine
{
	/// <summary>
	/// DataReader标签.如:&lt;vt:datareader var="members" connection="sitedb"  commandtext="select * from [member]"&gt;...&lt;/vt:foreach&gt;
	/// </summary>
	// Token: 0x0200000F RID: 15
	public class DataReaderTag : Tag
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="ownerTemplate"></param>
		// Token: 0x060000A0 RID: 160 RVA: 0x00003F09 File Offset: 0x00002109
		internal DataReaderTag(Template ownerTemplate) : base(ownerTemplate)
		{
			this.Parameters = new ElementCollection<IExpression>();
		}

		/// <summary>
		/// 返回标签的名称
		/// </summary>
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00003F1D File Offset: 0x0000211D
		public override string TagName
		{
			get
			{
				return "datareader";
			}
		}

		/// <summary>
		/// 返回此标签是否是单一标签.即是不需要配对的结束标签
		/// </summary>
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003F24 File Offset: 0x00002124
		internal override bool IsSingleTag
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 数据源名称.此名称必须已在项目配置文件(如:web.config)里的connectionStrings节点里定义.
		/// </summary>
		/// <remarks></remarks>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00003F27 File Offset: 0x00002127
		public Attribute Connection
		{
			get
			{
				return base.Attributes["Connection"];
			}
		}

		/// <summary>
		/// 数据查询命令语句.
		/// </summary>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003F39 File Offset: 0x00002139
		public Attribute CommandText
		{
			get
			{
				return base.Attributes["CommandText"];
			}
		}

		/// <summary>
		/// 数据查询命令语句类型
		/// </summary>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003F4B File Offset: 0x0000214B
		public Attribute CommandType
		{
			get
			{
				return base.Attributes["CommandType"];
			}
		}

		/// <summary>
		/// 数据查询参数的格式.默认为"@p{0}",其中"{0}"是占位符,表示各参数的索引数字.
		/// </summary>
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00003F5D File Offset: 0x0000215D
		public Attribute ParameterFormat
		{
			get
			{
				return base.Attributes["ParameterFormat"];
			}
		}

		/// <summary>
		/// 要获取的行号.从0开始计算
		/// </summary>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00003F6F File Offset: 0x0000216F
		public Attribute RowIndex
		{
			get
			{
				return base.Attributes["RowIndex"];
			}
		}

		/// <summary>
		/// 存储表达式结果的变量
		/// </summary>
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003F81 File Offset: 0x00002181
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003F89 File Offset: 0x00002189
		public VariableIdentity Variable { get; protected set; }

		/// <summary>
		/// 查询命令中使用的变量参数列表,各参数在查询命令语句中则用参数名代替.如"@p0","@p1"之类的参数名
		/// </summary>
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003F92 File Offset: 0x00002192
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00003F9A File Offset: 0x0000219A
		public virtual ElementCollection<IExpression> Parameters { get; protected set; }

		/// <summary>
		/// 添加标签属性时的触发函数.用于设置自身的某些属性值
		/// </summary>
		/// <param name="name"></param>
		/// <param name="item"></param>
		// Token: 0x060000AC RID: 172 RVA: 0x00003FA4 File Offset: 0x000021A4
		protected override void OnAddingAttribute(string name, Attribute item)
		{
			if (name != null)
			{
				if (name == "var")
				{
					this.Variable = ParserHelper.CreateVariableIdentity(base.OwnerTemplate, item.Text);
					return;
				}
				if (!(name == "parameters"))
				{
					return;
				}
				IExpression expression = item.Value;
				if (this.OwnerDocument.DocumentConfig.CompatibleMode && !(expression is VariableExpression))
				{
					expression = ParserHelper.CreateVariableExpression(base.OwnerTemplate, item.Text, false);
				}
				this.Parameters.Add(expression);
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
		// Token: 0x060000AD RID: 173 RVA: 0x0000402C File Offset: 0x0000222C
		internal override bool ProcessBeginTag(Template ownerTemplate, Tag container, Stack<Tag> tagStack, string text, ref Match match, bool isClosedTag)
		{
			if (this.Variable == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少var属性", this.TagName));
			}
			if (this.Connection == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少connection属性", this.TagName));
			}
			if (this.CommandText == null)
			{
				throw new ParserException(string.Format("{0}标签中缺少commandtext属性", this.TagName));
			}
			return base.ProcessBeginTag(ownerTemplate, container, tagStack, text, ref match, isClosedTag);
		}

		/// <summary>
		/// 克隆当前元素到新的宿主模板
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <returns></returns>
		// Token: 0x060000AE RID: 174 RVA: 0x000040A4 File Offset: 0x000022A4
		internal override Element Clone(Template ownerTemplate)
		{
			DataReaderTag dataReaderTag = new DataReaderTag(ownerTemplate);
			this.CopyTo(dataReaderTag);
			dataReaderTag.Variable = ((this.Variable == null) ? null : this.Variable.Clone(ownerTemplate));
			foreach (IExpression expression in this.Parameters)
			{
				dataReaderTag.Parameters.Add(expression.Clone(ownerTemplate));
			}
			return dataReaderTag;
		}

		/// <summary>
		/// 呈现本元素的数据
		/// </summary>
		/// <param name="writer"></param>
		// Token: 0x060000AF RID: 175 RVA: 0x00004128 File Offset: 0x00002328
		protected override void RenderTagData(TextWriter writer)
		{
			this.Variable.Value = this.GetDataSource();
			base.RenderTagData(writer);
		}

		/// <summary>
		/// 获取数据源
		/// </summary>
		/// <returns></returns>
		// Token: 0x060000B0 RID: 176 RVA: 0x00004144 File Offset: 0x00002344
		protected virtual object GetDataSource()
		{
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[this.Connection.GetTextValue()];
			if (connectionStringSettings == null)
			{
				return null;
			}
			DbProviderFactory dbProviderFactory = Utility.CreateDbProviderFactory(connectionStringSettings.ProviderName);
			if (dbProviderFactory == null)
			{
				return null;
			}
			object result = null;
			using (DbConnection dbConnection = dbProviderFactory.CreateConnection())
			{
				dbConnection.ConnectionString = connectionStringSettings.ConnectionString;
				using (DbCommand dbCommand = dbConnection.CreateCommand())
				{
					dbCommand.CommandType = ((this.CommandType == null) ? System.Data.CommandType.Text : ((CommandType)Utility.ConvertTo(this.CommandType.GetTextValue(), typeof(CommandType))));
					dbCommand.CommandText = this.CommandText.GetTextValue();
					if (this.Parameters.Count > 0)
					{
						string text = (this.ParameterFormat == null) ? "@p{0}" : this.ParameterFormat.GetTextValue();
						new List<object>();
						for (int i = 0; i < this.Parameters.Count; i++)
						{
							IExpression expression = this.Parameters[i];
							DbParameter dbParameter = dbProviderFactory.CreateParameter();
							object value = expression.GetValue();
							dbParameter.ParameterName = (string.IsNullOrEmpty(text) ? "?" : string.Format(text, i));
							dbParameter.DbType = Utility.GetObjectDbType(value);
							dbParameter.Value = value;
							dbCommand.Parameters.Add(dbParameter);
						}
					}
					using (DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter())
					{
						dbDataAdapter.SelectCommand = dbCommand;
						DataTable dataTable = new DataTable();
						dbDataAdapter.Fill(dataTable);
						if (this.RowIndex != null)
						{
							int num = Utility.ConverToInt32(this.RowIndex.GetTextValue());
							if (dataTable.Rows.Count > num)
							{
								result = dataTable.Rows[num];
							}
						}
						else
						{
							result = dataTable;
						}
					}
				}
			}
			return result;
		}
	}
}
