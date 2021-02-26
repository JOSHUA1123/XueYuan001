using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class ProfileLevelResponse : BaseStatusMessage
	{
		public int PreviousLevel
		{
			get;
			set;
		}
		public int? SlowOpThreshold
		{
			get;
			set;
		}
		static ProfileLevelResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<ProfileLevelResponse>(delegate(ITypeConfiguration<ProfileLevelResponse> a)
				{
					a.ForProperty((ProfileLevelResponse p) => (object)p.PreviousLevel).UseAlias("was");
					a.ForProperty((ProfileLevelResponse p) => (object)p.SlowOpThreshold).UseAlias("slowms");
				});
			});
		}
	}
}
