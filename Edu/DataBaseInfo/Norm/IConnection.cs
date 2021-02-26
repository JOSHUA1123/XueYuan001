using System;
using System.Net.Sockets;
namespace Norm
{
	public interface IConnection : IDisposable
	{
		int VerifyWriteCount
		{
			get;
		}
		string ConnectionString
		{
			get;
		}
		TcpClient Client
		{
			get;
		}
		bool IsConnected
		{
			get;
		}
		bool IsInvalid
		{
			get;
		}
		DateTime Created
		{
			get;
		}
		int QueryTimeout
		{
			get;
		}
		bool StrictMode
		{
			get;
		}
		string UserName
		{
			get;
		}
		string Database
		{
			get;
		}
		NetworkStream GetStream();
		string Digest(string nounce);
		void LoadOptions(string options);
		void ResetOptions();
		void Write(byte[] bytes, int start, int size);
	}
}
