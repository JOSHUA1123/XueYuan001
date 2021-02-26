using System;
namespace Norm.Linq
{
	public interface IMongoQueryResults
	{
		QueryTranslationResults TranslationResults
		{
			get;
		}
	}
}
