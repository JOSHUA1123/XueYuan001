using System;
namespace DataBaseInfo
{
	[Serializable]
	public abstract class TableCreator<TCreator> : ITableCreator<TCreator> where TCreator : class
	{
		private Table table;
		internal Table Table
		{
			get
			{
				return this.table;
			}
		}
		protected TableCreator()
		{
		}
		protected TableCreator(string tableName, string aliasName) : this()
		{
			this.table = new Table(tableName).As(aliasName);
		}
		protected TableCreator(Table table) : this()
		{
			this.table = table;
		}
		public TCreator From(string tableName)
		{
			this.table = new Table(tableName);
			return this as TCreator;
		}
		public TCreator From(Table table)
		{
			this.table = table;
			return this as TCreator;
		}
	}
}
