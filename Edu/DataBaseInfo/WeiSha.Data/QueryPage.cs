using System;
namespace DataBaseInfo
{
	public class QueryPage
	{
		public int PageIndex
		{
			get;
			set;
		}
		public int PageSize
		{
			get;
			set;
		}
		public int PageCount
		{
			get;
			set;
		}
		public int TotalPage
		{
			get;
			set;
		}
		public int Records
		{
			get;
			set;
		}
		public string OrderBy
		{
			get;
			set;
		}
		public string Sord
		{
			get;
			set;
		}
	}
}
