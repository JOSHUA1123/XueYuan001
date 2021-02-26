using System;
namespace Norm.Responses
{
	public class DroppedDatabaseResponse : BaseStatusMessage
	{
		public string Dropped
		{
			get;
			set;
		}
	}
}
