using System;
namespace Norm
{
	internal interface IOptionsContainer
	{
		void SetQueryTimeout(int timeout);
		void SetStrictMode(bool strict);
		void SetPoolSize(int size);
		void SetPooled(bool pooled);
		void SetTimeout(int timeout);
		void SetLifetime(int lifetime);
		void SetWriteCount(int writeCount);
	}
}
