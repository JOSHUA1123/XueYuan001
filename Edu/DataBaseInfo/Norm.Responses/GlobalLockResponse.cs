using System;
namespace Norm.Responses
{
	public class GlobalLockResponse
	{
		public double? TotalTime
		{
			get;
			set;
		}
		public double? LockTime
		{
			get;
			set;
		}
		public double? Ratio
		{
			get;
			set;
		}
		public LockQueueInfo CurrentQueue
		{
			get;
			set;
		}
	}
}
