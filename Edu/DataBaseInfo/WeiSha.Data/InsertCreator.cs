using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public class InsertCreator : TableCreator<InsertCreator>, IInsertCreator, ITableCreator<InsertCreator>
	{
		private Field identityField;
		private string sequenceName;
		private List<FieldValue> fvlist;
		internal Field IdentityField
		{
			get
			{
				return this.identityField;
			}
		}
		internal string SequenceName
		{
			get
			{
				return this.sequenceName;
			}
		}
		internal List<FieldValue> FieldValues
		{
			get
			{
				return this.fvlist;
			}
		}
		public static InsertCreator NewCreator()
		{
			return new InsertCreator();
		}
		public static InsertCreator NewCreator(string tableName)
		{
			return new InsertCreator(tableName);
		}
		public static InsertCreator NewCreator(Table table)
		{
			return new InsertCreator(table);
		}
		private InsertCreator()
		{
			this.fvlist = new List<FieldValue>();
		}
		private InsertCreator(string tableName) : base(tableName, null)
		{
			this.fvlist = new List<FieldValue>();
		}
		private InsertCreator(Table table) : base(table)
		{
			this.fvlist = new List<FieldValue>();
		}
		public InsertCreator SetEntity<T>(T entity) where T : Entity
		{
			this.fvlist = entity.GetFieldValues();
			this.fvlist.RemoveAll((FieldValue fv) => fv.IsChanged);
			this.sequenceName = entity.SequenceName;
			this.fvlist.ForEach(delegate(FieldValue fv)
			{
				if (fv.IsIdentity)
				{
					this.identityField = fv.Field;
				}
			});
			return base.From(Table.GetTable<T>());
		}
		public InsertCreator SetIdentityField(Field field)
		{
			this.identityField = field;
			return this;
		}
		public InsertCreator SetIdentityField(string fieldName)
		{
			return this.SetIdentityField(new Field(fieldName));
		}
		public InsertCreator AddInsert(Field field, object value)
		{
			if (!this.fvlist.Exists((FieldValue fv) => fv.Field.OriginalName == field.OriginalName))
			{
				FieldValue item = new FieldValue(field, value);
				this.fvlist.Add(item);
			}
			return this;
		}
		public InsertCreator AddInsert(string fieldName, object value)
		{
			return this.AddInsert(new Field(fieldName), value);
		}
		public InsertCreator AddInsert(IDictionary<string, object> dict)
		{
			string[] fieldNames = new List<string>(dict.Keys).ToArray();
			object[] values = new List<object>(dict.Values).ToArray();
			return this.AddInsert(fieldNames, values);
		}
		public InsertCreator AddInsert(IDictionary<Field, object> dict)
		{
			Field[] fields = new List<Field>(dict.Keys).ToArray();
			object[] values = new List<object>(dict.Values).ToArray();
			return this.AddInsert(fields, values);
		}
		public InsertCreator AddInsert(Field[] fields, object[] values)
		{
			if (fields == null || values == null)
			{
				throw new DataException("字段和值不能为null;");
			}
			if (fields.Length != values.Length)
			{
				throw new DataException("字段和值的数量必须一致;");
			}
			int num = 0;
			for (int i = 0; i < fields.Length; i++)
			{
				Field field = fields[i];
				this.AddInsert(field, values[num]);
				num++;
			}
			return this;
		}
		public InsertCreator AddInsert(string[] fieldNames, object[] values)
		{
			if (fieldNames == null || values == null)
			{
				throw new DataException("字段和值不能为null;");
			}
			if (fieldNames.Length != values.Length)
			{
				throw new DataException("字段和值的数量必须一致;");
			}
			int num = 0;
			for (int i = 0; i < fieldNames.Length; i++)
			{
				string fieldName = fieldNames[i];
				this.AddInsert(fieldName, values[num]);
				num++;
			}
			return this;
		}
		public InsertCreator RemoveInsert(params string[] fieldNames)
		{
			if (fieldNames == null)
			{
				return this;
			}
			List<Field> list = new List<Field>();
			for (int i = 0; i < fieldNames.Length; i++)
			{
				string fieldName = fieldNames[i];
				list.Add(new Field(fieldName));
			}
			return this.RemoveInsert(list.ToArray());
		}
		public InsertCreator RemoveInsert(params Field[] fields)
		{
			if (fields == null)
			{
				return this;
			}
			Field field;
			for (int i = 0; i < fields.Length; i++)
			{
				field = fields[i];
				if (this.fvlist.RemoveAll((FieldValue fv) => string.Compare(fv.Field.OriginalName, field.OriginalName, true) == 0) == 0)
				{
					throw new DataException("指定的字段不存在于Insert列表中！");
				}
			}
			return this;
		}
	}
}
