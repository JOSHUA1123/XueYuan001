using System;
namespace DataBaseInfo.Logger
{
	public interface ILog
	{
		void Write(string log, LogType type);
		void Write(Exception error);
	}
}
