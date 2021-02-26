using System;
namespace DataBaseInfo.Converter
{
	public class ToCharArray : ToArray
	{
		private static Type mValueType = typeof(char);
		protected override Type ValueType
		{
			get
			{
				return ToCharArray.mValueType;
			}
		}
	}
}
