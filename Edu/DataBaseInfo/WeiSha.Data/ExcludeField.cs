using System;
using System.Collections.Generic;
using System.Linq;
namespace DataBaseInfo
{
	[Serializable]
	public class ExcludeField : IFieldFilter
	{
		private Field[] fields;
		internal List<Field> Fields
		{
			get
			{
				if (this.fields == null || this.fields.Length == 0)
				{
					return new List<Field>();
				}
				return new List<Field>(this.fields);
			}
		}
		internal ExcludeField(Field[] fields)
		{
			this.fields = fields;
		}
		public Field[] GetFields(Field[] fields)
		{
			List<Field> list = new List<Field>(fields);
			list.RemoveAll((Field f) => this.Fields.Any((Field p) => p.Name == f.Name));
			return list.ToArray();
		}
	}
}
