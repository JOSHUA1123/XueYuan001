using System;
using System.Collections.Generic;
namespace Norm.BSON
{
	internal class CollectionWrapper<T> : BaseWrapper
	{
		private ICollection<T> _list;
		public override object Collection
		{
			get
			{
				return this._list;
			}
		}
		public override void Add(object value)
		{
			this._list.Add((T)((object)value));
		}
		protected override object CreateContainer(Type type, Type itemType)
		{
			return Activator.CreateInstance(type);
		}
		protected override void SetContainer(object container)
		{
			this._list = (ICollection<T>)container;
		}
	}
}
