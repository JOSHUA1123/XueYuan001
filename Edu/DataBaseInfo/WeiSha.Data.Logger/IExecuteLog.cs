using System;
using System.Data;
namespace DataBaseInfo.Logger
{
	public interface IExecuteLog
	{
		void Begin(IDbCommand command);
		void End(IDbCommand command, ReturnValue retValue, long elapsedTime);
	}
}
