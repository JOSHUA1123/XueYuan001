using System;
namespace DataBaseInfo.Logger
{
	public interface ILogable
	{
		event LogEventHandler OnLog;
	}
}
