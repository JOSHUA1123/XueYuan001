using System;
using System.Linq.Expressions;
namespace Norm.Configuration
{
	public interface ITypeConfiguration
	{
		void UseCollectionNamed(string collectionName);
		void UseConnectionString(string connectionString);
		void UseAsDiscriminator();
	}
	public interface ITypeConfiguration<T> : ITypeConfiguration
	{
		IPropertyMappingExpression ForProperty(Expression<Func<T, object>> sourcePropery);
		void IdIs(Expression<Func<T, object>> idProperty);
	}
}
