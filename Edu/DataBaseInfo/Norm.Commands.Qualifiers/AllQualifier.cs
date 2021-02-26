using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class AllQualifier<T> : QualifierCommand
	{
		public AllQualifier(params T[] all) : base("$all", all)
		{
		}
	}
}
