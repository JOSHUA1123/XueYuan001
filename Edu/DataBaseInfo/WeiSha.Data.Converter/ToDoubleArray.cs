using System;
namespace DataBaseInfo.Converter
{
	public class ToDoubleArray : ToArray
	{
		private static Type mValueType = typeof(double);
		protected override Type ValueType
		{
			get
			{
				return ToDoubleArray.mValueType;
			}
		}
	}
}
