using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class BuildInfoResponse : BaseStatusMessage
	{
		public string Version
		{
			get;
			set;
		}
		public string GitVersion
		{
			get;
			set;
		}
		public string SystemInformation
		{
			get;
			set;
		}
		public int? Bits
		{
			get;
			set;
		}
		static BuildInfoResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<BuildInfoResponse>(delegate(ITypeConfiguration<BuildInfoResponse> a)
				{
					a.ForProperty((BuildInfoResponse auth) => auth.Version).UseAlias("version");
					a.ForProperty((BuildInfoResponse auth) => auth.GitVersion).UseAlias("gitVersion");
					a.ForProperty((BuildInfoResponse auth) => auth.SystemInformation).UseAlias("sysInfo");
				});
			});
		}
	}
}
