using System;
namespace DataBaseInfo
{
	public class SortProperty
	{
		private string propertyName;
		private bool desc;
		internal string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}
		internal bool IsDesc
		{
			get
			{
				return this.desc;
			}
		}
		public SortProperty Asc
		{
			get
			{
				return new SortProperty(this.propertyName, false);
			}
		}
		public SortProperty Desc
		{
			get
			{
				return new SortProperty(this.propertyName, true);
			}
		}
		public SortProperty(string propertyName)
		{
			this.propertyName = propertyName;
			this.desc = false;
		}
		private SortProperty(string propertyName, bool desc) : this(propertyName)
		{
			this.desc = desc;
		}
	}
}
