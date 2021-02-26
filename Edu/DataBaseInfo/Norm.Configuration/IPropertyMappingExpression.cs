using System;
namespace Norm.Configuration
{
	public interface IPropertyMappingExpression : IHideObjectMembers
	{
		string SourcePropertyName
		{
			get;
			set;
		}
		void UseAlias(string alias);
	}
}
