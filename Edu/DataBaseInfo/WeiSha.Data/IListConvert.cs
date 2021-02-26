using System;
namespace DataBaseInfo
{
	internal interface IListConvert<T>
	{
		SourceList<TOutput> ConvertTo<TOutput>();
		SourceList<IOutput> ConvertTo<TOutput, IOutput>() where TOutput : IOutput;
		SourceList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter);
	}
}
