using Norm.Configuration;
using System;
namespace Norm.Responses
{
	internal class GetNonceResponse : BaseStatusMessage
	{
		public string Nonce
		{
			get;
			set;
		}
		static GetNonceResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<GetNonceResponse>(delegate(ITypeConfiguration<GetNonceResponse> a)
				{
					a.ForProperty((GetNonceResponse auth) => auth.Nonce).UseAlias("nonce");
				});
			});
		}
	}
}
