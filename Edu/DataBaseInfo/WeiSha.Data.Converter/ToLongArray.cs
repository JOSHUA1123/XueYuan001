using System;
namespace DataBaseInfo.Converter
{
	public class ToLongArray : ToArray
	{
		private static Type mValueType = typeof(long);
		protected override Type ValueType
		{
			get
			{
				return ToLongArray.mValueType;
			}
		}
	}
}
