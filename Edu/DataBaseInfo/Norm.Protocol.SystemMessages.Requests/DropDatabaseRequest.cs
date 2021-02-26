using Norm.Configuration;
using System;
namespace Norm.Protocol.SystemMessages.Requests
{
	internal class DropDatabaseRequest : ISystemQuery
	{
		public double DropDatabase
		{
			get
			{
				return 1.0;
			}
		}
		static DropDatabaseRequest()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<DropDatabaseRequest>(delegate(ITypeConfiguration<DropDatabaseRequest> a)
				{
					a.ForProperty((DropDatabaseRequest auth) => (object)auth.DropDatabase).UseAlias("dropDatabase");
				});
			});
		}
	}
}
