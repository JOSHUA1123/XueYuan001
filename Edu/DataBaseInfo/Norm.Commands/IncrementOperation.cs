using Norm.BSON;
using System;
namespace Norm.Commands
{
	public class IncrementOperation : ModifierCommand
	{
		public IncrementOperation(int amountToIncrement) : base("$inc", amountToIncrement)
		{
		}
	}
}
