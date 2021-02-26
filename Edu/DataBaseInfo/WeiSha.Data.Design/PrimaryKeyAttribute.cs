using System;
namespace DataBaseInfo.Design
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class PrimaryKeyAttribute : Attribute
	{
	}
}
