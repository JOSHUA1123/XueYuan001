using System;
using System.Collections.Generic;
namespace DataBaseInfo.Converter
{
	public static class ConverterFactory
	{
		private static IDictionary<Type, IStringConverter> mConverters;
		public static IDictionary<Type, IStringConverter> Converters
		{
			get
			{
				if (ConverterFactory.mConverters == null)
				{
					lock (typeof(ConverterFactory))
					{
						ConverterFactory.OnInit();
					}
				}
				return ConverterFactory.mConverters;
			}
		}
		private static void OnInit()
		{
			if (ConverterFactory.mConverters == null)
			{
				ConverterFactory.mConverters = new Dictionary<Type, IStringConverter>();
				ConverterFactory.mConverters.Add(typeof(byte), new ToByte());
				ConverterFactory.mConverters.Add(typeof(byte[]), new ToByteArray());
				ConverterFactory.mConverters.Add(typeof(sbyte), new ToSbyte());
				ConverterFactory.mConverters.Add(typeof(sbyte[]), new ToSbyteArray());
				ConverterFactory.mConverters.Add(typeof(short), new ToInt16());
				ConverterFactory.mConverters.Add(typeof(short[]), new ToInt16Array());
				ConverterFactory.mConverters.Add(typeof(ushort), new ToUInt16());
				ConverterFactory.mConverters.Add(typeof(ushort[]), new ToUInt16Array());
				ConverterFactory.mConverters.Add(typeof(int), new ToInt32());
				ConverterFactory.mConverters.Add(typeof(int[]), new ToInt32Array());
				ConverterFactory.mConverters.Add(typeof(uint), new ToUInt23());
				ConverterFactory.mConverters.Add(typeof(uint[]), new ToUInt16Array());
				ConverterFactory.mConverters.Add(typeof(long), new ToLong());
				ConverterFactory.mConverters.Add(typeof(long[]), new ToLongArray());
				ConverterFactory.mConverters.Add(typeof(ulong), new ToULong());
				ConverterFactory.mConverters.Add(typeof(ulong[]), new ToULongArray());
				ConverterFactory.mConverters.Add(typeof(char), new ToChar());
				ConverterFactory.mConverters.Add(typeof(char[]), new ToCharArray());
				ConverterFactory.mConverters.Add(typeof(Guid), new ToGuid());
				ConverterFactory.mConverters.Add(typeof(Guid[]), new ToGuidArray());
				ConverterFactory.mConverters.Add(typeof(DateTime), new ToDateTime());
				ConverterFactory.mConverters.Add(typeof(DateTime[]), new ToDateTimeArray());
				ConverterFactory.mConverters.Add(typeof(decimal), new ToDecimal());
				ConverterFactory.mConverters.Add(typeof(decimal[]), new ToDecimalArray());
				ConverterFactory.mConverters.Add(typeof(float), new ToFloat());
				ConverterFactory.mConverters.Add(typeof(float[]), new ToFloatArray());
				ConverterFactory.mConverters.Add(typeof(double), new ToDouble());
				ConverterFactory.mConverters.Add(typeof(double[]), new ToDoubleArray());
				ConverterFactory.mConverters.Add(typeof(string), new ToString());
				ConverterFactory.mConverters.Add(typeof(string[]), new ToStringArray());
				ConverterFactory.mConverters.Add(typeof(bool), new ToBool());
				ConverterFactory.mConverters.Add(typeof(bool[]), new ToBoolArray());
			}
		}
	}
}
