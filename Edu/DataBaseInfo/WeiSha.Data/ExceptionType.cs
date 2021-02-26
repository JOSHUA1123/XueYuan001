using System;
namespace DataBaseInfo
{
	[Serializable]
	public enum ExceptionType
	{
		Unknown,
		DataException,
		WebException,
		RemotingException,
		IoCException,
		TaskException
	}
}
