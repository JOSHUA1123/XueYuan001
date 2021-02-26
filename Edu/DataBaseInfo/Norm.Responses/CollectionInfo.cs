using Norm.Collections;
using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class CollectionInfo
	{
		public string Name
		{
			get;
			set;
		}
		public CreateCollectionOptions Options
		{
			get;
			set;
		}
		static CollectionInfo()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<CollectionInfo>(delegate(ITypeConfiguration<CollectionInfo> a)
				{
					a.ForProperty((CollectionInfo auth) => auth.Name).UseAlias("name");
				});
			});
		}
	}
}
