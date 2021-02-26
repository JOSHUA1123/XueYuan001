using Norm.Configuration;
using System;
namespace Norm.Protocol.SystemMessages.Requests
{
	internal class CollectionStatisticsRequest : ISystemQuery
	{
		public string CollectionStatistics
		{
			get;
			set;
		}
		static CollectionStatisticsRequest()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<CollectionStatisticsRequest>(delegate(ITypeConfiguration<CollectionStatisticsRequest> a)
				{
					a.ForProperty((CollectionStatisticsRequest auth) => auth.CollectionStatistics).UseAlias("collstats");
				});
			});
		}
	}
}
