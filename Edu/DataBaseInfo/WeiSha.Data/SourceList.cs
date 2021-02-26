using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
namespace DataBaseInfo
{
	[Serializable]
	public class SourceList<T> : ArrayList<T>, ISourceList<T>, IListConvert<T>, IArrayList<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IDataSource<IList<T>>
	{
		public SourceList()
		{
		}
		public SourceList(IList<T> list) : base(list)
		{
		}
		public SourceTable ToTable()
		{
			DataTable dataTable = this.GetDataTable(typeof(T));
			return new SourceTable(dataTable);
		}
		public SourceList<TOutput> ConvertTo<TOutput>()
		{
			return this.ConvertAll<TOutput>((T p) => DataHelper.ConvertType<T, TOutput>(p));
		}
		public new SourceList<T> FindAll(Predicate<T> match)
		{
			IList<T> list = base.FindAll(match);
			return new SourceList<T>(list);
		}
		public SourceList<T> Sort(SortComparer<T> sort)
		{
			List<T> list = new List<T>(this);
			list.Sort(sort);
			return new SourceList<T>(list);
		}
		public SourceList<T> Sort(int index, int count, SortComparer<T> sort)
		{
			List<T> list = new List<T>(this);
			list.Sort(index, count, sort);
			return new SourceList<T>(list);
		}
		public new SourceList<T> GetRange(int index, int size)
		{
			if (index < base.Count)
			{
				IList<T> range;
				if (index + size <= base.Count)
				{
					range = base.GetRange(index, size);
				}
				else
				{
					range = base.GetRange(index, base.Count - index);
				}
				return new SourceList<T>(range);
			}
			return new SourceList<T>();
		}
		public new SourceList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			IList<TOutput> list = base.ConvertAll<TOutput>(converter);
			return new SourceList<TOutput>(list);
		}
		public SourceList<IOutput> ConvertTo<TOutput, IOutput>() where TOutput : IOutput
		{
			if (!typeof(TOutput).IsClass)
			{
				throw new DataException("TOutput必须是Class类型！");
			}
			if (!typeof(IOutput).IsInterface)
			{
				throw new DataException("IOutput必须是Interface类型！");
			}
			return this.ConvertTo<TOutput>().ConvertTo<IOutput>();
		}
		internal DataTable GetDataTable(Type currType)
		{
			DataTable dataTable = new DataTable();
			dataTable.TableName = currType.Name;
			PropertyInfo[] propertiesFromType = CoreHelper.GetPropertiesFromType(currType);
			PropertyInfo[] array = propertiesFromType;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				Type type = propertyInfo.PropertyType;
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type = Nullable.GetUnderlyingType(type);
				}
				if (type.IsEnum)
				{
					type = Enum.GetUnderlyingType(type);
				}
				dataTable.Columns.Add(propertyInfo.Name, type);
			}
			if (base.Count == 0)
			{
				return dataTable;
			}
			foreach (T current in this)
			{
				DataRow dataRow = dataTable.NewRow();
				PropertyInfo[] array2 = propertiesFromType;
				for (int j = 0; j < array2.Length; j++)
				{
					PropertyInfo propertyInfo2 = array2[j];
					object propertyValue = CoreHelper.GetPropertyValue(current, propertyInfo2.Name);
					dataRow[propertyInfo2.Name] = ((propertyValue == null) ? DBNull.Value : propertyValue);
				}
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		void ISourceList<T>.ForEach(Action<T> action)
		{
			base.ForEach(action);
		}
		void ISourceList<T>.Sort(IComparer<T> comparer)
		{
			base.Sort(comparer);
		}
	}
}
