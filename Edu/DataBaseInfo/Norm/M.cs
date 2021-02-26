using Norm.Commands;
using Norm.Commands.Modifiers;
using System;
namespace Norm
{
	public class M
	{
		public static IncrementOperation Increment(int amountToIncrementBy)
		{
			return new IncrementOperation(amountToIncrementBy);
		}
		public static SetOperation<T> Set<T>(T setValue)
		{
			return new SetOperation<T>(setValue);
		}
		public static PushOperation<T> Push<T>(T pushValue)
		{
			return new PushOperation<T>(pushValue);
		}
		public static PushAllOperation<T> PushAll<T>(params T[] pushValues)
		{
			return new PushAllOperation<T>(pushValues);
		}
		public static AddToSetOperation<T> AddToSet<T>(T addToSetValue)
		{
			return new AddToSetOperation<T>(addToSetValue);
		}
		public static PullOperation<T> Pull<T>(T pullValue)
		{
			return new PullOperation<T>(pullValue);
		}
		public static PopOperation Pop(PopType popType)
		{
			return new PopOperation(popType);
		}
		public static PullAllOperation<T> PullAll<T>(params T[] pullValues)
		{
			return new PullAllOperation<T>(pullValues);
		}
	}
}
