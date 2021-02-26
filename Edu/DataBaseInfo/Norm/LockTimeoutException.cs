using System;
using System.Runtime.Serialization;
namespace Norm
{
	[Serializable]
	public class LockTimeoutException : ApplicationException
	{
		public LockTimeoutException() : base("Timeout waiting for lock")
		{
		}
		public LockTimeoutException(string message) : base(message)
		{
		}
		public LockTimeoutException(string message, Exception innerException) : base(message, innerException)
		{
		}
		protected LockTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
