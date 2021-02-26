using System;
namespace Norm
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class MongoDiscriminatedAttribute : Attribute
	{
		private static readonly Type AttributeType = typeof(MongoDiscriminatedAttribute);
		private static readonly Type RootType = typeof(object);
		public static Type GetDiscriminatingTypeFor(Type type)
		{
			Type type2 = type;
			while (type2 != MongoDiscriminatedAttribute.RootType)
			{
				if (type2.IsDefined(MongoDiscriminatedAttribute.AttributeType, false))
				{
					return type2;
				}
				type2 = type2.BaseType;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				Type type3 = interfaces[i];
				if (type3.IsDefined(MongoDiscriminatedAttribute.AttributeType, false))
				{
					return type3;
				}
			}
			return null;
		}
	}
}
