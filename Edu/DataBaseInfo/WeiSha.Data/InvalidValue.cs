using System;
namespace DataBaseInfo
{
	[Serializable]
	public class InvalidValue
	{
		public Field Field
		{
			get;
			set;
		}
		public string Message
		{
			get;
			set;
		}
	}
}
