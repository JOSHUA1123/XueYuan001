using System;
namespace DataBaseInfo
{
	[Serializable]
	public class AllField : Field
	{
		public virtual Field this[string propertyName]
		{
			get
			{
				return new Field(propertyName);
			}
		}
		public AllField() : base("All", null, "*", null)
		{
		}
		public IFieldFilter Exclude(params Field[] fields)
		{
			return new ExcludeField(fields);
		}
		public IFieldFilter Include(params Field[] fields)
		{
			return new IncludeField(fields);
		}
	}
	[Serializable]
	public class AllField<T> : AllField where T : Entity
	{
		public override Field this[string propertyName]
		{
			get
			{
				T t = CoreHelper.CreateInstance<T>();
				return t.As<IEntityBase>().GetField(propertyName);
			}
		}
		public AllField()
		{
			this.tableName = Table.GetTable<T>().OriginalName;
		}
	}
}
