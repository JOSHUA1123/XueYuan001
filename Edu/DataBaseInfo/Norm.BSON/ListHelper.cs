using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
namespace Norm.BSON
{
	internal static class ListHelper
	{
		public static Type GetListItemType(Type enumerableType)
		{
			if (enumerableType.IsArray)
			{
				return enumerableType.GetElementType();
			}
			if (!enumerableType.IsGenericType)
			{
				return typeof(object);
			}
			return enumerableType.GetGenericArguments()[0];
		}
		public static Type GetDictionarKeyType(Type enumerableType)
		{
			if (!enumerableType.IsGenericType)
			{
				return typeof(object);
			}
			return enumerableType.GetGenericArguments()[0];
		}
		public static Type GetDictionarValueType(Type enumerableType)
		{
			if (!enumerableType.IsGenericType)
			{
				return typeof(object);
			}
			return enumerableType.GetGenericArguments()[1];
		}
		public static IDictionary CreateDictionary(Type dictionaryType, Type keyType, Type valueType)
		{
			IDictionary result = new Dictionary<object, object>(0);
			if (dictionaryType.IsInterface)
			{
				result = (IDictionary)Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					keyType,
					valueType
				}));
			}
			else
			{
				if (dictionaryType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null) != null)
				{
					result = (IDictionary)Activator.CreateInstance(dictionaryType);
				}
			}
			return result;
		}
	}
}
