using Norm.BSON;
using System;
namespace Norm.Commands.Modifiers
{
	public class PopOperation : ModifierCommand
	{
		public PopOperation(PopType popType) : base("$pop", (int)popType)
		{
		}
	}
}
