using System;
using System.Data;
namespace DataBaseInfo
{
	[Serializable]
	public class SourceRow : IRowReader, IDisposable, IDataSource<DataRow>
	{
		private DataRow row;
		public DataRow OriginalData
		{
			get
			{
				return this.row;
			}
		}
		public object this[int i]
		{
			get
			{
				return this.GetValue(i);
			}
		}
		public object this[string name]
		{
			get
			{
				return this.GetValue(name);
			}
		}
		public SourceRow(DataRow row)
		{
			this.row = row;
		}
		public bool IsDBNull(int index)
		{
			return this.row == null || this.row.Table.Columns.Count - 1 < index || this.row.IsNull(index);
		}
		public object GetValue(int index)
		{
			object obj = this.row[index];
			if (obj == DBNull.Value)
			{
				return null;
			}
			return obj;
		}
		public TResult GetValue<TResult>(int index)
		{
			return CoreHelper.ConvertValue<TResult>(this.GetValue(index));
		}
		public bool IsDBNull(string name)
		{
			return string.IsNullOrEmpty(name) || !this.Contains(name) || this.row.IsNull(name);
		}
		public object GetValue(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			object obj = this.row[name];
			if (obj == DBNull.Value)
			{
				return null;
			}
			return obj;
		}
		public TResult GetValue<TResult>(string name)
		{
			return CoreHelper.ConvertValue<TResult>(this.GetValue(name));
		}
		public string GetString(string name)
		{
			return this.GetValue<string>(name);
		}
		public byte[] GetBytes(string name)
		{
			return this.GetValue<byte[]>(name);
		}
		public short GetInt16(string name)
		{
			return this.GetValue<short>(name);
		}
		public int GetInt32(string name)
		{
			return this.GetValue<int>(name);
		}
		public long GetInt64(string name)
		{
			return this.GetValue<long>(name);
		}
		public decimal GetDecimal(string name)
		{
			return this.GetValue<decimal>(name);
		}
		public double GetDouble(string name)
		{
			return this.GetValue<double>(name);
		}
		public float GetFloat(string name)
		{
			return this.GetValue<float>(name);
		}
		public byte GetByte(string name)
		{
			return this.GetValue<byte>(name);
		}
		public bool GetBoolean(string name)
		{
			return this.GetValue<bool>(name);
		}
		public DateTime GetDateTime(string name)
		{
			return this.GetValue<DateTime>(name);
		}
		public Guid GetGuid(string name)
		{
			return this.GetValue<Guid>(name);
		}
		public bool IsDBNull(Field field)
		{
			return this.IsDBNull(field.OriginalName);
		}
		public object GetValue(Field field)
		{
			return this.GetValue(field.OriginalName);
		}
		public TResult GetValue<TResult>(Field field)
		{
			return this.GetValue<TResult>(field.OriginalName);
		}
		public string GetString(Field field)
		{
			return this.GetString(field.OriginalName);
		}
		public byte[] GetBytes(Field field)
		{
			return this.GetBytes(field.OriginalName);
		}
		public short GetInt16(Field field)
		{
			return this.GetInt16(field.OriginalName);
		}
		public int GetInt32(Field field)
		{
			return this.GetInt32(field.OriginalName);
		}
		public long GetInt64(Field field)
		{
			return this.GetInt64(field.OriginalName);
		}
		public decimal GetDecimal(Field field)
		{
			return this.GetDecimal(field.OriginalName);
		}
		public double GetDouble(Field field)
		{
			return this.GetDouble(field.OriginalName);
		}
		public float GetFloat(Field field)
		{
			return this.GetFloat(field.OriginalName);
		}
		public byte GetByte(Field field)
		{
			return this.GetByte(field.OriginalName);
		}
		public bool GetBoolean(Field field)
		{
			return this.GetBoolean(field.OriginalName);
		}
		public DateTime GetDateTime(Field field)
		{
			return this.GetDateTime(field.OriginalName);
		}
		public Guid GetGuid(Field field)
		{
			return this.GetGuid(field.OriginalName);
		}
		private bool Contains(string name)
		{
			return this.row != null && this.row.Table.Columns.Contains(name);
		}
		public TOutput ToEntity<TOutput>()
		{
			return DataHelper.ConvertType<IRowReader, TOutput>(this);
		}
		public void Dispose()
		{
			this.row = null;
		}
	}
}
