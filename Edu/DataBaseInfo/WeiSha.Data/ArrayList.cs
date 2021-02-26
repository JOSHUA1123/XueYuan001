using System;
using System.Collections;
using System.Collections.Generic;
namespace DataBaseInfo
{
	[Serializable]
	public class ArrayList<T> : List<T>, IArrayList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IDataSource<IList<T>>, IDisposable
	{
		public new T this[int index]
		{
			get
			{
				if (base.Count == 0)
				{
					return default(T);
				}
				if (base.Count > index)
				{
					return base[index];
				}
				return default(T);
			}
			set
			{
				if (base.Count > index)
				{
					base[index] = value;
				}
			}
		}
		public IList<T> OriginalData
		{
			get
			{
				return base.ConvertAll<T>((T p) => p);
			}
		}
		public ArrayList()
		{
		}
		public ArrayList(IList<T> list)
		{
			if (list != null)
			{
				base.AddRange(list);
			}
		}
		public IDictionary<TResult, IList<T>> ToDictionary<TResult>(Field groupField)
		{
			return this.ToDictionary<TResult>(groupField.PropertyName);
		}
		public IDictionary<TResult, IList<T>> ToDictionary<TResult>(string propertyName)
		{
			IDictionary<TResult, IList<T>> dictionary = new Dictionary<TResult, IList<T>>();
			if (base.Count == 0)
			{
				return dictionary;
			}
			IDictionary<TResult, IList<T>> result;
			lock (dictionary)
			{
				foreach (T current in this)
				{
					object propertyValue = CoreHelper.GetPropertyValue(current, propertyName);
					TResult key = CoreHelper.ConvertValue<TResult>(propertyValue);
					if (!dictionary.ContainsKey(key))
					{
						dictionary[key] = new SourceList<T>();
					}
					dictionary[key].Add(current);
				}
				result = dictionary;
			}
			return result;
		}
		public void Dispose()
		{
			base.Clear();
		}
		T[] IArrayList<T>.ToArray()
		{
			return base.ToArray();
		}
	}
}
