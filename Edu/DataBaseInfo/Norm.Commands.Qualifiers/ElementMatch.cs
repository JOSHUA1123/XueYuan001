using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class ElementMatch<T> : QualifierCommand
	{
		public ElementMatch(T matchDoc) : base("$elemMatch", matchDoc)
		{
		}
	}
}
