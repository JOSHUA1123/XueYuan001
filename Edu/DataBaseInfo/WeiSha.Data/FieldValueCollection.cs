using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	public class FieldValueCollection<T> where T : Entity
	{
		private IDictionary<Field, object> fvValues;
		public FieldValueCollection(IDictionary<string, object> dictValues)
		{
			this.fvValues = new Dictionary<Field, object>();
			T t = CoreHelper.CreateInstance<T>();
			foreach (KeyValuePair<string, object> current in dictValues)
			{
				Field field = t.As<IEntityBase>().GetField(current.Key);
				if (field != null)
				{
					this.fvValues[field] = current.Value;
				}
			}
		}
		public FieldValueCollection(IDictionary<Field, object> dictValues)
		{
			this.fvValues = dictValues;
		}
		public FieldValue[] ToList()
		{
			List<FieldValue> list = new List<FieldValue>();
			foreach (KeyValuePair<Field, object> current in this.fvValues)
			{
				list.Add(new FieldValue(current.Key, current.Value));
			}
			return list.ToArray();
		}
	}
}
