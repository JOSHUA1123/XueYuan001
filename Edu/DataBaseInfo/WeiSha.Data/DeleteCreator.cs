using System;
namespace DataBaseInfo
{
	[Serializable]
	public class DeleteCreator : WhereCreator<DeleteCreator>, IDeleteCreator, IWhereCreator<DeleteCreator>, ITableCreator<DeleteCreator>
	{
		public static DeleteCreator NewCreator()
		{
			return new DeleteCreator();
		}
		public static DeleteCreator NewCreator(string tableName)
		{
			return new DeleteCreator(tableName);
		}
		public static DeleteCreator NewCreator(Table table)
		{
			return new DeleteCreator(table);
		}
		private DeleteCreator()
		{
		}
		private DeleteCreator(string tableName) : base(tableName, null)
		{
		}
		private DeleteCreator(Table table) : base(table)
		{
		}
	}
}
