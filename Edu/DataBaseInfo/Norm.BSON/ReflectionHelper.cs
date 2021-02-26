using Norm.Attributes;
using Norm.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
namespace Norm.BSON
{
	public class ReflectionHelper
	{
		private static readonly object _lock;
		private static readonly IDictionary<Type, ReflectionHelper> _cachedTypeLookup;
		private static readonly Type _ignoredType;
		private readonly IDictionary<string, MagicProperty> _properties;
		private readonly Type _type;
		private static readonly Regex _rxGenericTypeNameFinder;
		public bool IsExpando
		{
			get;
			private set;
		}
		static ReflectionHelper()
		{
			ReflectionHelper._lock = new object();
			ReflectionHelper._cachedTypeLookup = new Dictionary<Type, ReflectionHelper>();
			ReflectionHelper._ignoredType = typeof(MongoIgnoreAttribute);
			ReflectionHelper._rxGenericTypeNameFinder = new Regex("[^`]+", RegexOptions.Compiled);
			MongoConfiguration.TypeConfigurationChanged += new Action<Type>(ReflectionHelper.TypeConfigurationChanged);
		}
		private static void TypeConfigurationChanged(Type theType)
		{
			if (ReflectionHelper._cachedTypeLookup.ContainsKey(theType))
			{
				ReflectionHelper._cachedTypeLookup.Remove(theType);
			}
		}
		public static string GetScrubbedGenericName(Type t)
		{
			string text = t.Name;
			if (t.IsGenericType)
			{
				text = ReflectionHelper._rxGenericTypeNameFinder.Match(t.Name).Value;
				Type[] genericArguments = t.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type t2 = genericArguments[i];
					text = text + "_" + ReflectionHelper.GetScrubbedGenericName(t2);
				}
			}
			return text;
		}
		public static PropertyInfo[] GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		}
		public static PropertyInfo[] GetInterfaceProperties(Type type)
		{
			Type[] interfaces = type.GetInterfaces();
			if (interfaces.Count<Type>() != 0)
			{
				List<PropertyInfo> list = new List<PropertyInfo>();
				Type[] array = interfaces;
				for (int i = 0; i < array.Length; i++)
				{
					Type type2 = array[i];
					PropertyInfo[] properties = ReflectionHelper.GetProperties(type2);
					list.AddRange(properties);
				}
				return list.ToArray();
			}
			return null;
		}
		public ReflectionHelper(Type type)
		{
			this._type = type;
			PropertyInfo[] properties = ReflectionHelper.GetProperties(type);
			this._properties = ReflectionHelper.LoadMagicProperties(properties, new IdPropertyFinder(type, properties).IdProperty);
			if (typeof(IExpando).IsAssignableFrom(type))
			{
				this.IsExpando = true;
			}
		}
		public static ReflectionHelper GetHelperForType(Type type)
		{
			ReflectionHelper reflectionHelper;
			if (!ReflectionHelper._cachedTypeLookup.TryGetValue(type, out reflectionHelper))
			{
				lock (ReflectionHelper._lock)
				{
					if (!ReflectionHelper._cachedTypeLookup.TryGetValue(type, out reflectionHelper))
					{
						reflectionHelper = new ReflectionHelper(type);
						ReflectionHelper._cachedTypeLookup[type] = reflectionHelper;
					}
				}
			}
			return reflectionHelper;
		}
		public static string FindProperty(LambdaExpression lambdaExpression)
		{
			Expression expression = lambdaExpression;
			bool flag = false;
			while (!flag)
			{
				ExpressionType nodeType = expression.NodeType;
				if (nodeType != ExpressionType.Convert)
				{
					if (nodeType != ExpressionType.Lambda)
					{
						if (nodeType != ExpressionType.MemberAccess)
						{
							flag = true;
						}
						else
						{
							MemberExpression memberExpression = (MemberExpression)expression;
							if (memberExpression.Expression.NodeType != ExpressionType.Parameter && memberExpression.Expression.NodeType != ExpressionType.Convert)
							{
								throw new ArgumentException(string.Format("Expression '{0}' must resolve to top-level member.", lambdaExpression), "lambdaExpression");
							}
							return memberExpression.Member.Name;
						}
					}
					else
					{
						expression = ((LambdaExpression)expression).Body;
					}
				}
				else
				{
					expression = ((UnaryExpression)expression).Operand;
				}
			}
			return null;
		}
		public static PropertyInfo FindProperty(Type type, string name)
		{
			return (
				from p in type.GetProperties()
				where p.Name == name
				select p).First<PropertyInfo>();
		}
		public ICollection<MagicProperty> GetProperties()
		{
			return this._properties.Values;
		}
		public MagicProperty FindProperty(string name)
		{
			if (!this._properties.ContainsKey(name))
			{
				return null;
			}
			return this._properties[name];
		}
		public MagicProperty FindIdProperty()
		{
			if (this._properties.ContainsKey("$_id"))
			{
				return this._properties["$_id"];
			}
			if (!this._properties.ContainsKey("$id"))
			{
				return null;
			}
			return this._properties["$id"];
		}
		public static PropertyInfo FindIdProperty(Type type)
		{
			return new IdPropertyFinder(type).IdProperty;
		}
		private static IDictionary<string, MagicProperty> LoadMagicProperties(IEnumerable<PropertyInfo> properties, PropertyInfo idProperty)
		{
			Dictionary<string, MagicProperty> dictionary = new Dictionary<string, MagicProperty>(StringComparer.CurrentCultureIgnoreCase);
			foreach (PropertyInfo current in properties)
			{
				if (current.GetCustomAttributes(ReflectionHelper._ignoredType, true).Length <= 0 && current.GetIndexParameters().Length <= 0)
				{
					string propertyAlias = MongoConfiguration.GetPropertyAlias(current.DeclaringType, current.Name);
					string key = (current == idProperty && propertyAlias != "$id") ? "$_id" : propertyAlias;
					dictionary.Add(key, new MagicProperty(current, current.DeclaringType));
				}
			}
			return dictionary;
		}
		public string GetTypeDiscriminator()
		{
			Type discriminatingTypeFor = MongoDiscriminatedAttribute.GetDiscriminatingTypeFor(this._type);
			if (discriminatingTypeFor != null)
			{
				return string.Join(",", this._type.AssemblyQualifiedName.Split(new char[]
				{
					','
				}), 0, 2);
			}
			return null;
		}
		public void ApplyDefaultValues(object instance)
		{
			if (instance != null)
			{
				foreach (MagicProperty current in this.GetProperties())
				{
					if (current.HasDefaultValue)
					{
						current.Setter(instance, current.GetDefaultValue());
					}
				}
			}
		}
	}
}
