using System;
using System.Collections.Generic;
using System.Text;
using DataBaseInfo.Design;
namespace DataBaseInfo
{
	[Serializable]
	public class Field : IField
	{
		public static readonly AllField All = new AllField();
		protected string propertyName;
		protected string tableName;
		protected string fieldName;
		protected string aliasName;
		internal string FullName
		{
			get
			{
				if (this.aliasName == null)
				{
					return this.Name;
				}
				return string.Format("{0} as __[{1}]__", this.Name, this.aliasName);
			}
		}
		internal virtual string Name
		{
			get
			{
				if (this.tableName == null)
				{
					return this.FieldName;
				}
				return this.TableName + "." + this.FieldName;
			}
		}
		public string OriginalName
		{
			get
			{
				if (this.aliasName != null)
				{
					return this.aliasName;
				}
				if (this.fieldName.Contains("__$") || this.fieldName.Contains("$__"))
				{
					return this.fieldName.Replace("__$", "").Replace("$__", "");
				}
				return this.fieldName;
			}
		}
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}
		private string TableName
		{
			get
			{
				if (this.tableName == null || this.tableName.Contains("__[") || this.tableName.Contains("]__"))
				{
					return this.tableName;
				}
				return "__[" + this.tableName + "]__";
			}
		}
		private string FieldName
		{
			get
			{
				if (this.fieldName.Contains("__$") || this.fieldName.Contains("$__"))
				{
					return this.fieldName.Replace("__$", "").Replace("$__", "");
				}
				if (this.fieldName == "*" || this.fieldName.Contains("'") || this.fieldName.Contains("(") || this.fieldName.Contains(")") || this.fieldName.Contains("__[") || this.fieldName.Contains("]__"))
				{
					return this.fieldName;
				}
				return "__[" + this.fieldName + "]__";
			}
		}
		public OrderByClip Asc
		{
			get
			{
				return new OrderByClip(this.Name + " asc ");
			}
		}
		public OrderByClip Desc
		{
			get
			{
				return new OrderByClip(this.Name + " desc ");
			}
		}
		public GroupByClip Group
		{
			get
			{
				return new GroupByClip(this.Name);
			}
		}
		public FieldValue Set(object value)
		{
			return new FieldValue(this, value);
		}
		public FieldValue Set(DbValue value)
		{
			return new FieldValue(this, value);
		}
		public FieldValue Set(Field field)
		{
			return new FieldValue(this, field);
		}
		public static Field Create(string fieldName, QueryCreator creator)
		{
			return new CustomField(fieldName, creator);
		}
		public static Field Create<T>(string fieldName, QuerySection<T> query) where T : Entity
		{
			return new CustomField<T>(fieldName, query);
		}
		public static Field Create<T>(string fieldName, TableRelation<T> relation) where T : Entity
		{
			return new CustomField<T>(fieldName, relation);
		}
		public Field(string fieldName)
		{
			this.fieldName = fieldName;
			this.propertyName = this.OriginalName;
		}
		internal Field(string tableName, string fieldName) : this(fieldName)
		{
			this.tableName = (string.IsNullOrEmpty(tableName) ? null : tableName);
		}
		internal Field(string propertyName, string tableName, string fieldName, string aliasName) : this(tableName, fieldName)
		{
			this.propertyName = propertyName;
			this.aliasName = (string.IsNullOrEmpty(aliasName) ? null : aliasName);
		}
		public override bool Equals(object obj)
		{
			return obj != null && this.FieldName == (obj as Field).FieldName;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public static WhereClip operator ==(Field leftField, Field rightField)
		{
            //if (leftField.Name == "")
            //{
            //    return null;
            //}
            //if (rightField.Name == "")
            //{
            //    return null;
            //}
            //return new WhereClip(leftField.Name + " = " + rightField.Name, new SQLParameter[0]);
            return new WhereClip("", new SQLParameter[0]);
        }
		public static WhereClip operator !=(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
            //return new WhereClip(leftField.Name + " <> " + rightField.Name, new SQLParameter[0]);
            return new WhereClip("", new SQLParameter[0]);
        }
		public static WhereClip operator >(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new WhereClip(leftField.Name + " > " + rightField.Name, new SQLParameter[0]);
		}
		public static WhereClip operator >=(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new WhereClip(leftField.Name + " >= " + rightField.Name, new SQLParameter[0]);
		}
		public static WhereClip operator <(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new WhereClip(leftField.Name + " < " + rightField.Name, new SQLParameter[0]);
		}
		public static WhereClip operator <=(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new WhereClip(leftField.Name + " <= " + rightField.Name, new SQLParameter[0]);
		}
		public static WhereClip operator ==(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return Field.CreateWhereClip(field, "=", value);
		}
		public static WhereClip operator !=(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return Field.CreateWhereClip(field, "<>", value);
		}
		public static WhereClip operator >(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return Field.CreateWhereClip(field, ">", value);
		}
		public static WhereClip operator >=(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return Field.CreateWhereClip(field, ">=", value);
		}
		public static WhereClip operator <(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return Field.CreateWhereClip(field, "<", value);
		}
		public static WhereClip operator <=(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return Field.CreateWhereClip(field, "<=", value);
		}
		public static Field operator +(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new Field(string.Concat(new string[]
			{
				"(",
				leftField.Name,
				" + ",
				rightField.Name,
				")"
			})).As(leftField.OriginalName);
		}
		public static Field operator -(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new Field(string.Concat(new string[]
			{
				"(",
				leftField.Name,
				" - ",
				rightField.Name,
				")"
			})).As(leftField.OriginalName);
		}
		public static Field operator *(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new Field(string.Concat(new string[]
			{
				"(",
				leftField.Name,
				" * ",
				rightField.Name,
				")"
			})).As(leftField.OriginalName);
		}
		public static Field operator /(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new Field(string.Concat(new string[]
			{
				"(",
				leftField.Name,
				" / ",
				rightField.Name,
				")"
			})).As(leftField.OriginalName);
		}
		public static Field operator %(Field leftField, Field rightField)
		{
			if (leftField == null)
			{
				return null;
			}
			if (rightField == null)
			{
				return null;
			}
			return new Field(string.Concat(new string[]
			{
				"(",
				leftField.Name,
				" % ",
				rightField.Name,
				")"
			})).As(leftField.OriginalName);
		}
		public static Field operator +(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(field.Name + " + " + DataHelper.FormatValue(value)).As(field.OriginalName);
		}
		public static Field operator -(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(field.Name + " - " + DataHelper.FormatValue(value)).As(field.OriginalName);
		}
		public static Field operator *(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(field.Name + " * " + DataHelper.FormatValue(value)).As(field.OriginalName);
		}
		public static Field operator /(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(field.Name + " / " + DataHelper.FormatValue(value)).As(field.OriginalName);
		}
		public static Field operator %(Field field, object value)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(field.Name + " % " + DataHelper.FormatValue(value)).As(field.OriginalName);
		}
		public static Field operator +(object value, Field field)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(DataHelper.FormatValue(value) + " + " + field.Name).As(field.OriginalName);
		}
		public static Field operator -(object value, Field field)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(DataHelper.FormatValue(value) + " - " + field.Name).As(field.OriginalName);
		}
		public static Field operator *(object value, Field field)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(DataHelper.FormatValue(value) + " * " + field.Name).As(field.OriginalName);
		}
		public static Field operator /(object value, Field field)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(DataHelper.FormatValue(value) + " / " + field.Name).As(field.OriginalName);
		}
		public static Field operator %(object value, Field field)
		{
			if (field == null)
			{
				return null;
			}
			return new Field(DataHelper.FormatValue(value) + " % " + field.Name).As(field.OriginalName);
		}
		public Field Distinct()
		{
			return new Field("distinct(" + this.Name + ")");
		}
		public Field Count()
		{
			return new Field("count(" + this.Name + ")").As(this.OriginalName);
		}
		public Field Sum()
		{
			return new Field("sum(" + this.Name + ")").As(this.OriginalName);
		}
		public Field Avg()
		{
			return new Field("avg(" + this.Name + ")").As(this.OriginalName);
		}
		public Field Max()
		{
			return new Field("max(" + this.Name + ")").As(this.OriginalName);
		}
		public Field Min()
		{
			return new Field("min(" + this.Name + ")").As(this.OriginalName);
		}
		public Field As(string aliasName)
		{
			return new Field(this.propertyName, this.tableName, this.fieldName, aliasName);
		}
		public Field At(string tableName)
		{
			if (this.fieldName.Contains("(") || this.fieldName.Contains(")"))
			{
				return this;
			}
			return new Field(this.propertyName, tableName, this.fieldName, this.aliasName);
		}
		public Field At(Table table)
		{
			if (table == null)
			{
				return this;
			}
			if (table.Alias != null)
			{
				return this.At(table.Alias);
			}
			return this.At(table.Name);
		}
		public static Field Func(string function, params Field[] fields)
		{
			if (fields != null && fields.Length > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < fields.Length; i++)
				{
					Field field = fields[i];
					list.Add(field.Name);
				}
				return new Field(string.Format(function, list.ToArray()));
			}
			return new Field(function);
		}
		public WhereClip IsNull()
		{
			return this == null;
		}
		public WhereClip Contains(string value)
		{
			return this.Like("%" + value + "%");
		}
		public WhereClip Like(string value)
		{
			return Field.CreateWhereClip(this, "like", value);
		}
		public WhereClip StartsWith(string value)
		{
			return this.Like(value + "%");
		}
		public WhereClip EndsWith(string value)
		{
			return this.Like("%" + value);
		}
		public WhereClip Between(object leftValue, object rightValue)
		{
			string text = CoreHelper.MakeUniqueKey(100, "$");
			SQLParameter sQLParameter = new SQLParameter(text);
			sQLParameter.Value = leftValue;
			string text2 = CoreHelper.MakeUniqueKey(100, "$");
			SQLParameter sQLParameter2 = new SQLParameter(text2);
			sQLParameter2.Value = rightValue;
			string where = string.Format("{0} between {1} and {2}", this.Name, text, text2);
			return new WhereClip(where, new SQLParameter[]
			{
				sQLParameter,
				sQLParameter2
			});
		}
		public WhereClip In<T>(Field field) where T : Entity
		{
			return this.In<T>(null, field);
		}
		public WhereClip In<T>(Table table, Field field) where T : Entity
		{
			return this.In<T>(table, field, WhereClip.None);
		}
		public WhereClip In<T>(Field field, WhereClip where) where T : Entity
		{
			return this.In<T>(null, field, where);
		}
		public WhereClip In<T>(Table table, Field field, WhereClip where) where T : Entity
		{
			return this.In<T>(new FromSection<T>(table, null).Select(new Field[]
			{
				field
			}).Where(where));
		}
		public WhereClip In<T>(QuerySection<T> query) where T : Entity
		{
			return new WhereClip(this.Name + " in (" + query.QueryString + ") ", query.Parameters);
		}
		public WhereClip In<T>(TableRelation<T> relation) where T : Entity
		{
			QuerySection<T> query = relation.GetFromSection().Query;
			return new WhereClip(this.Name + " in (" + query.QueryString + ") ", query.Parameters);
		}
		public WhereClip In(params object[] values)
		{
			values = DataHelper.CheckAndReturnValues(values);
			if (values.Length == 1)
			{
				return this == values[0];
			}
			List<SQLParameter> list = new List<SQLParameter>();
			StringBuilder stringBuilder = new StringBuilder();
			object[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				object value = array[i];
				string text = CoreHelper.MakeUniqueKey(100, "$");
				SQLParameter sQLParameter = new SQLParameter(text);
				sQLParameter.Value = value;
				stringBuilder.Append(text);
				stringBuilder.Append(",");
				list.Add(sQLParameter);
			}
			string str = stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString().Trim();
			return new WhereClip(this.Name + " in (" + str + ") ", list.ToArray());
		}
		public WhereClip In(QueryCreator creator)
		{
			QuerySection<ViewEntity> query = this.GetQuery(creator);
			return this.In<ViewEntity>(query);
		}
		internal QuerySection<ViewEntity> GetQuery(QueryCreator creator)
		{
			if (creator.Table == null)
			{
				throw new DataException("用创建器操作时，表不能为null！");
			}
			FromSection<ViewEntity> fromSection = new FromSection<ViewEntity>(creator.Table, null);
			if (creator.IsRelation)
			{
				foreach (TableJoin current in creator.Relations.Values)
				{
					if (current.Type == JoinType.LeftJoin)
					{
						fromSection.LeftJoin<ViewEntity>(current.Table, current.Where);
					}
					else
					{
						if (current.Type == JoinType.RightJoin)
						{
							fromSection.RightJoin<ViewEntity>(current.Table, current.Where);
						}
						else
						{
							fromSection.InnerJoin<ViewEntity>(current.Table, current.Where);
						}
					}
				}
			}
			return fromSection.Select(creator.Fields).Where(creator.Where).OrderBy(creator.OrderBy);
		}
		private static WhereClip CreateWhereClip(Field field, string join, object value)
		{
			if (value != null)
			{
				string text = CoreHelper.MakeUniqueKey(100, "$p_");
				SQLParameter sQLParameter = new SQLParameter(text);
				sQLParameter.Value = value;
				string where = string.Format("{0} {1} {2}", field.Name, join, text);
				return new WhereClip(where, new SQLParameter[]
				{
					sQLParameter
				});
			}
			if (join == "=")
			{
				return new WhereClip(field.Name + " is null", new SQLParameter[0]);
			}
			if (join == "<>")
			{
				return new WhereClip(field.Name + " is not null", new SQLParameter[0]);
			}
			throw new DataException("当值为null时只能应用于=与<>操作！");
		}
	}
	[Serializable]
	public class Field<T> : Field where T : Entity
	{
		public Field(string fieldName) : this(fieldName, fieldName)
		{
		}
		public Field(string propertyName, string fieldName) : base(propertyName, null, fieldName, null)
		{
			this.tableName = Table.GetTable<T>().OriginalName;
			Field mappingField = EntityConfig.Instance.GetMappingField<T>(propertyName, fieldName);
			this.fieldName = mappingField.OriginalName;
		}
	}
}
