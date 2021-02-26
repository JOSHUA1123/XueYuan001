using Norm.BSON;
using System;
namespace Norm.Commands.Modifiers
{
	public class PushOperation<T> : ModifierCommand
	{
		public PushOperation(T pushValue) : base("$push", pushValue)
		{
		}
	}
}
