using System;
using System.Collections.Generic;
using System.Data;
namespace DataBaseInfo
{
	[Serializable]
	public class SourceTable : DataTable, ISourceTable, IListConvert<IRowReader>, IDataSource<DataTable>, IDisposable
	{
		public DataTable OriginalData
		{
			get
			{
				return base.Copy();
			}
		}
		public int RowCount
		{
			get
			{
				return base.Rows.Count;
			}
		}
		public int ColumnCount
		{
			get
			{
				return base.Columns.Count;
			}
		}
		public IRowReader this[int index]
		{
			get
			{
				if (base.Rows.Count == 0)
				{
					return null;
				}
				if (base.Rows.Count > index)
				{
					DataRow row = base.Rows[index];
					return new SourceRow(row);
				}
				return null;
			}
		}
		public SourceTable()
		{
			base.TableName = "DataTable";
		}
		public SourceTable(string tableName)
		{
			base.TableName = tableName;
		}
		public SourceTable(DataTable dt) : this()
		{
			if (dt != null)
			{
				if (!string.IsNullOrEmpty(dt.TableName))
				{
					base.TableName = dt.TableName;
				}
				foreach (DataColumn dataColumn in dt.Columns)
				{
					base.Columns.Add(dataColumn.ColumnName, dataColumn.DataType);
				}
				if (dt.Rows.Count != 0)
				{
					foreach (DataRow row in dt.Rows)
					{
						base.ImportRow(row);
					}
				}
			}
		}
		public SourceTable Select(params string[] names)
		{
			DataTable dt = base.Copy();
			List<string> list = new List<string>(names);
			list.ForEach(delegate(string p)
			{
				p.ToLower();
			});
			foreach (DataColumn dataColumn in base.Columns)
			{
				if (!list.Contains(dataColumn.ColumnName.ToLower()))
				{
					dt.Columns.Remove(dataColumn.ColumnName);
				}
			}
			int index = 0;
			list.ForEach(delegate(string p)
			{
				dt.Columns[p].SetOrdinal(index++);
			});
			return dt as SourceTable;
		}
		public SourceTable Filter(string expression)
		{
			DataRow[] array = base.Select(expression);
			DataTable dataTable = base.Clone();
			if (array != null && array.Length > 0)
			{
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow row = array2[i];
					dataTable.ImportRow(row);
				}
			}
			return dataTable as SourceTable;
		}
		public SourceTable Sort(string sort)
		{
			DataRow[] array = base.Select(null, sort);
			DataTable dataTable = base.Clone();
			if (array != null && array.Length > 0)
			{
				DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					DataRow row = array2[i];
					dataTable.ImportRow(row);
				}
			}
			return dataTable as SourceTable;
		}
		public void Remove(params string[] names)
		{
			List<string> list = new List<string>(names);
			list.ForEach(delegate(string p)
			{
				if (base.Columns.Contains(p))
				{
					base.Columns.Remove(p);
				}
			});
		}
		public void SetOrdinal(string name, int index)
		{
			if (!base.Columns.Contains(name))
			{
				throw new DataException(string.Format("当前表中不存在字段【{0}】！", name));
			}
			base.Columns[name].SetOrdinal(index);
		}
		public void Add(string name, Type type)
		{
			base.Columns.Add(name, type);
		}
		public void Add(string name, Type type, string expression)
		{
			base.Columns.Add(name, type, expression);
		}
		public void Rename(string oldname, string newname)
		{
			if (!base.Columns.Contains(oldname))
			{
				throw new DataException(string.Format("当前表中不存在字段【{0}】！", oldname));
			}
			if (string.IsNullOrEmpty(newname))
			{
				throw new DataException("设置的新列名不能为null或空！");
			}
			if (base.Columns.Contains(newname))
			{
				throw new DataException(string.Format("当前表中已存在字段【{0}】！", newname));
			}
			DataColumn dataColumn = base.Columns[oldname];
			dataColumn.ColumnName = newname;
		}
		public void Revalue<T>(string readName, string changeName, ReturnValue<T> revalue)
		{
			if (!base.Columns.Contains(readName))
			{
				throw new DataException(string.Format("当前表中不存在字段【{0}】！", readName));
			}
			if (!base.Columns.Contains(changeName))
			{
				throw new DataException(string.Format("当前表中不存在字段【{0}】！", changeName));
			}
			foreach (DataRow dataRow in base.Rows)
			{
				dataRow[changeName] = revalue(CoreHelper.ConvertValue<T>(dataRow[readName]));
			}
		}
		public void Fill(FillRelation relation, params string[] fillNames)
		{
			SourceTable dataSource = relation.DataSource;
			string parentName = relation.ParentName;
			string childName = relation.ChildName;
			if (relation == null)
			{
				return;
			}
			if (dataSource == null || dataSource.RowCount == 0)
			{
				return;
			}
			if (fillNames == null || fillNames.Length == 0)
			{
				return;
			}
			if (!base.Columns.Contains(parentName))
			{
				throw new DataException(string.Format("当前表中不存在字段【{0}】！", parentName));
			}
			if (!dataSource.Columns.Contains(childName))
			{
				throw new DataException(string.Format("关联表中不存在字段【{0}】！", childName));
			}
			for (int i = 0; i < fillNames.Length; i++)
			{
				if (!dataSource.Columns.Contains(fillNames[i]))
				{
					throw new DataException(string.Format("关联表中不存在字段【{0}】！", fillNames[i]));
				}
				if (!base.Columns.Contains(fillNames[i]))
				{
					base.Columns.Add(fillNames[i], dataSource.Columns[fillNames[i]].DataType);
				}
			}
			IDictionary<string, IRowReader> dictionary = new Dictionary<string, IRowReader>();
			for (int j = 0; j < dataSource.RowCount; j++)
			{
				IRowReader rowReader = dataSource[j];
				string @string = rowReader.GetString(childName);
				dictionary[@string] = rowReader;
			}
			foreach (DataRow dataRow in base.Rows)
			{
				string key = dataRow[parentName].ToString();
				if (dictionary.ContainsKey(key))
				{
					IRowReader rowReader2 = dictionary[key];
					for (int k = 0; k < fillNames.Length; k++)
					{
						if (dataSource.Columns.Contains(fillNames[k]))
						{
							object obj = rowReader2[fillNames[k]];
							dataRow[fillNames[k]] = ((obj == null) ? DBNull.Value : obj);
						}
					}
				}
			}
		}
		public SourceList<TOutput> ConvertTo<TOutput>()
		{
			return this.ConvertAll<TOutput>((IRowReader p) => p.ToEntity<TOutput>());
		}
		public SourceList<IOutput> ConvertTo<TOutput, IOutput>() where TOutput : IOutput
		{
			if (!typeof(TOutput).IsClass)
			{
				throw new DataException("TOutput必须是Class类型！");
			}
			if (!typeof(IOutput).IsInterface)
			{
				throw new DataException("IOutput必须是Interface类型！");
			}
			return this.ConvertTo<TOutput>().ConvertTo<IOutput>();
		}
		public SourceList<TOutput> ConvertAll<TOutput>(Converter<IRowReader, TOutput> handler)
		{
			SourceList<TOutput> sourceList = new SourceList<TOutput>();
			for (int i = 0; i < this.RowCount; i++)
			{
				sourceList.Add(handler(this[i]));
			}
			return sourceList;
		}
		public SourceReader ToReader()
		{
			DataTableReader reader = base.CreateDataReader();
			return new SourceReader(reader);
		}
		public new void Dispose()
		{
			base.Rows.Clear();
			base.Columns.Clear();
			base.Dispose(true);
		}
	}
}
