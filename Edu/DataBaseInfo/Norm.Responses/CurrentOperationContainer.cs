using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class CurrentOperationContainer
	{
		public CurrentOperationResponse[] Responses
		{
			get;
			set;
		}
		static CurrentOperationContainer()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<CurrentOperationContainer>(delegate(ITypeConfiguration<CurrentOperationContainer> a)
				{
					a.ForProperty((CurrentOperationContainer op) => op.Responses).UseAlias("inprog");
				});
			});
		}
	}
}
