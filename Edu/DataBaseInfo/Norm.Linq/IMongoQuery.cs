using System;
using System.Linq.Expressions;
namespace Norm.Linq
{
	public interface IMongoQuery
	{
		Expression GetExpression();
	}
}
