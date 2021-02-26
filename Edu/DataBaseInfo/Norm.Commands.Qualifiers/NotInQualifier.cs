using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class NotInQualifier<T> : QualifierCommand
	{
		public NotInQualifier(params T[] notInSet) : base("$nin", notInSet)
		{
		}
	}
}
