using System;
namespace DataBaseInfo
{
	internal class UnionItem<T> where T : Entity
	{
		public QuerySection<T> Query
		{
			get;
			set;
		}
		public bool IsUnionAll
		{
			get;
			set;
		}
	}
}
