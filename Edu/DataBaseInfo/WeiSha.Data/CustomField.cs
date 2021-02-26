using System;
using DataBaseInfo.Design;
namespace DataBaseInfo
{
	[Serializable]
	internal class CustomField : Field, IProvider
	{
		private QueryCreator creator;
		private string qString;
		internal override string Name
		{
			get
			{
				if (string.IsNullOrEmpty(this.qString))
				{
					throw new DataException("需要设置DbProvider及DbTrans才能处理CustomField！");
				}
				return string.Format("({1}) as {0}", base.Name, this.qString);
			}
		}
		public CustomField(string fieldName, QueryCreator creator) : base(fieldName)
		{
			this.creator = creator;
		}
		void IProvider.SetDbProvider(DbProvider dbProvider, DbTrans dbTran)
		{
			if (this.creator != null)
			{
				QuerySection<ViewEntity> query = base.GetQuery(this.creator);
				query.SetDbProvider(dbProvider, dbTran);
				this.qString = query.GetTop(1).QueryString;
			}
		}
	}
	[Serializable]
	internal class CustomField<T> : Field, IProvider where T : Entity
	{
		private TableRelation<T> relation;
		private string qString;
		internal override string Name
		{
			get
			{
				if (string.IsNullOrEmpty(this.qString))
				{
					throw new DataException("需要设置DbProvider及DbTrans才能处理CustomField！");
				}
				return string.Format("({1}) as {0}", base.Name, this.qString);
			}
		}
		public CustomField(string fieldName, QuerySection<T> query) : base(fieldName)
		{
			this.qString = query.GetTop(1).QueryString;
		}
		public CustomField(string fieldName, TableRelation<T> relation) : base(fieldName)
		{
			this.relation = relation;
		}
		void IProvider.SetDbProvider(DbProvider dbProvider, DbTrans dbTran)
		{
			if (string.IsNullOrEmpty(this.qString) && this.relation != null)
			{
				QuerySection<T> query = this.relation.GetFromSection().Query;
				query.SetDbProvider(dbProvider, dbTran);
				this.qString = query.GetTop(1).QueryString;
			}
		}
	}
}
