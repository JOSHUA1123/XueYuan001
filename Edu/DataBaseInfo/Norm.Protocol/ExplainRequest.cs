using Norm.Configuration;
using Norm.Protocol.SystemMessages;
using System;
namespace Norm.Protocol
{
	public class ExplainRequest<T> : ISystemQuery
	{
		private bool Explain
		{
			get
			{
				return true;
			}
		}
		public T Query
		{
			get;
			protected set;
		}
		static ExplainRequest()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer cfg)
			{
				cfg.For<ExplainRequest<T>>(delegate(ITypeConfiguration<ExplainRequest<T>> y)
				{
					y.ForProperty((ExplainRequest<T> c) => (object)c.Explain).UseAlias("$explain");
					y.ForProperty((ExplainRequest<T> c) => (object)c.Query).UseAlias("query");
				});
			});
		}
		public ExplainRequest(T query)
		{
			this.Query = query;
		}
	}
}
