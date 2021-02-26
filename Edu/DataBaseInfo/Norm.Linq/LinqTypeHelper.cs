using System;
using System.Collections.Generic;
using System.Reflection;
namespace Norm.Linq
{
	public static class LinqTypeHelper
	{
		public static Type FindIEnumerable(Type seqType)
		{
			if (seqType == null || seqType == typeof(string))
			{
				return null;
			}
			if (seqType.IsArray)
			{
				return typeof(IEnumerable<>).MakeGenericType(new Type[]
				{
					seqType.GetElementType()
				});
			}
			if (seqType.IsGenericType)
			{
				Type[] genericArguments = seqType.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type type = genericArguments[i];
					Type type2 = typeof(IEnumerable<>).MakeGenericType(new Type[]
					{
						type
					});
					if (type2.IsAssignableFrom(seqType))
					{
						Type result = type2;
						return result;
					}
				}
			}
			Type[] interfaces = seqType.GetInterfaces();
			if (interfaces != null && interfaces.Length > 0)
			{
				Type[] array = interfaces;
				for (int j = 0; j < array.Length; j++)
				{
					Type seqType2 = array[j];
					Type type3 = LinqTypeHelper.FindIEnumerable(seqType2);
					if (type3 != null)
					{
						Type result = type3;
						return result;
					}
				}
			}
			if (seqType.BaseType != null && seqType.BaseType != typeof(object))
			{
				return LinqTypeHelper.FindIEnumerable(seqType.BaseType);
			}
			return null;
		}
		public static Type GetSequenceType(Type elementType)
		{
			return typeof(IEnumerable<>).MakeGenericType(new Type[]
			{
				elementType
			});
		}
		public static Type GetElementType(Type seqType)
		{
			Type type = LinqTypeHelper.FindIEnumerable(seqType);
			if (!(type == null))
			{
				return type.GetGenericArguments()[0];
			}
			return seqType;
		}
		public static bool IsNullableType(Type type)
		{
			return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}
		public static bool IsNullAssignable(Type type)
		{
			return !type.IsValueType || LinqTypeHelper.IsNullableType(type);
		}
		public static Type GetNonNullableType(Type type)
		{
			if (!LinqTypeHelper.IsNullableType(type))
			{
				return type;
			}
			return type.GetGenericArguments()[0];
		}
		public static Type GetMemberType(MemberInfo mi)
		{
			FieldInfo fieldInfo = mi as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.FieldType;
			}
			PropertyInfo propertyInfo = mi as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			EventInfo eventInfo = mi as EventInfo;
			if (!(eventInfo != null))
			{
				return null;
			}
			return eventInfo.EventHandlerType;
		}
	}
}
