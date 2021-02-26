using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class LessThanQualifier : QualifierCommand
	{
		internal LessThanQualifier(object value) : base("$lt", value)
		{
		}
	}
}
