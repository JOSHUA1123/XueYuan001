using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class AssertInfoResponse : BaseStatusMessage
	{
		public bool? DatabaseAsserted
		{
			get;
			set;
		}
		public bool? Asserted
		{
			get;
			set;
		}
		public string Assert
		{
			get;
			set;
		}
		public string WarningAssert
		{
			get;
			set;
		}
		public string AssertMessage
		{
			get;
			set;
		}
		public string AssertUser
		{
			get;
			set;
		}
		static AssertInfoResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<AssertInfoResponse>(delegate(ITypeConfiguration<AssertInfoResponse> a)
				{
					a.ForProperty((AssertInfoResponse auth) => (object)auth.DatabaseAsserted).UseAlias("dbasserted");
					a.ForProperty((AssertInfoResponse auth) => (object)auth.Asserted).UseAlias("asserted");
					a.ForProperty((AssertInfoResponse auth) => auth.Assert).UseAlias("assert");
					a.ForProperty((AssertInfoResponse auth) => auth.WarningAssert).UseAlias("assertw");
					a.ForProperty((AssertInfoResponse auth) => auth.AssertMessage).UseAlias("assertmsg");
					a.ForProperty((AssertInfoResponse auth) => auth.AssertUser).UseAlias("assertuser");
				});
			});
		}
	}
}
