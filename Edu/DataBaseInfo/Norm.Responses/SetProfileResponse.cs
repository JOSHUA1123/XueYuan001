using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class SetProfileResponse : BaseStatusMessage
	{
		public int? Profile
		{
			get;
			set;
		}
		public int? SlowOpThreshold
		{
			get;
			set;
		}
		public int? PreviousLevel
		{
			get;
			set;
		}
		static SetProfileResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<SetProfileResponse>(delegate(ITypeConfiguration<SetProfileResponse> a)
				{
					a.ForProperty((SetProfileResponse p) => (object)p.PreviousLevel).UseAlias("was");
					a.ForProperty((SetProfileResponse p) => (object)p.Profile).UseAlias("profile");
					a.ForProperty((SetProfileResponse p) => (object)p.SlowOpThreshold).UseAlias("slowms");
				});
			});
		}
	}
}
