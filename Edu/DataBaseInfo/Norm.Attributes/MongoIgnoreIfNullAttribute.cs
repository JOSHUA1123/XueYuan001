using System;
namespace Norm.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MongoIgnoreIfNullAttribute : Attribute
	{
	}
}
