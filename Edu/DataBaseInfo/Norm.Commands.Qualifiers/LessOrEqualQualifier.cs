using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class LessOrEqualQualifier : QualifierCommand
	{
		internal LessOrEqualQualifier(object value) : base("$lte", value)
		{
		}
	}
}
