using System;
using System.Collections.Generic;
namespace Norm.Responses
{
	public class ListDatabasesResponse : BaseStatusMessage
	{
		public double? TotalSize
		{
			get;
			set;
		}
		public List<DatabaseInfo> Databases
		{
			get;
			set;
		}
	}
}
