using System;
namespace DataBaseInfo.Converter
{
	public abstract class ToArray : IStringConverter
	{
		protected abstract Type ValueType
		{
			get;
		}
		protected object ConverItem(string value, out bool succeeded)
		{
			succeeded = ConverterFactory.Converters.ContainsKey(this.ValueType);
			if (!succeeded)
			{
				return null;
			}
			IStringConverter stringConverter = ConverterFactory.Converters[this.ValueType];
			return stringConverter.ConvertTo(value, out succeeded);
		}
		object IStringConverter.ConvertTo(string value, out bool succeeded)
		{
			succeeded = (value != null && value != string.Empty);
			if (!succeeded)
			{
				return null;
			}
			string[] array = value.Split(new char[]
			{
				','
			});
			Array array2 = Array.CreateInstance(this.ValueType, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				object value2 = this.ConverItem(array[i], out succeeded);
				if (!succeeded)
				{
					return null;
				}
				array2.SetValue(value2, i);
			}
			return array2;
		}
	}
}
