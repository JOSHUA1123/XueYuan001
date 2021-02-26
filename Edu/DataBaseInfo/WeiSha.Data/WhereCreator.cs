using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public abstract class WhereCreator<TCreator> : TableCreator<TCreator>, IWhereCreator<TCreator>, ITableCreator<TCreator> where TCreator : class
	{
		private IList<WhereClip> whereList;
		internal WhereClip Where
		{
			get
			{
				WhereClip whereClip = WhereClip.None;
				foreach (WhereClip current in this.whereList)
				{
					whereClip &= current;
				}
				return whereClip;
			}
		}
		protected WhereCreator()
		{
			this.whereList = new List<WhereClip>();
		}
		protected WhereCreator(string tableName, string aliasName) : base(tableName, aliasName)
		{
			this.whereList = new List<WhereClip>();
		}
		protected WhereCreator(Table table) : base(table)
		{
			this.whereList = new List<WhereClip>();
		}
		public TCreator AddWhere(WhereClip where)
		{
			if (DataHelper.IsNullOrEmpty(where))
			{
				return this as TCreator;
			}
			this.whereList.Add(where);
			return this as TCreator;
		}
		public TCreator AddWhere(string where, params SQLParameter[] parameters)
		{
			if (string.IsNullOrEmpty(where))
			{
				return this as TCreator;
			}
			return this.AddWhere(new WhereClip(where, parameters));
		}
		public TCreator AddWhere(Field field, object value)
		{
			if (value == null)
			{
				return this.AddWhere(field.IsNull());
			}
			return this.AddWhere(field == value);
		}
		public TCreator AddWhere(string fieldName, object value)
		{
			return this.AddWhere(new Field(fieldName), value);
		}
	}
}
