using Norm.Attributes;
using System;
namespace Norm.Collections
{
	public class CreateCollectionOptions
	{
		public string Name
		{
			get;
			set;
		}
		public bool Capped
		{
			get;
			set;
		}
		[MongoIgnoreIfNull]
		public int? Size
		{
			get;
			set;
		}
		[MongoIgnoreIfNull]
		public long? Max
		{
			get;
			set;
		}
		public bool AutoIndexId
		{
			get;
			set;
		}
		public string Create
		{
			get;
			set;
		}
		public CreateCollectionOptions()
		{
		}
		public CreateCollectionOptions(string name)
		{
			this.Name = name;
		}
	}
}
