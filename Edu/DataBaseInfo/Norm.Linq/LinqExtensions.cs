using Norm.BSON;
using Norm.Configuration;
using Norm.Protocol.Messages;
using Norm.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Norm.Linq
{
	public static class LinqExtensions
	{
		public static T GetConstantValue<T>(this Expression exp)
		{
			T result = default(T);
			if (exp is ConstantExpression)
			{
				ConstantExpression constantExpression = (ConstantExpression)exp;
				result = (T)((object)constantExpression.Value);
			}
			return result;
		}
		public static ExplainResponse Explain<T>(this IQueryable<T> expression)
		{
			MongoQuery<T> mongoQuery = expression as MongoQuery<T>;
			if (mongoQuery != null)
			{
				MongoQueryTranslator mongoQueryTranslator = new MongoQueryTranslator();
				QueryTranslationResults queryTranslationResults = mongoQueryTranslator.Translate(expression.Expression, false);
				mongoQueryTranslator.CollectionName = mongoQuery.CollectionName;
				return mongoQuery.Explain(queryTranslationResults.Where);
			}
			return null;
		}
		public static IEnumerable<T> Hint<T>(this IEnumerable<T> find, Expression<Func<T, object>> hint, IndexOption direction)
		{
			MongoQueryExecutor<T, Expando> mongoQueryExecutor = (MongoQueryExecutor<T, Expando>)find;
			MongoQueryTranslator mongoQueryTranslator = new MongoQueryTranslator();
			QueryTranslationResults queryTranslationResults = mongoQueryTranslator.Translate(hint);
			mongoQueryTranslator.CollectionName = mongoQueryExecutor.CollectionName;
			mongoQueryExecutor.AddHint(queryTranslationResults.Query, direction);
			return find;
		}
		public static string EscapeJavaScriptString(this string str)
		{
			return str.Replace("\\", "\\\\").Replace("\"", "\\\"");
		}
		public static Expando AsExpando(this QualifierCommand qualifier)
		{
			Expando expando = new Expando();
			expando[qualifier.CommandName] = qualifier.ValueForCommand;
			return expando;
		}
		public static string GetPropertyAlias(this MemberExpression mex)
		{
			string str = "";
			MemberExpression memberExpression = mex.Expression as MemberExpression;
			if (memberExpression != null)
			{
				str = str + memberExpression.GetPropertyAlias() + ".";
			}
			return str + MongoConfiguration.GetPropertyAlias(mex.Expression.Type, mex.Member.Name);
		}
	}
}
