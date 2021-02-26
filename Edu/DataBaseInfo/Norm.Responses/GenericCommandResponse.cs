using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class GenericCommandResponse : BaseStatusMessage
	{
		public string Info
		{
			get;
			set;
		}
		static GenericCommandResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<GenericCommandResponse>(delegate(ITypeConfiguration<GenericCommandResponse> a)
				{
					a.ForProperty((GenericCommandResponse auth) => auth.Info).UseAlias("info");
				});
			});
		}
	}
}
