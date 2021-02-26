using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Norm.Linq
{
	internal static class PartialEvaluator
	{
		private class SubtreeEvaluator : ExpressionVisitor
		{
			private HashSet<Expression> candidates;
			private SubtreeEvaluator(HashSet<Expression> candidates)
			{
				this.candidates = candidates;
			}
			internal static Expression Eval(HashSet<Expression> candidates, Expression exp)
			{
				return new PartialEvaluator.SubtreeEvaluator(candidates).Visit(exp);
			}
			protected override Expression Visit(Expression exp)
			{
				if (exp == null)
				{
					return null;
				}
				if (this.candidates.Contains(exp))
				{
					return this.Evaluate(exp);
				}
				return base.Visit(exp);
			}
			private Expression Evaluate(Expression e)
			{
				if (e.NodeType == ExpressionType.Constant)
				{
					return e;
				}
				Type type = e.Type;
				if (type.IsValueType)
				{
					e = Expression.Convert(e, typeof(object));
				}
				Expression<Func<object>> expression = Expression.Lambda<Func<object>>(e, new ParameterExpression[0]);
				Func<object> func = expression.Compile();
				return Expression.Constant(func(), type);
			}
		}
		private class Nominator : ExpressionVisitor
		{
			private Func<Expression, bool> fnCanBeEvaluated;
			private HashSet<Expression> candidates;
			private bool cannotBeEvaluated;
			private Nominator(Func<Expression, bool> fnCanBeEvaluated)
			{
				this.candidates = new HashSet<Expression>();
				this.fnCanBeEvaluated = fnCanBeEvaluated;
			}
			internal static HashSet<Expression> Nominate(Func<Expression, bool> fnCanBeEvaluated, Expression expression)
			{
				PartialEvaluator.Nominator nominator = new PartialEvaluator.Nominator(fnCanBeEvaluated);
				nominator.Visit(expression);
				return nominator.candidates;
			}
			protected override Expression Visit(Expression expression)
			{
				if (expression != null)
				{
					bool flag = this.cannotBeEvaluated;
					this.cannotBeEvaluated = false;
					base.Visit(expression);
					if (!this.cannotBeEvaluated)
					{
						if (this.fnCanBeEvaluated(expression))
						{
							this.candidates.Add(expression);
						}
						else
						{
							this.cannotBeEvaluated = true;
						}
					}
					this.cannotBeEvaluated |= flag;
				}
				return expression;
			}
		}
		public static Expression Eval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
		{
			return PartialEvaluator.SubtreeEvaluator.Eval(PartialEvaluator.Nominator.Nominate(fnCanBeEvaluated, expression), expression);
		}
		public static Expression Eval(Expression expression)
		{
			return PartialEvaluator.Eval(expression, new Func<Expression, bool>(PartialEvaluator.CanBeEvaluatedLocally));
		}
		private static bool CanBeEvaluatedLocally(Expression expression)
		{
			return expression.NodeType != ExpressionType.Parameter;
		}
	}
}
