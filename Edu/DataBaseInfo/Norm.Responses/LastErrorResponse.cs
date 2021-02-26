using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class LastErrorResponse : BaseStatusMessage
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
		public int Code
		{
			get;
			set;
		}
		static LastErrorResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<LastErrorResponse>(delegate(ITypeConfiguration<LastErrorResponse> a)
				{
					a.ForProperty((LastErrorResponse auth) => (object)auth.NumberOfErrors).UseAlias("n");
					a.ForProperty((LastErrorResponse auth) => auth.Error).UseAlias("err");
					a.ForProperty((LastErrorResponse auth) => (object)auth.Code).UseAlias("code");
				});
			});
		}
	}
}
