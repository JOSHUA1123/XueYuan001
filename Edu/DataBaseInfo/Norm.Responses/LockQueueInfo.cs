using System;
namespace Norm.Responses
{
	public class LockQueueInfo
	{
		public int Total
		{
			get;
			set;
		}
		public int Readers
		{
			get;
			set;
		}
		public int Writers
		{
			get;
			set;
		}
	}
}
