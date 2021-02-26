using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class NotEqualQualifier : QualifierCommand
	{
		internal NotEqualQualifier(object value) : base("$ne", value)
		{
		}
	}
}
