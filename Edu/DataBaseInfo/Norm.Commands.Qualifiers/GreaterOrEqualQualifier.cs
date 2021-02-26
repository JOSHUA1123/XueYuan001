using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class GreaterOrEqualQualifier : QualifierCommand
	{
		internal GreaterOrEqualQualifier(object value) : base("$gte", value)
		{
		}
	}
}
