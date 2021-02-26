using System;
namespace DataBaseInfo
{
	[Serializable]
	public class GroupByClip
	{
        public static readonly GroupByClip None = new GroupByClip((string)null);
		private string groupBy;
		public string Value
		{
			get
			{
				return this.groupBy;
			}
			set
			{
				this.groupBy = value;
			}
		}
		public GroupByClip()
		{
		}
		public GroupByClip(string groupBy)
		{
			this.groupBy = groupBy;
		}
		public GroupByClip(params Field[] fields)
		{
			if (fields != null && fields.Length > 0)
			{
				GroupByClip groupByClip = GroupByClip.None;
				for (int i = 0; i < fields.Length; i++)
				{
					Field field = fields[i];
					groupByClip &= field.Group;
				}
				this.groupBy = groupByClip.ToString();
			}
		}
		public static GroupByClip operator &(GroupByClip leftGroup, GroupByClip rightGroup)
		{
			if (DataHelper.IsNullOrEmpty(leftGroup) && DataHelper.IsNullOrEmpty(rightGroup))
			{
				return GroupByClip.None;
			}
			if (DataHelper.IsNullOrEmpty(leftGroup))
			{
				return rightGroup;
			}
			if (DataHelper.IsNullOrEmpty(rightGroup))
			{
				return leftGroup;
			}
			return new GroupByClip(leftGroup.ToString() + "," + rightGroup.ToString());
		}
		public static bool operator true(GroupByClip right)
		{
			return false;
		}
		public static bool operator false(GroupByClip right)
		{
			return false;
		}
		public override string ToString()
		{
			return this.groupBy;
		}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
