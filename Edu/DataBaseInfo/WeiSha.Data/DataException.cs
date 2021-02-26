using System;
namespace DataBaseInfo
{
	[Serializable]
	public class DataException : SNDataException
	{
		public DataException(string message) : base(ExceptionType.DataException, message)
		{
		}
		public DataException(string message, Exception ex) : base(ExceptionType.DataException, message, ex)
		{
		}
	}
}
