using Norm.BSON;
using System;
namespace Norm.Configuration
{
	public interface IMongoConfigurationMap : IHideObjectMembers
	{
		void For<T>(Action<ITypeConfiguration<T>> typeConfiguration);
		void RemoveFor<T>();
		void TypeConverterFor<TClr, TCnv>() where TCnv : IBsonTypeConverter, new();
		IBsonTypeConverter GetTypeConverterFor(Type t);
		void RemoveTypeConverterFor<TClr>();
		string GetCollectionName(Type type);
		string GetConnectionString(Type type);
		string GetPropertyAlias(Type type, string propertyName);
		string GetTypeDescriminator(Type type);
	}
}
