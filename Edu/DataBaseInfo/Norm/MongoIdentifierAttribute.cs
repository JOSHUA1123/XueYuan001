using System;
namespace Norm
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class MongoIdentifierAttribute : Attribute
	{
	}
}
