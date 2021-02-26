using System;
namespace DataBaseInfo.Converter
{
	public class ToUInt16Array : ToArray
	{
		private static Type mValueType = typeof(ushort);
		protected override Type ValueType
		{
			get
			{
				return ToUInt16Array.mValueType;
			}
		}
	}
}
