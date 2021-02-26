using System;
namespace DataBaseInfo.Design
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class DescriptionAttribute : Attribute
	{
		private string description;
		public string Description
		{
			get
			{
				return this.description;
			}
		}
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}
	}
}
