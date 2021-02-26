using System;
namespace Norm.Configuration
{
	public class PropertyMappingExpression : IPropertyMappingExpression, IHideObjectMembers
	{
		internal string Alias
		{
			get;
			set;
		}
		internal bool IsId
		{
			get;
			set;
		}
		public string SourcePropertyName
		{
			get;
			set;
		}
		public void UseAlias(string alias)
		{
			this.Alias = alias;
		}
		Type IHideObjectMembers.GetType()
		{
			return base.GetType();
		}
	}
}
