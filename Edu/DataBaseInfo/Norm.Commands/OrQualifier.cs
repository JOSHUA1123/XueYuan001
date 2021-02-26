using Norm.BSON;
using System;
namespace Norm.Commands
{
	public class OrQualifier : QualifierCommand
	{
		public OrQualifier(params object[] orCriteriaGroups) : base("$or", orCriteriaGroups)
		{
		}
	}
}
