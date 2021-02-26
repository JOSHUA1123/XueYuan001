using System;
namespace DataBaseInfo
{
	[Serializable]
	public class OrderByClip
	{
		public static readonly OrderByClip None = new OrderByClip(null);
		private string orderBy;
		public string Value
		{
			get
			{
				return this.orderBy;
			}
			set
			{
				this.orderBy = value;
			}
		}
		public OrderByClip()
		{
		}
		public OrderByClip(string orderBy)
		{
			this.orderBy = orderBy;
		}
		public static bool operator true(OrderByClip right)
		{
			return false;
		}
		public static bool operator false(OrderByClip right)
		{
			return false;
		}
		public static OrderByClip operator &(OrderByClip leftOrder, OrderByClip rightOrder)
		{
			if (DataHelper.IsNullOrEmpty(leftOrder) && DataHelper.IsNullOrEmpty(rightOrder))
			{
				return OrderByClip.None;
			}
			if (DataHelper.IsNullOrEmpty(leftOrder))
			{
				return rightOrder;
			}
			if (DataHelper.IsNullOrEmpty(rightOrder))
			{
				return leftOrder;
			}
			return new OrderByClip(leftOrder.ToString() + "," + rightOrder.ToString());
		}
		public override string ToString()
		{
			return this.orderBy;
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
