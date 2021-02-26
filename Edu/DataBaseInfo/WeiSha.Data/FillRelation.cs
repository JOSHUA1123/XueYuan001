using System;
namespace DataBaseInfo
{
	[Serializable]
	public class FillRelation
	{
		public SourceTable DataSource
		{
			get;
			set;
		}
		public string ParentName
		{
			get;
			set;
		}
		public string ChildName
		{
			get;
			set;
		}
		public FillRelation()
		{
		}
		public FillRelation(SourceTable source, string relationName) : this(source, relationName, relationName)
		{
		}
		public FillRelation(SourceTable source, string parentName, string childName)
		{
			this.DataSource = source;
			this.ParentName = parentName;
			this.ChildName = childName;
		}
	}
}
