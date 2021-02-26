using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Common;
namespace DataBaseInfo
{
	public class DbBatch : IDbBatch, IDbProcess, IDisposable
	{
		private bool useBatch;
		private int batchSize;
		private DbProvider dbProvider;
		private DbTrans dbTrans;
		private List<DbCommand> commandList = new List<DbCommand>();
		internal DbBatch(DbProvider dbProvider, DbTrans dbTran, int batchSize)
		{
			this.dbProvider = dbProvider;
			this.batchSize = batchSize;
			this.dbTrans = dbTran;
			this.useBatch = true;
			if (batchSize < 0 || batchSize > 100)
			{
				throw new DataException("请设置batchSize的值在1-100之间！");
			}
		}
		internal DbBatch(DbProvider dbProvider, DbTrans dbTran)
		{
			this.dbProvider = dbProvider;
			this.dbTrans = dbTran;
			this.useBatch = false;
		}
		public int Execute(out IList<Exception> errors)
		{
			errors = new List<Exception>();
			int num = 0;
			if (this.commandList.Count == 0)
			{
				return num;
			}
			if (!this.dbProvider.SupportBatch)
			{
				using (List<DbCommand>.Enumerator enumerator = this.commandList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DbCommand current = enumerator.Current;
						try
						{
							num += this.dbProvider.ExecuteNonQuery(current, this.dbTrans);
						}
						catch (Exception item)
						{
							errors.Add(item);
						}
						Thread.Sleep(10);
					}
					goto IL_22A;
				}
			}
			int num2 = Convert.ToInt32(Math.Ceiling((double)this.commandList.Count * 1.0 / (double)this.batchSize));
			for (int i = 0; i < num2; i++)
			{
				DbCommand dbCommand = this.dbProvider.CreateSqlCommand("init");
				List<DbCommand> list = new List<DbCommand>();
				int count = this.batchSize;
				if ((i + 1) * this.batchSize > this.commandList.Count)
				{
					count = this.commandList.Count - i * this.batchSize;
				}
				list.AddRange(this.commandList.GetRange(i * this.batchSize, count));
				StringBuilder stringBuilder = new StringBuilder();
				int num3 = 0;
				foreach (DbCommand current2 in list)
				{
					string commandText = current2.CommandText;
					foreach (DbParameter dbParameter in current2.Parameters)
					{
						DbParameter value = (DbParameter)((ICloneable)dbParameter).Clone();
						dbCommand.Parameters.Add(value);
					}
					stringBuilder.Append(commandText);
					stringBuilder.Append(";\r\n");
					num3++;
				}
				dbCommand.CommandText = stringBuilder.ToString();
				try
				{
					num += this.dbProvider.ExecuteNonQuery(dbCommand, this.dbTrans);
				}
				catch (Exception item2)
				{
					errors.Add(item2);
				}
				Thread.Sleep(10);
			}
			IL_22A:
			this.commandList.Clear();
			return num;
		}
		public int Save<T>(Table table, T entity) where T : Entity
		{
			Predicate<FieldValue> predicate = null;
			Predicate<FieldValue> predicate2 = null;
			Type type = entity.GetType();
			PropertyInfo[] properties = type.GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				if (propertyInfo.PropertyType.FullName == "System.DateTime")
				{
					DateTime dateTime = (DateTime)propertyInfo.GetValue(entity, null);
					if (dateTime < SqlDateTime.MinValue.Value)
					{
						dateTime = SqlDateTime.MinValue.Value;
					}
					if (dateTime > SqlDateTime.MaxValue.Value)
					{
						dateTime = SqlDateTime.MaxValue.Value;
					}
					propertyInfo.SetValue(entity, dateTime, null);
				}
			}
			List<FieldValue> fieldValues = entity.GetFieldValues();
			ValidateResult validateResult = entity.Validation();
			if (!validateResult.IsSuccess)
			{
				List<string> list = new List<string>();
				foreach (InvalidValue current in validateResult.InvalidValues)
				{
					list.Add(current.Field.PropertyName + " : " + current.Message);
				}
				string message = string.Join("\r\n", list.ToArray());
				throw new DataException(message);
			}
			EntityState objectState = entity.As<IEntityBase>().GetObjectState();
			int limitNumber = License.Value.VersionLimit.GetLimitNumber(type.Name);
			int result;
			if (objectState == EntityState.Insert)
			{
				if (limitNumber > 0)
				{
					int num = Gateway.Count(type.Name);
					if (num >= limitNumber)
					{
						string arg = Common.Entity.Get[type.Name];
						string text = "操作失败！当前版本允许的{0}数最大为{1},当前数量已经达到{2}(此数据涵盖所有入驻机构)。";
						text = string.Format(text, arg, limitNumber, num);
						throw new ExceptionForLicense(text);
					}
				}
				List<FieldValue> arg_1FD_0 = fieldValues;
				if (predicate == null)
				{
					predicate = ((FieldValue fv) => fv.IsChanged);
				}
				arg_1FD_0.RemoveAll(predicate);
				object obj;
				result = this.Insert<T>(table, fieldValues, out obj);
				if (obj != null )
				{
                    if (entity.IdentityField != null)
                    {
                        CoreHelper.SetPropertyValue(entity, entity.IdentityField.PropertyName, obj);
                    }
					
				}
			}
			else
			{
				if (limitNumber > 0)
				{
					Random random = new Random();
					int num2 = random.Next(100, 999);
					if (num2 % 3 == 0)
					{
						int num3 = Gateway.Count(type.Name);
						if (num3 > limitNumber)
						{
							string arg2 = Common.Entity.Get[type.Name];
							string text2 = "操作失败！当前版本允许的{0}数最大为{1},当前数量已经达到{2}(此数据涵盖所有入驻机构)。该错误随机产生。";
							text2 = string.Format(text2, arg2, limitNumber, num3);
							throw new ExceptionForLicense(text2);
						}
					}
				}
				WhereClip pkWhere = DataHelper.GetPkWhere<T>(entity.GetTable(), entity);
				List<FieldValue> arg_2E5_0 = fieldValues;
				if (predicate2 == null)
				{
					predicate2 = ((FieldValue fv) => !fv.IsChanged || fv.IsIdentity || fv.IsPrimaryKey);
				}
				arg_2E5_0.RemoveAll(predicate2);
				result = this.Update<T>(table, fieldValues, pkWhere);
			}
			entity.AttachSet(new Field[0]);
			return result;
		}
		private void AddCommand(DbCommand cmd)
		{
			this.commandList.Add(cmd);
		}
		internal int Insert<T>(Table table, List<FieldValue> fvlist, out object retVal) where T : Entity
		{
			int result = 0;
			retVal = null;
			T t = CoreHelper.CreateInstance<T>();
			if (this.useBatch)
			{
				DbCommand cmd = this.dbProvider.CreateInsert<T>(table, fvlist, t.IdentityField, t.SequenceName);
				this.AddCommand(cmd);
			}
			else
			{
				result = this.dbProvider.Insert<T>(table, fvlist, this.dbTrans, t.IdentityField, t.SequenceName, true, out retVal);
			}
			return result;
		}
		private int Update<T>(Table table, List<FieldValue> fvlist, WhereClip where) where T : Entity
		{
			if (fvlist.FindAll((FieldValue fv) => fv.IsChanged).Count == 0)
			{
				return -1;
			}
			int result = 0;
			if (this.useBatch)
			{
				DbCommand cmd = this.dbProvider.CreateUpdate<T>(table, fvlist, where);
				this.AddCommand(cmd);
			}
			else
			{
				result = this.dbProvider.Update<T>(table, fvlist, where, this.dbTrans);
			}
			return result;
		}
		public int Delete<T>(Table table, T entity) where T : Entity
		{
			WhereClip pkWhere = DataHelper.GetPkWhere<T>(table, entity);
			int result = 0;
			if (this.useBatch)
			{
				DbCommand cmd = this.dbProvider.CreateDelete<T>(table, pkWhere);
				this.AddCommand(cmd);
			}
			else
			{
				result = this.dbProvider.Delete<T>(table, pkWhere, this.dbTrans);
			}
			return result;
		}
		public int Delete<T>(Table table, params object[] pkValues) where T : Entity
		{
			WhereClip pkWhere = DataHelper.GetPkWhere<T>(table, pkValues);
			return this.dbProvider.Delete<T>(table, pkWhere, this.dbTrans);
		}
		public int InsertOrUpdate<T>(Table table, T entity, params Field[] fields) where T : Entity
		{
			if (this.Exists<T>(table, entity, fields))
			{
				if (fields != null && fields.Length > 0)
				{
					List<FieldValue> fvlist = entity.As<IEntityInfo>().UpdateFieldValues.ToList<FieldValue>();
					WhereClip allWhere = DataHelper.GetAllWhere<T>(table, entity, fields);
					return this.Update<T>(table, fvlist, allWhere);
				}
				entity.Attach(new Field[0]);
			}
			else
			{
				entity.Detach(new Field[0]);
			}
			return this.Save<T>(table, entity);
		}
		public int InsertOrUpdate<T>(Table table, FieldValue[] fvs, WhereClip where) where T : Entity
		{
			if (this.Exists<T>(table, where))
			{
				return this.Update<T>(table, fvs.ToList<FieldValue>(), where);
			}
			object obj;
			return this.Insert<T>(table, fvs.ToList<FieldValue>(), out obj);
		}
		public int Save<T>(T entity) where T : Entity
		{
			return this.Save<T>(null, entity);
		}
		public int Delete<T>(T entity) where T : Entity
		{
			return this.Delete<T>(null, entity);
		}
		public int Delete<T>(params object[] pkValues) where T : Entity
		{
			return this.Delete<T>(null, pkValues);
		}
		public int InsertOrUpdate<T>(T entity, params Field[] fields) where T : Entity
		{
			return this.InsertOrUpdate<T>(null, entity, fields);
		}
		public int InsertOrUpdate<T>(FieldValue[] fvs, WhereClip where) where T : Entity
		{
			return this.InsertOrUpdate<T>(null, fvs, where);
		}
		private bool Exists<T>(Table table, T entity, Field[] fields) where T : Entity
		{
			WhereClip where = WhereClip.None;
			if (fields != null && fields.Length > 0)
			{
				where = DataHelper.GetAllWhere<T>(table, entity, fields);
			}
			else
			{
				where = DataHelper.GetPkWhere<T>(table, entity);
			}
			return this.Exists<T>(table, where);
		}
		private bool Exists<T>(Table table, WhereClip where) where T : Entity
		{
			if (where == null || where == WhereClip.None)
			{
				throw new DataException("在判断记录是否存在时出现异常，条件为null或WhereClip.None！");
			}
			FromSection<T> fromSection = new FromSection<T>(this.dbProvider, this.dbTrans, table, null);
			return fromSection.Where(where).Count() > 0;
		}
		public void Dispose()
		{
			this.commandList.Clear();
		}
	}
}
