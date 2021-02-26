using Norm.Configuration;
using System;
namespace Norm.Responses
{
	internal class DeleteIndicesResponse : BaseStatusMessage
	{
		public int? NumberIndexesWas
		{
			get;
			set;
		}
		public string Message
		{
			get;
			set;
		}
		public string Namespace
		{
			get;
			set;
		}
		static DeleteIndicesResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<DeleteIndicesResponse>(delegate(ITypeConfiguration<DeleteIndicesResponse> a)
				{
					a.ForProperty((DeleteIndicesResponse auth) => (object)auth.NumberIndexesWas).UseAlias("nIndexesWas");
					a.ForProperty((DeleteIndicesResponse auth) => auth.Message).UseAlias("msg");
					a.ForProperty((DeleteIndicesResponse auth) => auth.Namespace).UseAlias("ns ");
				});
			});
		}
	}
}
