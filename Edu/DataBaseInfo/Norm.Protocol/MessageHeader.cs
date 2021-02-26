using System;
namespace Norm.Protocol
{
	internal class MessageHeader
	{
		public int MessageLength
		{
			get;
			set;
		}
		public int RequestID
		{
			get;
			set;
		}
		public int ResponseTo
		{
			get;
			set;
		}
		public MongoOp OpCode
		{
			get;
			set;
		}
	}
}
