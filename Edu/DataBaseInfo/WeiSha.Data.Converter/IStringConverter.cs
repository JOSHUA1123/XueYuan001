using System;
namespace DataBaseInfo.Converter
{
	public interface IStringConverter
	{
		object ConvertTo(string value, out bool succeeded);
	}
}
