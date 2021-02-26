using System;
namespace DataBaseInfo.Converter
{
	public class ToByteArray : ToArray
	{
		private static Type mValueType = typeof(byte[]);
		protected override Type ValueType
		{
			get
			{
				return ToByteArray.mValueType;
			}
		}
	}
}
