using System;
namespace Norm.Responses
{
	public class DroppedCollectionResponse : BaseStatusMessage
	{
		public string drop
		{
			get;
			set;
		}
		public double? NIndexesWas
		{
			get;
			set;
		}
		public string Msg
		{
			get;
			set;
		}
		public string Ns
		{
			get;
			set;
		}
	}
}
