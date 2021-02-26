using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class InQualifier<T> : QualifierCommand
	{
		public InQualifier(params T[] inset) : base("$in", inset)
		{
		}
	}
}
