using System;
namespace Norm.BSON
{
	public abstract class ModifierCommand : Command
	{
		protected ModifierCommand(string command, object value) : base(command, value)
		{
		}
	}
}
