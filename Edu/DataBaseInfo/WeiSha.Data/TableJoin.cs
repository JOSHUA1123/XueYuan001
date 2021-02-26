using System;
namespace DataBaseInfo
{
	[Serializable]
	internal class TableJoin
	{
		public Table Table
		{
			get;
			set;
		}
		public JoinType Type
		{
			get;
			set;
		}
		public WhereClip Where
		{
			get;
			set;
		}
	}
}
