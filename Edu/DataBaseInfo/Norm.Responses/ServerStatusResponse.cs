using Norm.Configuration;
using System;
namespace Norm.Responses
{
	public class ServerStatusResponse : BaseStatusMessage
	{
		public double? Uptime
		{
			get;
			set;
		}
		public double? localTime
		{
			get;
			set;
		}
		public GlobalLockResponse GlobalLock
		{
			get;
			set;
		}
		public MemoryResponse Mem
		{
			get;
			set;
		}
		public ConnectionsResponse Connections
		{
			get;
			set;
		}
		public ExtraInfoResponse Extrainfo
		{
			get;
			set;
		}
		public IndexCountersResponse IndexCounters
		{
			get;
			set;
		}
		public BackgroundFlushingResponse BackgroundFlushing
		{
			get;
			set;
		}
		public OpCountersResponse OpCounters
		{
			get;
			set;
		}
		public AssertsResponse Asserts
		{
			get;
			set;
		}
		public string Note
		{
			get;
			set;
		}
		static ServerStatusResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<ServerStatusResponse>(delegate(ITypeConfiguration<ServerStatusResponse> a)
				{
					a.ForProperty((ServerStatusResponse op) => op.GlobalLock).UseAlias("globalLock");
					a.ForProperty((ServerStatusResponse op) => op.Mem).UseAlias("mem");
					a.ForProperty((ServerStatusResponse op) => op.Connections).UseAlias("connections");
					a.ForProperty((ServerStatusResponse op) => op.Extrainfo).UseAlias("extra_info");
					a.ForProperty((ServerStatusResponse op) => op.IndexCounters).UseAlias("indexCounters");
					a.ForProperty((ServerStatusResponse op) => op.BackgroundFlushing).UseAlias("backgroundFlushing");
					a.ForProperty((ServerStatusResponse op) => op.OpCounters).UseAlias("opcounters");
					a.ForProperty((ServerStatusResponse op) => op.Asserts).UseAlias("asserts");
					a.ForProperty((ServerStatusResponse op) => op.Note).UseAlias("note");
				});
			});
		}
	}
}
