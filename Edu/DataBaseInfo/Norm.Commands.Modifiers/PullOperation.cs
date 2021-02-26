using Norm.BSON;
using System;
namespace Norm.Commands.Modifiers
{
	public class PullOperation<T> : ModifierCommand
	{
		public PullOperation(T valueToPull) : base("$pull", valueToPull)
		{
		}
	}
}
