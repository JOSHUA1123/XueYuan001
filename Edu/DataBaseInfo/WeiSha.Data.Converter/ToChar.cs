using System;
namespace DataBaseInfo.Converter
{
	public class ToChar : IStringConverter
	{
		public object ConvertTo(string value, out bool succeeded)
		{
			char c;
			succeeded = char.TryParse(value, out c);
			return c;
		}
	}
}
