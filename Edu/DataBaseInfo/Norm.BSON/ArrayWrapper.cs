using System;
using System.Collections.Generic;
namespace Norm.BSON
{
	internal class ArrayWrapper<T> : BaseWrapper
	{
		private readonly List<T> _list = new List<T>();
		public override object Collection
		{
			get
			{
				return this._list.ToArray();
			}
		}
		public override void Add(object value)
		{
			this._list.Add((T)((object)value));
		}
		protected override object CreateContainer(Type type, Type itemType)
		{
			return null;
		}
		protected override void SetContainer(object container)
		{
			if (container != null)
			{
				throw new MongoException("An container cannot exist when trying to deserialize an array");
			}
		}
	}
}
