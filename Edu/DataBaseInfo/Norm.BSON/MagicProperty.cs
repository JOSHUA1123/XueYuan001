using Norm.Attributes;
using System;
using System.ComponentModel;
using System.Reflection;
namespace Norm.BSON
{
	public class MagicProperty
	{
		private static readonly Type _myType = typeof(MagicProperty);
		private static readonly Type _ignoredIfNullType = typeof(MongoIgnoreIfNullAttribute);
		private static readonly Type _defaultValueType = typeof(DefaultValueAttribute);
		private readonly PropertyInfo _property;
		private readonly DefaultValueAttribute _defaultValueAttribute;
		public Type DeclaringType
		{
			get;
			private set;
		}
		public Type Type
		{
			get
			{
				return this._property.PropertyType;
			}
		}
		public string Name
		{
			get
			{
				return this._property.Name;
			}
		}
		public bool IgnoreIfNull
		{
			get;
			private set;
		}
		public bool HasDefaultValue
		{
			get
			{
				return this._defaultValueAttribute != null;
			}
		}
		public Action<object, object> Setter
		{
			get;
			private set;
		}
		public Func<object, object> Getter
		{
			get;
			private set;
		}
		public Func<object, bool> ShouldSerialize
		{
			get;
			private set;
		}
		public MagicProperty(PropertyInfo property, Type declaringType)
		{
			this._property = property;
			this.IgnoreIfNull = (property.GetCustomAttributes(MagicProperty._ignoredIfNullType, true).Length > 0);
			object[] customAttributes = property.GetCustomAttributes(MagicProperty._defaultValueType, true);
			if (customAttributes.Length > 0)
			{
				this._defaultValueAttribute = (DefaultValueAttribute)customAttributes[0];
			}
			this.DeclaringType = declaringType;
			this.Getter = MagicProperty.CreateGetterMethod(property);
			this.Setter = MagicProperty.CreateSetterMethod(property);
			this.ShouldSerialize = MagicProperty.CreateShouldSerializeMethod(property);
		}
		public object GetDefaultValue()
		{
			object result = null;
			if (this.HasDefaultValue)
			{
				result = this._defaultValueAttribute.Value;
			}
			return result;
		}
		public bool IgnoreProperty(object document, out object value)
		{
			value = null;
			bool result = false;
			if (this.ShouldSerialize(document))
			{
				value = this.Getter(document);
				if (this.HasDefaultValue)
				{
					object defaultValue = this.GetDefaultValue();
					bool flag = (defaultValue != null) ? defaultValue.Equals(value) : (value == null);
					if (flag)
					{
						result = true;
					}
				}
				if (this.IgnoreIfNull && value == null)
				{
					result = true;
				}
			}
			return result;
		}
		private static Action<object, object> CreateSetterMethod(PropertyInfo property)
		{
			MethodInfo method = MagicProperty._myType.GetMethod("SetterMethod", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
			{
				property.DeclaringType,
				property.PropertyType
			});
			return (Action<object, object>)methodInfo.Invoke(null, new object[]
			{
				property
			});
		}
		private static Func<object, object> CreateGetterMethod(PropertyInfo property)
		{
			MethodInfo method = MagicProperty._myType.GetMethod("GetterMethod", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo = method.MakeGenericMethod(new Type[]
			{
				property.DeclaringType,
				property.PropertyType
			});
			return (Func<object, object>)methodInfo.Invoke(null, new object[]
			{
				property
			});
		}
		private static Func<object, bool> CreateShouldSerializeMethod(PropertyInfo property)
		{
			string name = "ShouldSerialize" + property.Name;
			MemberInfo[] member = property.DeclaringType.GetMember(name);
			if (member.Length == 0)
			{
				return (object o) => true;
			}
			MethodInfo methodInfo = member[0] as MethodInfo;
			MethodInfo method = MagicProperty._myType.GetMethod("ShouldSerializeMethod", BindingFlags.Static | BindingFlags.NonPublic);
			MethodInfo methodInfo2 = method.MakeGenericMethod(new Type[]
			{
				property.DeclaringType
			});
			return (Func<object, bool>)methodInfo2.Invoke(null, new object[]
			{
				methodInfo
			});
		}
		private static Action<object, object> SetterMethod<TTarget, TParam>(PropertyInfo method) where TTarget : class
		{
			MethodInfo setMethod = method.GetSetMethod(true);
			if (setMethod == null)
			{
				return null;
			}
			Action<TTarget, TParam> func = (Action<TTarget, TParam>)Delegate.CreateDelegate(typeof(Action<TTarget, TParam>), setMethod);
			return delegate(object target, object param)
			{
				func((TTarget)((object)target), (TParam)((object)param));
			};
		}
		private static Func<object, object> GetterMethod<TTarget, TParam>(PropertyInfo method) where TTarget : class
		{
			MethodInfo getMethod = method.GetGetMethod(true);
			Func<TTarget, TParam> func = (Func<TTarget, TParam>)Delegate.CreateDelegate(typeof(Func<TTarget, TParam>), getMethod);
			return (object target) => func((TTarget)((object)target));
		}
		private static Func<object, bool> ShouldSerializeMethod<TTarget>(MethodInfo method) where TTarget : class
		{
			Func<TTarget, bool> func = (Func<TTarget, bool>)Delegate.CreateDelegate(typeof(Func<TTarget, bool>), method);
			return (object target) => func((TTarget)((object)target));
		}
	}
}
