using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
namespace DataBaseInfo
{
	public static class DataHelper
	{
		public static TOutput ConvertType<TInput, TOutput>(TInput obj)
		{
			if (obj == null)
			{
				return default(TOutput);
			}
			if (obj is TOutput && typeof(TOutput).IsInterface)
			{
				return (TOutput)((object)obj);
			}
			TOutput tOutput = default(TOutput);
			try
			{
				if (typeof(TOutput) == typeof(TInput))
				{
					tOutput = CoreHelper.CreateInstance<TOutput>(obj.GetType());
				}
				else
				{
					tOutput = CoreHelper.CreateInstance<TOutput>();
				}
			}
			catch (Exception ex)
			{
				throw new DataException(string.Format("创建类型对象【{0}】出错，可能不存在构造函数！", typeof(TOutput).FullName), ex);
			}
			if (tOutput is Entity && obj is IRowReader)
			{
				(tOutput as Entity).SetDbValues(obj as IRowReader);
			}
			else
			{
				PropertyInfo[] propertiesFromType = CoreHelper.GetPropertiesFromType<TOutput>();
				int i = 0;
				while (i < propertiesFromType.Length)
				{
					PropertyInfo propertyInfo = propertiesFromType[i];
					object obj2;
					if (obj is NameValueCollection)
					{
						NameValueCollection nameValueCollection = obj as NameValueCollection;
						if (nameValueCollection[propertyInfo.Name] != null)
						{
							obj2 = nameValueCollection[propertyInfo.Name];
							goto IL_208;
						}
					}
					else
					{
						if (obj is IDictionary)
						{
							IDictionary dictionary = obj as IDictionary;
							if (dictionary.Contains(propertyInfo.Name) && dictionary[propertyInfo.Name] != null)
							{
								obj2 = dictionary[propertyInfo.Name];
								goto IL_208;
							}
						}
						else
						{
							if (obj is IRowReader)
							{
								IRowReader rowReader = obj as IRowReader;
								if (!rowReader.IsDBNull(propertyInfo.Name))
								{
									obj2 = rowReader[propertyInfo.Name];
									goto IL_208;
								}
							}
							else
							{
								if (!(obj is DataRow))
								{
									obj2 = CoreHelper.GetPropertyValue(obj, propertyInfo.Name);
									goto IL_208;
								}
								IRowReader rowReader2 = new SourceRow(obj as DataRow);
								if (!rowReader2.IsDBNull(propertyInfo.Name))
								{
									obj2 = rowReader2[propertyInfo.Name];
									goto IL_208;
								}
							}
						}
					}
					IL_218:
					i++;
					continue;
					IL_208:
					if (obj2 != null)
					{
						CoreHelper.SetPropertyValue(tOutput, propertyInfo, obj2);
						goto IL_218;
					}
					goto IL_218;
				}
			}
			if (tOutput != null && tOutput is Entity)
			{
				(tOutput as Entity).AttachSet(new Field[0]);
				(tOutput as Entity).Detach(new Field[0]);
			}
			return tOutput;
		}
		public static bool IsNullOrEmpty(WhereClip where)
		{
			return where == null || string.IsNullOrEmpty(where.ToString());
		}
		public static bool IsNullOrEmpty(OrderByClip order)
		{
			return order == null || string.IsNullOrEmpty(order.ToString());
		}
		public static bool IsNullOrEmpty(GroupByClip group)
		{
			return group == null || string.IsNullOrEmpty(group.ToString());
		}
		internal static string FormatValue(object val)
		{
			if (val == null || val == DBNull.Value)
			{
				return "null";
			}
			Type type = val.GetType();
			if (type == typeof(Guid))
			{
				return string.Format("'{0}'", val);
			}
			if (type == typeof(DateTime))
			{
				return string.Format("'{0}'", ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (type == typeof(bool))
			{
				if (!(bool)val)
				{
					return "0";
				}
				return "1";
			}
			else
			{
				if (val is Field)
				{
					return ((Field)val).Name;
				}
				if (val is DbValue)
				{
					return ((DbValue)val).Value;
				}
				if (type.IsEnum)
				{
					return Convert.ToInt32(val).ToString();
				}
				if (!type.IsValueType)
				{
					return string.Format("N'{0}'", val.ToString());
				}
				if (CoreHelper.CheckStructType(type))
				{
					return null;
				}
				return val.ToString();
			}
		}
		internal static string FormatSQL(string sql, char leftToken, char rightToken, bool isAccess)
		{
			if (sql == null)
			{
				return string.Empty;
			}
			if (isAccess)
			{
				sql = sql.Replace("__[[", '('.ToString()).Replace("]]__", ')'.ToString()).Replace("__[", leftToken.ToString()).Replace("]__", rightToken.ToString());
			}
			else
			{
				sql = sql.Replace("__[[", ' '.ToString()).Replace("]]__", ' '.ToString()).Replace("__[", leftToken.ToString()).Replace("]__", rightToken.ToString());
			}
			return CoreHelper.RemoveSurplusSpaces(sql);
		}
		internal static object[] CheckAndReturnValues(object[] values)
		{
			if (values == null)
			{
				throw new DataException("传入的数据不能为null！");
			}
			if (values.Length == 0)
			{
				throw new DataException("传入的数据个数不能为0！");
			}
			if (values.Length == 1 && values[0].GetType().IsArray)
			{
				try
				{
					values = ArrayList.Adapter((Array)values[0]).ToArray();
				}
				catch
				{
					throw new DataException("传入的数据不能正确被解析！");
				}
			}
			return values;
		}
		internal static WhereClip GetPkWhere<T>(Table table, object[] pkValues) where T : Entity
		{
			WhereClip where = null;
			T t = CoreHelper.CreateInstance<T>();
			List<FieldValue> fieldValues = t.GetFieldValues();
			pkValues = DataHelper.CheckAndReturnValues(pkValues);
			int count = fieldValues.FindAll((FieldValue p) => p.IsPrimaryKey).Count;
			if (pkValues.Length != count)
			{
				throw new DataException("传入的数据与主键无法对应，应该传入【" + count + "】个主键值！");
			}
			fieldValues.ForEach(delegate(FieldValue fv)
			{
				int num = 0;
				if (fv.IsPrimaryKey)
				{
					where &= (fv.Field.At(table) == pkValues[num]);
					num++;
				}
			});
			return where;
		}
		internal static WhereClip GetPkWhere<T>(Table table, T entity) where T : Entity
		{
			WhereClip where = null;
			List<FieldValue> fieldValues = entity.GetFieldValues();
			fieldValues.ForEach(delegate(FieldValue fv)
			{
				if (fv.IsPrimaryKey)
				{
					where &= (fv.Field.At(table) == fv.Value);
				}
			});
			return where;
		}
		internal static WhereClip GetAllWhere<T>(Table table, T entity, Field[] fields) where T : Entity
		{
			WhereClip where = null;
			List<FieldValue> fieldValues = entity.GetFieldValues();
			List<Field> flist = fields.ToList<Field>();
			fieldValues.ForEach(delegate(FieldValue fv)
			{
				if (flist.Exists((Field p) => fv.Field.Name == p.Name))
				{
					where &= (fv.Field.At(table) == fv.Value);
				}
			});
			return where;
		}
		internal static List<FieldValue> CreateFieldValue(Field[] fields, object[] values, bool isInsert)
		{
			if (fields == null || values == null)
			{
				throw new DataException("字段及值不能为null！");
			}
			if (fields.Length != values.Length)
			{
				throw new DataException("字段及值长度不一致！");
			}
			int num = 0;
			List<FieldValue> list = new List<FieldValue>();
			for (int i = 0; i < fields.Length; i++)
			{
				Field field = fields[i];
				FieldValue fieldValue = new FieldValue(field, values[num]);
				if (isInsert && values[num] is Field)
				{
					fieldValue.IsIdentity = true;
				}
				else
				{
					if (!isInsert)
					{
						fieldValue.IsChanged = true;
					}
				}
				list.Add(fieldValue);
				num++;
			}
			return list;
		}
		public static string GetCommandLog(IDbCommand command)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("{0}\t{1}\t\r\n", command.CommandType, command.CommandText));
			if (command.Parameters != null && command.Parameters.Count > 0)
			{
				stringBuilder.Append("Parameters:\r\n");
				foreach (DbParameter dbParameter in command.Parameters)
				{
					if (dbParameter.Size > 0)
					{
						stringBuilder.Append(string.Format("{0}[{1}({2})] = {3}\r\n", new object[]
						{
							dbParameter.ParameterName,
							dbParameter.DbType,
							dbParameter.Size,
							dbParameter.Value
						}));
					}
					else
					{
						stringBuilder.Append(string.Format("{0}[{1}] = {2}\r\n", dbParameter.ParameterName, dbParameter.DbType, dbParameter.Value));
					}
				}
			}
			stringBuilder.Append("\r\n");
			return stringBuilder.ToString();
		}
	}
}
