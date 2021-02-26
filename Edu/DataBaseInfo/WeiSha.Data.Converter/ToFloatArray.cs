using System;
namespace DataBaseInfo.Converter
{
	public class ToFloatArray : ToArray
	{
		private static Type mValueType = typeof(float);
		protected override Type ValueType
		{
			get
			{
				return ToFloatArray.mValueType;
			}
		}
	}
}
