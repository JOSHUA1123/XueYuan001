using Norm.BSON;
using Norm.Commands;
using Norm.Commands.Qualifiers;
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
namespace Norm
{
	public static class Q
	{
		public static object IsNull()
		{
			return null;
		}
		public static NotEqualQualifier IsNotNull()
		{
			return Q.NotEqual<bool?>(null);
		}
		public static Expando And(this QualifierCommand baseCommand, params QualifierCommand[] additionalQualifiers)
		{
			Expando expando = new Expando();
			expando[baseCommand.CommandName] = baseCommand.ValueForCommand;
			for (int i = 0; i < additionalQualifiers.Length; i++)
			{
				QualifierCommand qualifierCommand = additionalQualifiers[i];
				expando[qualifierCommand.CommandName] = qualifierCommand.ValueForCommand;
			}
			return expando;
		}
		public static OrQualifier Or(params object[] orGroups)
		{
			return new OrQualifier(orGroups);
		}
		public static OrQualifier Or(IEnumerable orGroups)
		{
			return Q.Or(orGroups.OfType<object>().ToArray<object>());
		}
		public static SliceQualifier Slice(int index)
		{
			return new SliceQualifier(index);
		}
		public static SliceQualifier Slice(int left, int right)
		{
			return new SliceQualifier(left, right);
		}
		public static WhereQualifier Where(string expression)
		{
			return new WhereQualifier(expression);
		}
		public static LessThanQualifier LessThan(double value)
		{
			return new LessThanQualifier(value);
		}
		public static LessThanQualifier LessThan(object value)
		{
			return new LessThanQualifier(value);
		}
		public static LessOrEqualQualifier LessOrEqual(double value)
		{
			return new LessOrEqualQualifier(value);
		}
		public static LessOrEqualQualifier LessOrEqual(object value)
		{
			return new LessOrEqualQualifier(value);
		}
		public static GreaterOrEqualQualifier GreaterOrEqual(double value)
		{
			return new GreaterOrEqualQualifier(value);
		}
		public static GreaterOrEqualQualifier GreaterOrEqual(object value)
		{
			return new GreaterOrEqualQualifier(value);
		}
		public static GreaterThanQualifier GreaterThan(double value)
		{
			return new GreaterThanQualifier(value);
		}
		public static GreaterThanQualifier GreaterThan(object value)
		{
			return new GreaterThanQualifier(value);
		}
		public static AllQualifier<T> All<T>(params T[] all)
		{
			return new AllQualifier<T>(all);
		}
		public static InQualifier<T> In<T>(params T[] inSet)
		{
			return new InQualifier<T>(inSet);
		}
		public static NotEqualQualifier NotEqual<T>(T test)
		{
			return new NotEqualQualifier(test);
		}
		public static T Equals<T>(T test)
		{
			return test;
		}
		public static SizeQualifier Size(double size)
		{
			return new SizeQualifier(size);
		}
		public static NotInQualifier<T> NotIn<T>(params T[] inSet)
		{
			return new NotInQualifier<T>(inSet);
		}
		public static ElementMatch<T> ElementMatch<T>(T matchDoc)
		{
			return new ElementMatch<T>(matchDoc);
		}
		public static Regex Matches(string pattern)
		{
			return new Regex(pattern);
		}
		public static ExistsQualifier Exists(bool value)
		{
			return new ExistsQualifier(value);
		}
	}
}
