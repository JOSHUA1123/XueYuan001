using Norm.BSON;
using System;
namespace Norm.Commands.Qualifiers
{
	public class WhereQualifier : Expando
	{
		public WhereQualifier(string inExpression)
		{
			base["$where"] = inExpression;
		}
	}
}
