using Norm.Configuration;
using System;
namespace Norm.Protocol.SystemMessages.Requests
{
	public class ListDatabasesRequest : ISystemQuery
	{
		public bool ListDatabases
		{
			get
			{
				return true;
			}
		}
		static ListDatabasesRequest()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<ListDatabasesRequest>(delegate(ITypeConfiguration<ListDatabasesRequest> a)
				{
					a.ForProperty((ListDatabasesRequest auth) => (object)auth.ListDatabases).UseAlias("listDatabases");
				});
			});
		}
	}
}
