using Norm.Collections;
using Norm.Configuration;
using System;
namespace Norm.Protocol.SystemMessages.Request
{
	internal class CreateCollectionRequest : ISystemQuery
	{
		private readonly CreateCollectionOptions _options;
		public string Create
		{
			get
			{
				return this._options.Name;
			}
		}
		public int? Size
		{
			get
			{
				return this._options.Size;
			}
		}
		public long? Max
		{
			get
			{
				return this._options.Max;
			}
		}
		public bool Capped
		{
			get
			{
				return this._options.Capped;
			}
		}
		public bool AutoIndexId
		{
			get
			{
				return this._options.AutoIndexId;
			}
		}
		static CreateCollectionRequest()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<CreateCollectionRequest>(delegate(ITypeConfiguration<CreateCollectionRequest> a)
				{
					a.ForProperty((CreateCollectionRequest auth) => auth.Create).UseAlias("create");
					a.ForProperty((CreateCollectionRequest auth) => (object)auth.Size).UseAlias("size");
					a.ForProperty((CreateCollectionRequest auth) => (object)auth.Max).UseAlias("max");
					a.ForProperty((CreateCollectionRequest auth) => (object)auth.Capped).UseAlias("capped");
					a.ForProperty((CreateCollectionRequest auth) => (object)auth.AutoIndexId).UseAlias("autoIndexId");
				});
			});
		}
		public CreateCollectionRequest(CreateCollectionOptions options)
		{
			this._options = options;
		}
	}
}
