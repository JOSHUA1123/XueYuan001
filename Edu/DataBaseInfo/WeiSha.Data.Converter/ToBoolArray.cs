using System;
namespace DataBaseInfo.Converter
{
	public class ToBoolArray : ToArray
	{
		private static Type mValueType = typeof(bool);
		protected override Type ValueType
		{
			get
			{
				return ToBoolArray.mValueType;
			}
		}
	}
}
