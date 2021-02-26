using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class BackgroundFlushingResponse
	{
		public int? Flushes
		{
			get;
			set;
		}
		public int? TotalMilliseconds
		{
			get;
			set;
		}
		public int? AverageMilliseconds
		{
			get;
			set;
		}
		public int? LastMilliseconds
		{
			get;
			set;
		}
		public double? LastFinished
		{
			get;
			set;
		}
		static BackgroundFlushingResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<BackgroundFlushingResponse>(delegate(ITypeConfiguration<BackgroundFlushingResponse> a)
				{
					a.ForProperty((BackgroundFlushingResponse op) => (object)op.Flushes).UseAlias("flushes");
					a.ForProperty((BackgroundFlushingResponse op) => (object)op.TotalMilliseconds).UseAlias("total_ms");
					a.ForProperty((BackgroundFlushingResponse op) => (object)op.AverageMilliseconds).UseAlias("average_ms");
					a.ForProperty((BackgroundFlushingResponse op) => (object)op.LastMilliseconds).UseAlias("last_ms");
					a.ForProperty((BackgroundFlushingResponse op) => (object)op.LastFinished).UseAlias("last_finished");
				});
			});
		}
	}
}
