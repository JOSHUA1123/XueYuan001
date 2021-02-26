using Norm.BSON;
using System;
namespace Norm.Commands.Modifiers
{
	public class AddToSetOperation<T> : ModifierCommand
	{
		public AddToSetOperation(T addToSetValue) : base("$addToSet", addToSetValue)
		{
		}
	}
}
