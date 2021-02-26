using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class SizeQualifier : QualifierCommand
	{
		internal SizeQualifier(double value) : base("$size", value)
		{
		}
	}
}
