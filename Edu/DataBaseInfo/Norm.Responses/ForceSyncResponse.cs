using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class ForceSyncResponse : BaseStatusMessage
	{
		public int? NumberOfFiles
		{
			get;
			set;
		}
		static ForceSyncResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<ForceSyncResponse>(delegate(ITypeConfiguration<ForceSyncResponse> a)
				{
					a.ForProperty((ForceSyncResponse auth) => (object)auth.NumberOfFiles).UseAlias("numFiles");
				});
			});
		}
	}
}
