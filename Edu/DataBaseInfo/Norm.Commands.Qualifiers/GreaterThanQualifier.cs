using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class GreaterThanQualifier : QualifierCommand
	{
		internal GreaterThanQualifier(object value) : base("$gt", value)
		{
		}
	}
}
