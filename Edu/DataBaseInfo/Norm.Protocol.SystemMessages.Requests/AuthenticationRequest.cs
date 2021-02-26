using Norm.Configuration;
using System;
namespace Norm.Protocol.SystemMessages.Requests
{
	internal class AuthenticationRequest : ISystemQuery
	{
		public int Authenticate
		{
			get
			{
				return 1;
			}
		}
		public string Nonce
		{
			get;
			set;
		}
		public string User
		{
			get;
			set;
		}
		public string Key
		{
			get;
			set;
		}
		static AuthenticationRequest()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<AuthenticationRequest>(delegate(ITypeConfiguration<AuthenticationRequest> a)
				{
					a.ForProperty((AuthenticationRequest auth) => (object)auth.Authenticate).UseAlias("authenticate");
					a.ForProperty((AuthenticationRequest auth) => auth.Nonce).UseAlias("nonce");
					a.ForProperty((AuthenticationRequest auth) => auth.User).UseAlias("user");
					a.ForProperty((AuthenticationRequest auth) => auth.Key).UseAlias("key");
				});
			});
		}
	}
}
