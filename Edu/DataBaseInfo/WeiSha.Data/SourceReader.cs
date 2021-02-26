using System;
using System.Collections.Generic;
using System.Data;
namespace DataBaseInfo
{
	[Serializable]
	public sealed class SourceReader : ISourceReader, IListConvert<IRowReader>, IRowReader, IDataSource<IDataReader>, IDisposable
	{
		private IDataReader reader;
		private IDictionary<string, int> dictIndex;
		public int FieldCount
		{
			get
			{
				return this.reader.FieldCount;
			}
		}
		public IDataReader OriginalData
		{
			get
			{
				return this.reader;
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
		public SourceReader(IDataReader reader)
		{
			this.reader = reader;
			this.dictIndex = new Dictionary<string, int>();
			if (!reader.IsClosed)
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					string text = reader.GetName(i);
					text = text.ToLower();
					if (!this.dictIndex.ContainsKey(text))
					{
						this.dictIndex.Add(text, i);
					}
				}
			}
		}
		public bool Read()
		{
			return this.reader.Read();
		}
		public bool NextResult()
		{
			return this.reader.NextResult();
		}
		public void Close()
		{
			if (!this.reader.IsClosed)
			{
				this.reader.Close();
				this.reader.Dispose();
			}
		}
		public void Dispose()
		{
			this.Close();
		}
		public bool IsDBNull(int index)
		{
			return this.reader.IsDBNull(index);
		}
		public object GetValue(int index)
		{
			object value = this.reader.GetValue(index);
			if (value == DBNull.Value)
			{
				return null;
			}
			return value;
		}
		public TResult GetValue<TResult>(int index)
		{
			return CoreHelper.ConvertValue<TResult>(this.GetValue(index));
		}
		public bool IsDBNull(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return true;
			}
			if (this.reader.IsClosed)
			{
				return true;
			}
			name = name.ToLower();
			return !this.dictIndex.ContainsKey(name) || this.reader.IsDBNull(this.dictIndex[name]);
		}
		public object GetValue(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			if (this.reader.IsClosed)
			{
				return null;
			}
			name = name.ToLower();
			if (!this.dictIndex.ContainsKey(name))
			{
				return null;
			}
			return this.GetValue(this.dictIndex[name]);
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
		public SourceList<TOutput> ConvertTo<TOutput>()
		{
			return this.ConvertAll<TOutput>((IRowReader p) => p.ToEntity<TOutput>());
		}
		public TOutput ToEntity<TOutput>()
		{
			return DataHelper.ConvertType<IRowReader, TOutput>(this);
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
			while (this.Read())
			{
				sourceList.Add(handler(this));
			}
			this.reader.Close();
			this.reader.Dispose();
			return sourceList;
		}
		public SourceTable ToTable()
		{
			SourceTable sourceTable = new SourceTable();
			sourceTable.Load(this.reader);
			this.reader.Close();
			this.reader.Dispose();
			return sourceTable;
		}
	}
}
