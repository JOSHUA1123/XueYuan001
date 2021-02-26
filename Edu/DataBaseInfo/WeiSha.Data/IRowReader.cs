using System;
namespace DataBaseInfo
{
	public interface IRowReader : IDisposable
	{
		object this[int i]
		{
			get;
		}
		object this[string name]
		{
			get;
		}
		bool IsDBNull(int index);
		object GetValue(int index);
		TResult GetValue<TResult>(int index);
		bool IsDBNull(string name);
		object GetValue(string name);
		TResult GetValue<TResult>(string name);
		string GetString(string name);
		byte[] GetBytes(string name);
		short GetInt16(string name);
		int GetInt32(string name);
		long GetInt64(string name);
		decimal GetDecimal(string name);
		double GetDouble(string name);
		float GetFloat(string name);
		byte GetByte(string name);
		bool GetBoolean(string name);
		DateTime GetDateTime(string name);
		Guid GetGuid(string name);
		bool IsDBNull(Field field);
		object GetValue(Field field);
		TResult GetValue<TResult>(Field field);
		string GetString(Field field);
		byte[] GetBytes(Field field);
		short GetInt16(Field field);
		int GetInt32(Field field);
		long GetInt64(Field field);
		decimal GetDecimal(Field field);
		double GetDouble(Field field);
		float GetFloat(Field field);
		byte GetByte(Field field);
		bool GetBoolean(Field field);
		DateTime GetDateTime(Field field);
		Guid GetGuid(Field field);
		TOutput ToEntity<TOutput>();
	}
}
