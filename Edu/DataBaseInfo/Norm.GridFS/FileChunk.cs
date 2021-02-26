using Norm.Configuration;
using System;
namespace Norm.GridFS
{
	internal class FileChunk
	{
		public ObjectId Id
		{
			get;
			set;
		}
		public ObjectId FileID
		{
			get;
			set;
		}
		public int ChunkNumber
		{
			get;
			set;
		}
		public byte[] BinaryData
		{
			get;
			set;
		}
		static FileChunk()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer container)
			{
				container.For<FileChunk>(delegate(ITypeConfiguration<FileChunk> y)
				{
					y.ForProperty((FileChunk j) => j.FileID).UseAlias("files_id");
					y.ForProperty((FileChunk j) => (object)j.ChunkNumber).UseAlias("n");
					y.ForProperty((FileChunk j) => j.BinaryData).UseAlias("data");
				});
			});
		}
		public FileChunk()
		{
			this.Id = ObjectId.NewObjectId();
		}
	}
}
