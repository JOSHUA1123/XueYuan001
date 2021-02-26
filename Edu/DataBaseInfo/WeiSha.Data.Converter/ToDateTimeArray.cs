using System;
namespace DataBaseInfo.Converter
{
	public class ToDateTimeArray : ToArray
	{
		private static Type mValueType = typeof(DateTime);
		protected override Type ValueType
		{
			get
			{
				return ToDateTimeArray.mValueType;
			}
		}
	}
}
