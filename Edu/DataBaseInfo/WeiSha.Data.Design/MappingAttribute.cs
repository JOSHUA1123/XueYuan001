using System;
namespace DataBaseInfo.Design
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class MappingAttribute : Attribute
	{
		private string name;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public MappingAttribute(string name)
		{
			this.name = name;
		}
	}
}
