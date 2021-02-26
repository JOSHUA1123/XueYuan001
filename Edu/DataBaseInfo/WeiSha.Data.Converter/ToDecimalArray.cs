using System;
namespace DataBaseInfo.Converter
{
	public class ToDecimalArray : ToArray
	{
		private static Type mValueType = typeof(decimal);
		protected override Type ValueType
		{
			get
			{
				return ToDecimalArray.mValueType;
			}
		}
	}
}
