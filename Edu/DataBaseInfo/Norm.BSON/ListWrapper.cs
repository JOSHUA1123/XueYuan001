using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
namespace Norm.BSON
{
	internal class ListWrapper : BaseWrapper
	{
		private IList _list;
		public override object Collection
		{
			get
			{
				return this._list;
			}
		}
		public override void Add(object value)
		{
			this._list.Add(value);
		}
		protected override object CreateContainer(Type type, Type itemType)
		{
			object result = null;
			if (type.IsInterface)
			{
				result = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[]
				{
					itemType
				}));
			}
			else
			{
				if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null) != null)
				{
					result = Activator.CreateInstance(type);
				}
			}
			return result;
		}
		protected override void SetContainer(object container)
		{
			this._list = ((container == null) ? new ArrayList() : ((IList)container));
		}
	}
}
