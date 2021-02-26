using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	public class SortComparer<T> : IComparer<T>
	{
		private List<SortProperty> sorts;
		public SortComparer(params SortProperty[] sorts)
		{
			this.sorts = new List<SortProperty>(sorts);
		}
		public void AddProperty(params SortProperty[] sorts)
		{
			if (sorts != null && sorts.Length > 0)
			{
				this.sorts.AddRange(sorts);
			}
		}
		public int Compare(T x, T y)
		{
			return this.CompareValue(x, y, 0);
		}
		private int CompareValue(T x, T y, int index)
		{
			int num = 0;
			if (this.sorts.Count - 1 >= index)
			{
				num = this.CompareProperty(x, y, this.sorts[index]);
				if (num == 0)
				{
					num = this.CompareValue(x, y, ++index);
				}
			}
			return num;
		}
		private int CompareProperty(T x, T y, SortProperty property)
		{
			object propertyValue = CoreHelper.GetPropertyValue(x, property.PropertyName);
			object propertyValue2 = CoreHelper.GetPropertyValue(y, property.PropertyName);
			int num = CoreHelper.Compare<object>(propertyValue, propertyValue2);
			if (property.IsDesc)
			{
				return -num;
			}
			return num;
		}
	}
}
