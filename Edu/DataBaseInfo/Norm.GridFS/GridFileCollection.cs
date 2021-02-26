using Norm.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Norm.GridFS
{
	public class GridFileCollection
	{
		private IMongoCollection<FileChunk> FileChunks
		{
			get;
			set;
		}
		private IMongoCollection<GridFile> FileSummaries
		{
			get;
			set;
		}
		internal GridFileCollection(IMongoCollection<GridFile> fileSummaries, IMongoCollection<FileChunk> fileChunks)
		{
			this.FileChunks = fileChunks;
			this.FileSummaries = fileSummaries;
		}
		public void Save(GridFile file)
		{
			this.FileSummaries.Save(file);
			this.FileChunks.Delete(new
			{
				_id = file.Id
			});
			if (file.CachedChunks.Any<FileChunk>())
			{
				this.FileChunks.Insert(file.CachedChunks);
			}
		}
		public GridFile FindOne<U>(U template)
		{
			GridFile retval = this.FileSummaries.FindOne<U>(template);
			if (retval != null)
			{
				retval.Chunks = 
					from y in this.FileChunks.AsQueryable()
					where y.FileID == retval.Id
					select y into j
					orderby j.ChunkNumber
					select j;
			}
			return retval;
		}
		public IEnumerable<GridFile> Find<U>(U template)
		{
			foreach (GridFile f in this.FileSummaries.Find(template))
			{
				f.Chunks = 
					from y in this.FileChunks.AsQueryable()
					where y.FileID == f.Id
					select y into j
					orderby j.ChunkNumber
					select j;
				yield return f;
			}
			yield break;
		}
		public void Delete(ObjectId IDofFileToDelete)
		{
			if (IDofFileToDelete != null)
			{
				this.FileSummaries.Delete(new
				{
					_id = IDofFileToDelete
				});
				this.FileChunks.Delete(new
				{
					_id = IDofFileToDelete
				});
				return;
			}
			this.FileSummaries.Delete(new
			{

			});
			this.FileChunks.Delete(new
			{

			});
		}
	}
}
