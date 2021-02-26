using System;
namespace DataBaseInfo
{
	[Serializable]
	internal class TableEntity
	{
		public Table Table
		{
			get;
			set;
		}
		public Entity Entity
		{
			get;
			set;
		}
	}
}
