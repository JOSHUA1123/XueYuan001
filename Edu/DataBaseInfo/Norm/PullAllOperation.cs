using Norm.BSON;
using System;
namespace Norm
{
	public class PullAllOperation<T> : ModifierCommand
	{
		public PullAllOperation(params T[] values) : base("$pullAll", values)
		{
		}
	}
}
