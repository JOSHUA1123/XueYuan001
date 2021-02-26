using System;
namespace DataBaseInfo.Converter
{
	public class ToSbyteArray : ToArray
	{
		private static Type mValueType = typeof(sbyte);
		protected override Type ValueType
		{
			get
			{
				return ToSbyteArray.mValueType;
			}
		}
	}
}
