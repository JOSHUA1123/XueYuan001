using System;
namespace DataBaseInfo.Design
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class ReadOnlyAttribute : Attribute
	{
	}
}
