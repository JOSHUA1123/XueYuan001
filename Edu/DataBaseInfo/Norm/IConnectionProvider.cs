using System;
namespace Norm
{
	public interface IConnectionProvider
	{
		ConnectionStringBuilder ConnectionString
		{
			get;
		}
		IConnection Open(string options);
		void Close(IConnection connection);
	}
}
