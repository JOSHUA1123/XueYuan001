using System;
namespace DataBaseInfo
{
	[Serializable]
	public class DbField : Field
	{
		internal override string Name
		{
			get
			{
				return base.OriginalName;
			}
		}
		public DbField(string fieldName) : base(string.Format("__${0}$__", fieldName))
		{
		}
	}
}
