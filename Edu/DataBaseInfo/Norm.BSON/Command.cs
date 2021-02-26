using System;
namespace Norm.BSON
{
	public abstract class Command
	{
		public string CommandName
		{
			get;
			protected set;
		}
		public object ValueForCommand
		{
			get;
			set;
		}
		protected Command(string commandName, object value)
		{
			this.CommandName = commandName;
			this.ValueForCommand = value;
		}
	}
}
