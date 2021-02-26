using System;
namespace DataBaseInfo.Converter
{
	public class ToDateTime : IStringConverter
	{
		object IStringConverter.ConvertTo(string value, out bool succeeded)
		{
			DateTime dateTime;
			succeeded = DateTime.TryParse(value, out dateTime);
			return dateTime;
		}
	}
}
