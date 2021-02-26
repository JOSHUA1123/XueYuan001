using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class OpCountersResponse
	{
		public int? Insertions
		{
			get;
			set;
		}
		public int? Queries
		{
			get;
			set;
		}
		public int? Updates
		{
			get;
			set;
		}
		public int? Deletions
		{
			get;
			set;
		}
		public int? Pages
		{
			get;
			set;
		}
		public int? Commands
		{
			get;
			set;
		}
		static OpCountersResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<OpCountersResponse>(delegate(ITypeConfiguration<OpCountersResponse> a)
				{
					a.ForProperty((OpCountersResponse op) => (object)op.Insertions).UseAlias("insert");
					a.ForProperty((OpCountersResponse op) => (object)op.Queries).UseAlias("query");
					a.ForProperty((OpCountersResponse op) => (object)op.Updates).UseAlias("update");
					a.ForProperty((OpCountersResponse op) => (object)op.Deletions).UseAlias("delete");
					a.ForProperty((OpCountersResponse op) => (object)op.Pages).UseAlias("getmore");
					a.ForProperty((OpCountersResponse op) => (object)op.Commands).UseAlias("command");
				});
			});
		}
	}
}
