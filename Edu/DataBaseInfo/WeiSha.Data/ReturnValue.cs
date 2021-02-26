using System;
namespace DataBaseInfo
{
	public delegate object ReturnValue<T>(T value);
	[Serializable]
	public sealed class ReturnValue
	{
		public object Data
		{
			get;
			set;
		}
		public Exception Error
		{
			get;
			set;
		}
		public int Count
		{
			get;
			set;
		}
		public bool IsError
		{
			get
			{
				return this.Error != null;
			}
		}
	}
}
