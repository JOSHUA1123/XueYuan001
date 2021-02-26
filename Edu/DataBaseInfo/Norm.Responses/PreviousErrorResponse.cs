using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class PreviousErrorResponse : BaseStatusMessage
	{
		public long? NumberOfErrors
		{
			get;
			set;
		}
		public string Error
		{
			get;
			set;
		}
		public long? NumberOfOperationsAgo
		{
			get;
			set;
		}
		static PreviousErrorResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<PreviousErrorResponse>(delegate(ITypeConfiguration<PreviousErrorResponse> a)
				{
					a.ForProperty((PreviousErrorResponse auth) => (object)auth.NumberOfErrors).UseAlias("n");
					a.ForProperty((PreviousErrorResponse auth) => auth.Error).UseAlias("err");
					a.ForProperty((PreviousErrorResponse auth) => (object)auth.NumberOfOperationsAgo).UseAlias("nPrev");
				});
			});
		}
	}
}
