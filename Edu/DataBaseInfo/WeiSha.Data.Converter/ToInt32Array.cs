using System;
namespace DataBaseInfo.Converter
{
	public class ToInt32Array : ToArray
	{
		private static Type mValueType = typeof(int);
		protected override Type ValueType
		{
			get
			{
				return ToInt32Array.mValueType;
			}
		}
	}
}
