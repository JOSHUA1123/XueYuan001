using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public class UpdateCreator : WhereCreator<UpdateCreator>, IUpdateCreator, IWhereCreator<UpdateCreator>, ITableCreator<UpdateCreator>
	{
		private List<FieldValue> fvlist;
		internal Field[] Fields
		{
			get
			{
				return this.fvlist.ConvertAll<Field>((FieldValue fv) => fv.Field).ToArray();
			}
		}
		internal object[] Values
		{
			get
			{
				return this.fvlist.ConvertAll<object>((FieldValue fv) => fv.Value).ToArray();
			}
		}
		public static UpdateCreator NewCreator()
		{
			return new UpdateCreator();
		}
		public static UpdateCreator NewCreator(string tableName)
		{
			return new UpdateCreator(tableName);
		}
		public static UpdateCreator NewCreator(Table table)
		{
			return new UpdateCreator(table);
		}
		private UpdateCreator()
		{
			this.fvlist = new List<FieldValue>();
		}
		private UpdateCreator(string tableName) : base(tableName, null)
		{
			this.fvlist = new List<FieldValue>();
		}
		private UpdateCreator(Table table) : base(table)
		{
			this.fvlist = new List<FieldValue>();
		}
		public UpdateCreator SetEntity<T>(T entity, bool useKeyWhere) where T : Entity
		{
			this.fvlist = entity.GetFieldValues();
			this.fvlist.RemoveAll((FieldValue fv) => !fv.IsChanged || fv.IsIdentity || fv.IsPrimaryKey);
			if (useKeyWhere)
			{
				WhereClip pkWhere = DataHelper.GetPkWhere<T>(entity.GetTable(), entity);
				return base.From(Table.GetTable<T>()).AddWhere(pkWhere);
			}
			return base.From(Table.GetTable<T>());
		}
		public UpdateCreator AddUpdate(Field field, object value)
		{
			if (!this.fvlist.Exists((FieldValue fv) => fv.Field.OriginalName == field.OriginalName))
			{
				FieldValue item = new FieldValue(field, value);
				this.fvlist.Add(item);
			}
			return this;
		}
		public UpdateCreator AddUpdate(string fieldName, object value)
		{
			return this.AddUpdate(new Field(fieldName), value);
		}
		public UpdateCreator AddUpdate(IDictionary<string, object> dict)
		{
			string[] fieldNames = new List<string>(dict.Keys).ToArray();
			object[] values = new List<object>(dict.Values).ToArray();
			return this.AddUpdate(fieldNames, values);
		}
		public UpdateCreator AddUpdate(IDictionary<Field, object> dict)
		{
			Field[] fields = new List<Field>(dict.Keys).ToArray();
			object[] values = new List<object>(dict.Values).ToArray();
			return this.AddUpdate(fields, values);
		}
		public UpdateCreator AddUpdate(Field[] fields, object[] values)
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
				this.AddUpdate(field, values[num]);
				num++;
			}
			return this;
		}
		public UpdateCreator AddUpdate(string[] fieldNames, object[] values)
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
				this.AddUpdate(fieldName, values[num]);
				num++;
			}
			return this;
		}
		public UpdateCreator RemoveUpdate(params string[] fieldNames)
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
			return this.RemoveUpdate(list.ToArray());
		}
		public UpdateCreator RemoveUpdate(params Field[] fields)
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
					throw new DataException("指定的字段不存在于Update列表中！");
				}
			}
			return this;
		}
	}
}
