using System;
namespace DataBaseInfo.Design
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class SequenceAttribute : Attribute
	{
		private string name;
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		public SequenceAttribute(string name)
		{
			this.name = name;
		}
	}
}
