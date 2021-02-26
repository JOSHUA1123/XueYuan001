using System;
namespace Norm.BSON
{
	public abstract class QualifierCommand : Command
	{
		protected QualifierCommand(string command, object value) : base(command, value)
		{
		}
	}
}
