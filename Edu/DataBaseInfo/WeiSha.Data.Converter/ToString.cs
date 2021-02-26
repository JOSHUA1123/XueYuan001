using System;
namespace DataBaseInfo.Converter
{
	public class ToString : IStringConverter
	{
		object IStringConverter.ConvertTo(string value, out bool succeeded)
		{
			succeeded = true;
			return value;
		}
	}
}