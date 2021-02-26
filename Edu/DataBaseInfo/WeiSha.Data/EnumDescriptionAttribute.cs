using System;
using System.Collections.Generic;
using System.Reflection;
namespace DataBaseInfo
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class EnumDescriptionAttribute : Attribute
	{
		private string _desc;
		public string Description
		{
			get
			{
				return this._desc;
			}
		}
		public EnumDescriptionAttribute(string desc)
		{
			this._desc = desc;
		}
		public static string GetDescription(object enumObj)
		{
			Type type = enumObj.GetType();
			if (!type.IsEnum)
			{
				throw new Exception("参数不是枚举类型！");
			}
			FieldInfo field = type.GetField(enumObj.ToString());
			object[] customAttributes = field.GetCustomAttributes(false);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			EnumDescriptionAttribute enumDescriptionAttribute = customAttributes[0] as EnumDescriptionAttribute;
			return enumDescriptionAttribute.Description;
		}
		public static string[] GetDescriptions(Type enumType)
		{
			if (!enumType.IsEnum)
			{
				throw new Exception("参数不是枚举类型！");
			}
			FieldInfo[] fields = enumType.GetFields();
			List<string> list = new List<string>();
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				object[] customAttributes = fieldInfo.GetCustomAttributes(false);
				if (customAttributes.Length != 0)
				{
					EnumDescriptionAttribute enumDescriptionAttribute = customAttributes[0] as EnumDescriptionAttribute;
					list.Add(enumDescriptionAttribute.Description);
				}
			}
			return list.ToArray();
		}
	}
}
