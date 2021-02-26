using System;
using System.Collections.Generic;
namespace Norm.Responses
{
	internal class DistinctValuesResponse<T> : BaseStatusMessage
	{
		public List<T> Values
		{
			get;
			set;
		}
	}
}
