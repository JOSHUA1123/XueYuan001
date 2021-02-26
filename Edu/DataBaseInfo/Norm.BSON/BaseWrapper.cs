using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Norm.BSON
{
	internal abstract class BaseWrapper
	{
		public abstract object Collection
		{
			get;
		}
		public static BaseWrapper Create(Type type, Type itemType, object existingContainer)
		{
			BaseWrapper baseWrapper = BaseWrapper.CreateWrapperFromType((existingContainer == null) ? type : existingContainer.GetType(), itemType);
			baseWrapper.SetContainer(existingContainer ?? baseWrapper.CreateContainer(type, itemType));
			return baseWrapper;
		}
		private static BaseWrapper CreateWrapperFromType(Type type, Type itemType)
		{
			BaseWrapper baseWrapper = null;
			if (type.IsArray)
			{
				baseWrapper = (BaseWrapper)Activator.CreateInstance(typeof(ArrayWrapper<>).MakeGenericType(new Type[]
				{
					itemType
				}));
			}
			else
			{
				List<Type> list = new List<Type>(type.GetInterfaces().Select(delegate(Type h)
				{
					if (!h.IsGenericType)
					{
						return h;
					}
					return h.GetGenericTypeDefinition();
				}));
				list.Insert(0, type.IsGenericType ? type.GetGenericTypeDefinition() : type);
				if (list.Any((Type i) => typeof(IList<>).IsAssignableFrom(i) || typeof(IList).IsAssignableFrom(i)))
				{
					baseWrapper = new ListWrapper();
				}
				else
				{
					if (list.Any((Type y) => typeof(ICollection<>).IsAssignableFrom(y)))
					{
						baseWrapper = (BaseWrapper)Activator.CreateInstance(typeof(CollectionWrapper<>).MakeGenericType(new Type[]
						{
							itemType
						}));
					}
					else
					{
						if (list.Any((Type i) => typeof(IEnumerable<>).IsAssignableFrom(i) || typeof(IEnumerable).IsAssignableFrom(i)))
						{
							baseWrapper = new ListWrapper();
						}
						else
						{
							if (baseWrapper == null)
							{
								throw new MongoException(string.Format("Collection of type {0} cannot be deserialized.", type.FullName));
							}
						}
					}
				}
			}
			return baseWrapper;
		}
		public abstract void Add(object value);
		protected abstract object CreateContainer(Type type, Type itemType);
		protected abstract void SetContainer(object container);
	}
}
