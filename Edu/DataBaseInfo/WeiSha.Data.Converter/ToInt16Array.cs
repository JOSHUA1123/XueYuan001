using System;
namespace DataBaseInfo.Converter
{
	public class ToInt16Array : ToArray
	{
		private static Type mValueType = typeof(short);
		protected override Type ValueType
		{
			get
			{
				return ToInt16Array.mValueType;
			}
		}
	}
}
