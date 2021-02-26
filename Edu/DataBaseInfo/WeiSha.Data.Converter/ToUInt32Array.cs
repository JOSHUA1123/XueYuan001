using System;
namespace DataBaseInfo.Converter
{
	public class ToUInt32Array : ToArray
	{
		private static Type mValueType = typeof(uint);
		protected override Type ValueType
		{
			get
			{
				return ToUInt32Array.mValueType;
			}
		}
	}
}
