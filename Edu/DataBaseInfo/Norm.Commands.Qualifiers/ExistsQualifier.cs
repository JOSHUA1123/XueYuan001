using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class ExistsQualifier : QualifierCommand
	{
		internal ExistsQualifier(bool doesExist) : base("$exists", doesExist)
		{
		}
	}
}
