using Norm.Attributes;
using Norm.Configuration;
using System;
namespace Norm.Protocol.Messages
{
	public class MapReduceMessage
	{
		public string MapReduce
		{
			get;
			set;
		}
		public string Map
		{
			get;
			set;
		}
		public string Reduce
		{
			get;
			set;
		}
		public bool KeepTemp
		{
			get;
			set;
		}
		public string Out
		{
			get;
			set;
		}
		public int? Limit
		{
			get;
			set;
		}
		public object Query
		{
			get;
			set;
		}
		[MongoIgnoreIfNull]
		public new string Finalize
		{
			get;
			set;
		}
		static MapReduceMessage()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => m.MapReduce).UseAlias("mapreduce");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => m.Map).UseAlias("map");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => m.Reduce).UseAlias("reduce");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => (object)m.KeepTemp).UseAlias("keeptemp");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => m.Out).UseAlias("out");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => (object)m.Limit).UseAlias("limit");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => m.Finalize).UseAlias("finalize");
				});
				c.For<MapReduceMessage>(delegate(ITypeConfiguration<MapReduceMessage> mrm)
				{
					mrm.ForProperty((MapReduceMessage m) => m.Query).UseAlias("query");
				});
			});
		}
	}
}
