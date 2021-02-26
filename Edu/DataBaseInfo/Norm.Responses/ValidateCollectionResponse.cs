using System;
namespace Norm.Responses
{
	public class ValidateCollectionResponse : BaseStatusMessage
	{
		public string Ns
		{
			get;
			set;
		}
		public string Result
		{
			get;
			set;
		}
		public bool? Valid
		{
			get;
			set;
		}
		public double? LastExtentSize
		{
			get;
			set;
		}
	}
}
