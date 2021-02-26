using Norm.BSON;
using System;
namespace Norm.Commands.Modifiers
{
	public class SetOperation<T> : ModifierCommand
	{
		public SetOperation(T setValue) : base("$set", setValue)
		{
		}
	}
}
