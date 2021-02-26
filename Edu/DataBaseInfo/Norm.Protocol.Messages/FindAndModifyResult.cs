using Norm.Responses;
using System;
namespace Norm.Protocol.Messages
{
	internal class FindAndModifyResult<T> : BaseStatusMessage
	{
		public T Value
		{
			get;
			set;
		}
	}
}
