using System;
namespace DataBaseInfo.Converter
{
	public class ToSbyte : IStringConverter
	{
		object IStringConverter.ConvertTo(string value, out bool succeeded)
		{
			sbyte b;
			succeeded = sbyte.TryParse(value, out b);
			return b;
		}
	}
}
