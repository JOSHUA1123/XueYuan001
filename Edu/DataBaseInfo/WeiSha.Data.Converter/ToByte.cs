using System;
namespace DataBaseInfo.Converter
{
	public class ToByte : IStringConverter
	{
		object IStringConverter.ConvertTo(string value, out bool succeeded)
		{
			byte b;
			succeeded = byte.TryParse(value, out b);
			return b;
		}
	}
}
