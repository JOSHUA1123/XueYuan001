using System;
namespace DataBaseInfo.Converter
{
	public class ToGuidArray : ToArray
	{
		private static Type mValueType = typeof(Guid);
		protected override Type ValueType
		{
			get
			{
				return ToGuidArray.mValueType;
			}
		}
	}
}
