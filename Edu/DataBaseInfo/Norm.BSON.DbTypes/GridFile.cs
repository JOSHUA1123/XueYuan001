using Norm.Collections;
using Norm.Configuration;
using System;
using System.Collections.Generic;
namespace Norm.BSON.DbTypes
{
	public class GridFile
	{
		protected class FileChunk
		{
			public ObjectId ID
			{
				get;
				set;
			}
			public int SequenceID
			{
				get;
				set;
			}
			public ObjectId FileID
			{
				get;
				set;
			}
			public byte[] Payload
			{
				get;
				set;
			}
			static FileChunk()
			{
				MongoConfiguration.Initialize(delegate(IConfigurationContainer cfg)
				{
					cfg.For<GridFile.FileChunk>(delegate(ITypeConfiguration<GridFile.FileChunk> j)
					{
						j.UseCollectionNamed("chunks");
						j.ForProperty((GridFile.FileChunk k) => (object)k.SequenceID).UseAlias("n");
						j.ForProperty((GridFile.FileChunk k) => k.FileID).UseAlias("files_id");
						j.ForProperty((GridFile.FileChunk k) => k.Payload).UseAlias("data");
					});
				});
			}
		}
		protected class FileMetadata
		{
			public ObjectId ID
			{
				get;
				set;
			}
			public string FileName
			{
				get;
				set;
			}
			public string ContentType
			{
				get;
				set;
			}
			public long Length
			{
				get;
				set;
			}
			public int ChunkSize
			{
				get;
				set;
			}
			public DateTime UploadDate
			{
				get;
				set;
			}
			public List<string> Aliases
			{
				get;
				set;
			}
			public object MetaData
			{
				get;
				set;
			}
			public string MD5Checksum
			{
				get;
				set;
			}
			static FileMetadata()
			{
				MongoConfiguration.Initialize(delegate(IConfigurationContainer cfg)
				{
					cfg.For<GridFile.FileMetadata>(delegate(ITypeConfiguration<GridFile.FileMetadata> f)
					{
						f.UseCollectionNamed("files");
						f.ForProperty((GridFile.FileMetadata j) => j.FileName).UseAlias("filename");
						f.ForProperty((GridFile.FileMetadata j) => j.ContentType).UseAlias("contentType");
						f.ForProperty((GridFile.FileMetadata j) => (object)j.Length).UseAlias("length");
						f.ForProperty((GridFile.FileMetadata j) => (object)j.ChunkSize).UseAlias("chunkSize");
						f.ForProperty((GridFile.FileMetadata j) => (object)j.UploadDate).UseAlias("uploadDate");
						f.ForProperty((GridFile.FileMetadata j) => j.Aliases).UseAlias("aliases");
						f.ForProperty((GridFile.FileMetadata j) => j.MetaData).UseAlias("metadata");
						f.ForProperty((GridFile.FileMetadata j) => j.MD5Checksum).UseAlias("md5");
					});
				});
			}
		}
		public static GridFile OpenFile(Mongo db, ObjectId fileKey)
		{
			return GridFile.OpenFile((IMongoCollection)db.GetCollection<object>("fs"), fileKey);
		}
		public static GridFile OpenFile(IMongoCollection collection, ObjectId fileKey)
		{
			return new GridFile(collection, fileKey);
		}
		public static GridFile CreateFile(Mongo db)
		{
			return new GridFile((IMongoCollection)db.GetCollection<object>("fs"));
		}
		private GridFile(IMongoCollection collection, ObjectId fileKey)
		{
		}
		private GridFile(IMongoCollection collection)
		{
		}
		public void Save()
		{
		}
	}
}
