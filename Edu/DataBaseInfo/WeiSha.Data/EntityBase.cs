using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
namespace DataBaseInfo
{
	[Serializable]
	public abstract class EntityBase : IEntityBase, IEntityInfo, IValidator
	{
		protected List<Field> updatelist = new List<Field>();
		protected List<Field> removeinsertlist = new List<Field>();
		protected bool isUpdate;
		protected bool isFromDB;
		internal string SequenceName
		{
			get
			{
				bool flag = false;
				string sequence;
				try
				{
					Monitor.Enter(this, ref flag);
					sequence = this.GetSequence();
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
				return sequence;
			}
		}
		internal Field PagingField
		{
			get
			{
				bool flag = false;
				Field result;
				try
				{
					Monitor.Enter(this, ref flag);
					Field field = this.GetIdentityField();
					if (field == null)
					{
						Field[] primaryKeyFields = this.GetPrimaryKeyFields();
						if (primaryKeyFields.Length > 0)
						{
							field = primaryKeyFields[0];
						}
					}
					result = field;
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
				return result;
			}
		}
		internal Field IdentityField
		{
			get
			{
				bool flag = false;
				Field identityField;
				try
				{
					Monitor.Enter(this, ref flag);
					identityField = this.GetIdentityField();
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
				return identityField;
			}
		}
		Table IEntityInfo.Table
		{
			get
			{
				return this.GetTable();
			}
		}
		Field[] IEntityInfo.Fields
		{
			get
			{
				return this.GetFields();
			}
		}
		FieldValue[] IEntityInfo.FieldValues
		{
			get
			{
				return this.GetFieldValues().ToArray();
			}
		}
		Field[] IEntityInfo.UpdateFields
		{
			get
			{
				return this.GetFieldValues().FindAll((FieldValue p) => p.IsChanged).ConvertAll<Field>((FieldValue p) => p.Field).ToArray();
			}
		}
		FieldValue[] IEntityInfo.UpdateFieldValues
		{
			get
			{
				return this.GetFieldValues().FindAll((FieldValue p) => p.IsChanged).ToArray();
			}
		}
		bool IEntityInfo.IsUpdate
		{
			get
			{
				return this.GetFieldValues().FindAll((FieldValue p) => p.IsChanged).Count > 0;
			}
		}
		bool IEntityInfo.IsReadOnly
		{
			get
			{
				return this.GetReadOnly();
			}
		}
		public TResult As<TResult>()
		{
			bool flag = false;
			TResult result;
			try
			{
				Monitor.Enter(this, ref flag);
				result = DataHelper.ConvertType<IEntityBase, TResult>(this);
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
		IRowReader IEntityBase.ToRowReader()
		{
			bool flag = false;
			IRowReader result;
			try
			{
				Monitor.Enter(this, ref flag);
				try
				{
					DataTable dataTable = new SourceList<EntityBase>
					{
						this
					}.GetDataTable(base.GetType());
					ISourceTable sourceTable = new SourceTable(dataTable);
					result = sourceTable[0];
				}
				catch (Exception ex)
				{
					throw new DataException("数据转换失败！", ex);
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
		IDictionary<string, object> IEntityBase.ToDictionary()
		{
			IDictionary<string, object> result;
			try
			{
				IDictionary<string, object> dictionary = new Dictionary<string, object>();
				Field[] fields = this.GetFields();
				for (int i = 0; i < fields.Length; i++)
				{
					Field field = fields[i];
					object propertyValue = CoreHelper.GetPropertyValue(this, field.PropertyName);
					dictionary[field.OriginalName] = propertyValue;
				}
				result = dictionary;
			}
			catch (Exception ex)
			{
				throw new DataException("数据转换失败！", ex);
			}
			return result;
		}
		object IEntityBase.GetValue(string propertyName)
		{
			return CoreHelper.GetPropertyValue(this, propertyName);
		}
		void IEntityBase.SetValue(string propertyName, object value)
		{
			CoreHelper.SetPropertyValue(this, propertyName, value);
		}
		object IEntityBase.GetValue(Field field)
		{
			return CoreHelper.GetPropertyValue(this, field.PropertyName);
		}
		void IEntityBase.SetValue(Field field, object value)
		{
			CoreHelper.SetPropertyValue(this, field.PropertyName, value);
		}
		Field IEntityBase.GetField(string propertyName)
		{
			Field field = this.GetFields().FirstOrDefault((Field p) => string.Compare(p.PropertyName, propertyName, true) == 0);
			if (field == null)
			{
				throw new DataException(string.Format("实体【{0}】中未找到属性为【{1}】的字段信息！", base.GetType().FullName, propertyName));
			}
			return field;
		}
		EntityState IEntityBase.GetObjectState()
		{
			if (!this.isUpdate)
			{
				return EntityState.Insert;
			}
			return EntityState.Update;
		}
		protected virtual string GetSequence()
		{
			return null;
		}
		protected virtual Field GetIdentityField()
		{
			return null;
		}
		protected virtual Field[] GetPrimaryKeyFields()
		{
			return new Field[0];
		}
		protected internal abstract Field[] GetFields();
		protected abstract object[] GetValues();
		protected internal virtual bool GetReadOnly()
		{
			return false;
		}
		protected internal virtual Table GetTable()
		{
			return new Table("TempTable");
		}
		protected abstract void SetValues(IRowReader reader);
		protected virtual void SetExtValues(IRowReader reader)
		{
		}
		internal void SetDbValues(IRowReader reader)
		{
			bool flag = false;
			try
			{
				Monitor.Enter(this, ref flag);
				this.SetValues(reader);
				this.SetExtValues(reader);
				this.isFromDB = true;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
		}
		internal List<FieldValue> GetFieldValues()
		{
			bool flag = false;
			List<FieldValue> result;
			try
			{
				Monitor.Enter(this, ref flag);
				List<FieldValue> list = new List<FieldValue>();
				Field identityField = this.GetIdentityField();
				List<Field> source = new List<Field>(this.GetPrimaryKeyFields());
				Field[] fields = this.GetFields();
				object[] values = this.GetValues();
				if (fields.Length != values.Length)
				{
					throw new DataException("字段与值无法对应！");
				}
				int num = 0;
				Field[] array = fields;
				Field field;
				for (int i = 0; i < array.Length; i++)
				{
					field = array[i];
					FieldValue fieldValue = new FieldValue(field, values[num]);
                    //过滤主键
					if (!string.IsNullOrEmpty(identityField.Name))
					{
                        if (identityField.Name == field.Name)
                        {
                            fieldValue.IsIdentity = true;
                        }
						
					}
					if (source.Any((Field p) => p.Name == field.Name))
					{
						fieldValue.IsPrimaryKey = true;
					}
					if (this.isUpdate)
					{
						if (this.updatelist.Any((Field p) => p.Name == field.Name))
						{
							fieldValue.IsChanged = true;
						}
					}
					else
					{
						if (this.removeinsertlist.Any((Field p) => p.Name == field.Name))
						{
							fieldValue.IsChanged = true;
						}
					}
					list.Add(fieldValue);
					num++;
				}
				result = list;
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this);
				}
			}
			return result;
		}
		public virtual ValidateResult Validation()
		{
			return ValidateResult.Default;
		}
	}
}
