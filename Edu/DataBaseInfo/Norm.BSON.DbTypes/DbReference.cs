using Norm.Collections;
using Norm.Configuration;
using System;
namespace Norm.BSON.DbTypes
{
	public class DbReference<T, TId> : ObjectId where T : class, new()
	{
		public string Collection
		{
			get;
			set;
		}
		public TId Id
		{
			get;
			set;
		}
		public string DatabaseName
		{
			get;
			set;
		}
		static DbReference()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<DbReference<T, TId>>(delegate(ITypeConfiguration<DbReference<T, TId>> dbr)
				{
					dbr.ForProperty((DbReference<T, TId> d) => d.Collection).UseAlias("$ref");
					dbr.ForProperty((DbReference<T, TId> d) => d.DatabaseName).UseAlias("$db");
					dbr.ForProperty((DbReference<T, TId> d) => (object)d.Id).UseAlias("$id");
				});
			});
		}
		public DbReference()
		{
		}
		public DbReference(TId id)
		{
			this.Id = id;
			this.Collection = MongoConfiguration.GetCollectionName(typeof(T));
		}
		public T Fetch(Func<IMongoCollection<T>> referenceCollection)
		{
			return referenceCollection().FindOne(new
			{
				_id = this.Id
			});
		}
		public T Fetch(Func<IMongo> server)
		{
			return this.Fetch(() => server().GetCollection<T>());
		}
	}
	public class DbReference<T> : DbReference<T, ObjectId> where T : class, new()
	{
		static DbReference()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<DbReference<T>>(delegate(ITypeConfiguration<DbReference<T>> dbr)
				{
					dbr.ForProperty((DbReference<T> d) => d.Collection).UseAlias("$ref");
					dbr.ForProperty((DbReference<T> d) => d.DatabaseName).UseAlias("$db");
					dbr.ForProperty((DbReference<T> d) => d.Id).UseAlias("$id");
				});
			});
		}
		public DbReference()
		{
		}
		public DbReference(ObjectId id) : base(id)
		{
		}
	}
}
