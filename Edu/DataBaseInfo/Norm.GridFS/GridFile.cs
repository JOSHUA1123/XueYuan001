using Norm.Attributes;
using Norm.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
namespace Norm.GridFS
{
	public class GridFile
	{
		private List<FileChunk> _cachedChunks;
		private int Length
		{
			get;
			set;
		}
		private int ChunkSize
		{
			get;
			set;
		}
		[MongoIgnore]
		internal IQueryable<FileChunk> Chunks
		{
			get;
			set;
		}
		[MongoIgnore]
		internal List<FileChunk> CachedChunks
		{
			get
			{
				if (this._cachedChunks == null)
				{
					if (this.Chunks != null)
					{
						this._cachedChunks = this.Chunks.ToList<FileChunk>();
					}
					else
					{
						this._cachedChunks = new List<FileChunk>(0);
					}
				}
				return this._cachedChunks;
			}
			private set
			{
				this._cachedChunks = value.ToList<FileChunk>();
			}
		}
		public ObjectId Id
		{
			get;
			set;
		}
		public DateTime UploadDate
		{
			get;
			set;
		}
		public string MD5Checksum
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
		public IEnumerable<string> Aliases
		{
			get;
			set;
		}
		[MongoIgnore]
		public IEnumerable<byte> Content
		{
			get
			{
				return this.CachedChunks.SelectMany((FileChunk y) => y.BinaryData);
			}
			set
			{
				MD5 mD = MD5.Create();
				this.CachedChunks.Clear();
				int num = 0;
				int num2 = 0;
				int num3;
				do
				{
					byte[] array = value.Skip(num).Take(this.ChunkSize).ToArray<byte>();
					num3 = array.Length;
					num += num3;
					if (num3 > 0)
					{
						FileChunk fileChunk = new FileChunk();
						fileChunk.ChunkNumber = num2;
						fileChunk.FileID = this.Id;
						fileChunk.BinaryData = array;
						num2++;
					}
				}
				while (num3 > 0);
				this.Length = num;
				byte[] value2 = mD.ComputeHash(this.CachedChunks.SelectMany((FileChunk y) => y.BinaryData).ToArray<byte>());
				this.MD5Checksum = BitConverter.ToString(value2).Replace("-", "");
			}
		}
		static GridFile()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer container)
			{
				container.For<GridFile>(delegate(ITypeConfiguration<GridFile> y)
				{
					y.ForProperty((GridFile j) => (object)j.Length).UseAlias("length");
					y.ForProperty((GridFile j) => (object)j.ChunkSize).UseAlias("chunkSize");
					y.ForProperty((GridFile j) => (object)j.UploadDate).UseAlias("uploadDate");
					y.ForProperty((GridFile j) => j.MD5Checksum).UseAlias("md5");
					y.ForProperty((GridFile j) => j.FileName).UseAlias("filename");
					y.ForProperty((GridFile j) => j.ContentType).UseAlias("contentType");
					y.ForProperty((GridFile j) => j.Aliases).UseAlias("aliases");
				});
			});
		}
		public GridFile()
		{
			this.UploadDate = DateTime.Now.ToUniversalTime();
			this.ChunkSize = 262144;
			this.Id = ObjectId.NewObjectId();
		}
	}
}
