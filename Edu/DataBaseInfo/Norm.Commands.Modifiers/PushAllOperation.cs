using Norm.BSON;
using System;
namespace Norm.Commands.Modifiers
{
	public class PushAllOperation<T> : ModifierCommand
	{
		public PushAllOperation(params T[] pushValues) : base("$pushAll", pushValues)
		{
		}
	}
}
