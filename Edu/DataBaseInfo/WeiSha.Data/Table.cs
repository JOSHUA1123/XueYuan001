using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public class Table : ITable
	{
		private static readonly IDictionary<Type, Table> dictTable = new Dictionary<Type, Table>();
		private string aliasName;
		private string tableName;
		private string prefix;
		private string suffix;
		public Field this[string fieldName]
		{
			get
			{
				return new Field(fieldName).At(this);
			}
		}
		internal string Alias
		{
			get
			{
				return this.aliasName;
			}
		}
		internal string FullName
		{
			get
			{
				if (this.aliasName == null)
				{
					return this.Name;
				}
				return string.Format("{0} __[{1}]__", this.Name, this.aliasName);
			}
		}
		internal string Name
		{
			get
			{
				return "__[" + this.OriginalName + "]__";
			}
		}
		internal string TableName
		{
			get
			{
				return this.tableName;
			}
			set
			{
				this.tableName = value;
			}
		}
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}
		public string Suffix
		{
			get
			{
				return this.suffix;
			}
			set
			{
				this.suffix = value;
			}
		}
		public string OriginalName
		{
			get
			{
				return string.Format("{0}{1}{2}", this.prefix, this.tableName, this.suffix);
			}
		}
		public Table(string tableName)
		{
			this.tableName = tableName.Replace("__[", "").Replace("]__", "");
			this.prefix = null;
			this.suffix = null;
		}
		public Table As(string aliasName)
		{
			if (!string.IsNullOrEmpty(aliasName))
			{
				this.aliasName = aliasName;
			}
			return this;
		}
		public static Table GetTable<T>() where T : Entity
		{
			if (Table.dictTable.ContainsKey(typeof(T)))
			{
				return Table.dictTable[typeof(T)];
			}
			lock (Table.dictTable)
			{
				T t = CoreHelper.CreateInstance<T>();
				Table table = t.GetTable();
				Table.dictTable[typeof(T)] = table;
			}
			return Table.dictTable[typeof(T)];
		}
		public static Table GetTable<T>(string suffix) where T : Entity
		{
			Table table = Table.GetTable<T>();
			table.Suffix = suffix;
			return table;
		}
		public static Table GetTable<T>(string prefix, string suffix) where T : Entity
		{
			Table table = Table.GetTable<T>();
			table.Prefix = prefix;
			table.Suffix = suffix;
			return table;
		}
		public static TableRelation<T> From<T>() where T : Entity
		{
            return Table.From<T>((Table)null);
		}
		public static TableRelation<T> From<T>(string aliasName) where T : Entity
		{
			return new TableRelation<T>(null, aliasName);
		}
		public static TableRelation<T> From<T>(Table table) where T : Entity
		{
			return new TableRelation<T>(table, null);
		}
	}
	[Serializable]
	public class Table<T> : Table where T : class
	{
		public Table(string tableName) : base(tableName)
		{
			Table mappingTable = EntityConfig.Instance.GetMappingTable<T>(tableName);
			base.TableName = mappingTable.TableName;
			base.Prefix = mappingTable.Prefix;
			base.Suffix = mappingTable.Suffix;
		}
	}
}
