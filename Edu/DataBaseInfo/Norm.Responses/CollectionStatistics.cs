using Norm.Configuration;
using System;
using System.Collections.Generic;
namespace Norm.Responses
{
	public class CollectionStatistics : BaseStatusMessage
	{
		public long? CurrentExtents
		{
			get;
			set;
		}
		public long? TotalIndexSize
		{
			get;
			set;
		}
		public long? LastIndexSize
		{
			get;
			set;
		}
		public Dictionary<string, double> IndexSizes
		{
			get;
			set;
		}
		public long? PreviousExtentSize
		{
			get;
			set;
		}
		public long? Flags
		{
			get;
			set;
		}
		public double? PaddingFactor
		{
			get;
			set;
		}
		public string Namespace
		{
			get;
			set;
		}
		public long? Count
		{
			get;
			set;
		}
		public long? Size
		{
			get;
			set;
		}
		public long? StorageSize
		{
			get;
			set;
		}
		public int? NumberOfIndices
		{
			get;
			set;
		}
		static CollectionStatistics()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<CollectionStatistics>(delegate(ITypeConfiguration<CollectionStatistics> a)
				{
					a.ForProperty((CollectionStatistics stat) => stat.Namespace).UseAlias("ns");
					a.ForProperty((CollectionStatistics stat) => (object)stat.Count).UseAlias("count");
					a.ForProperty((CollectionStatistics stat) => (object)stat.Size).UseAlias("size");
					a.ForProperty((CollectionStatistics stat) => (object)stat.StorageSize).UseAlias("storageSize");
					a.ForProperty((CollectionStatistics stat) => (object)stat.NumberOfIndices).UseAlias("nIndexes");
					a.ForProperty((CollectionStatistics stat) => (object)stat.PaddingFactor).UseAlias("paddingFactor");
					a.ForProperty((CollectionStatistics stat) => (object)stat.CurrentExtents).UseAlias("numExtents");
					a.ForProperty((CollectionStatistics stat) => (object)stat.PreviousExtentSize).UseAlias("lastExtentSize");
					a.ForProperty((CollectionStatistics stat) => (object)stat.Flags).UseAlias("flags");
					a.ForProperty((CollectionStatistics stat) => (object)stat.LastIndexSize).UseAlias("lIndexSize");
					a.ForProperty((CollectionStatistics stat) => stat.IndexSizes).UseAlias("indexSizes");
					a.ForProperty((CollectionStatistics stat) => (object)stat.TotalIndexSize).UseAlias("totalIndexSize");
				});
			});
		}
	}
}
