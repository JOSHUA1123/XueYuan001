using System;
namespace DataBaseInfo
{
	[Serializable]
	public class DbValue
	{
		private string dbvalue;
		public static DbValue DateTime
		{
			get
			{
				return new DbValue("getdate()");
			}
		}
		public static DbValue Default
		{
			get
			{
				return new DbValue("$$$___$$$___$$$");
			}
		}
		internal string Value
		{
			get
			{
				return this.dbvalue;
			}
		}
		public DbValue(string dbvalue)
		{
			this.dbvalue = dbvalue;
		}
		public override bool Equals(object obj)
		{
			return obj is DbValue && string.Compare(this.Value, (obj as DbValue).Value, true) == 0;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
