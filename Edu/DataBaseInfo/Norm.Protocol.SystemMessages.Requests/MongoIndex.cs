using Norm.BSON;
using Norm.Configuration;
using System;
namespace Norm.Protocol.SystemMessages.Requests
{
	public class MongoIndex : IUpdateWithoutId, ISystemQuery
	{
		public Expando Key
		{
			get;
			set;
		}
		public string Namespace
		{
			get;
			set;
		}
		public bool Unique
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		static MongoIndex()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<MongoIndex>(delegate(ITypeConfiguration<MongoIndex> a)
				{
					a.ForProperty((MongoIndex auth) => auth.Key).UseAlias("key");
					a.ForProperty((MongoIndex auth) => auth.Namespace).UseAlias("ns");
					a.ForProperty((MongoIndex auth) => (object)auth.Unique).UseAlias("unique");
					a.ForProperty((MongoIndex auth) => auth.Name).UseAlias("name");
				});
			});
		}
	}
}
