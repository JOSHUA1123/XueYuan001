using System;
namespace DataBaseInfo.Converter
{
	public class ToULongArray : ToArray
	{
		private static Type mValueType = typeof(ulong);
		protected override Type ValueType
		{
			get
			{
				return ToULongArray.mValueType;
			}
		}
	}
}
