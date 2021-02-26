using Norm.BSON;
using Norm.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace Norm.Linq
{
	internal class MongoQueryTranslator : ExpressionVisitor
	{
		private int _takeCount = 2147483647;
		private string _lastFlyProperty = string.Empty;
		private string _lastOperator = " === ";
		private List<string> _prefixAlias = new List<string>();
		private StringBuilder _sbWhere;
		private bool _whereWritten;
		private bool _isDeepGraphWithArrays;
		private static HashSet<string> _callableMethods = new HashSet<string>
		{
			"First",
			"Single",
			"FirstOrDefault",
			"SingleOrDefault",
			"Count",
			"Sum",
			"Average",
			"Min",
			"Max",
			"Any",
			"Take",
			"Skip",
			"OrderBy",
			"ThenBy",
			"OrderByDescending",
			"ThenByDescending",
			"Where",
			"Select"
		};
		private Expando FlyWeight
		{
			get;
			set;
		}
		private Expando SortFly
		{
			get;
			set;
		}
		private string AggregatePropName
		{
			get;
			set;
		}
		private string TypeName
		{
			get;
			set;
		}
		public string CollectionName
		{
			get;
			set;
		}
		private string MethodCall
		{
			get;
			set;
		}
		private bool IsComplex
		{
			get;
			set;
		}
		private int ConditionalCount
		{
			get;
			set;
		}
		private int Skip
		{
			get;
			set;
		}
		private int Take
		{
			get
			{
				return this._takeCount;
			}
			set
			{
				this._takeCount = value;
			}
		}
		private string WhereExpression
		{
			get
			{
				return this._sbWhere.ToString();
			}
		}
		public bool UseScopedQualifier
		{
			get;
			set;
		}
		private Type OriginalSelectType
		{
			get;
			set;
		}
		private LambdaExpression SelectLambda
		{
			get;
			set;
		}
		public QueryTranslationResults Translate(Expression exp)
		{
			return this.Translate(exp, true);
		}
		public QueryTranslationResults Translate(Expression exp, bool useScopedQualifier)
		{
			this.UseScopedQualifier = useScopedQualifier;
			this._sbWhere = new StringBuilder();
			this.FlyWeight = new Expando();
			this.SortFly = new Expando();
			this.Visit(exp);
			this.ProcessGuards();
			this.TransformToFlyWeightWhere();
			return new QueryTranslationResults
			{
				Where = this.FlyWeight,
				Sort = this.SortFly,
				Skip = this.Skip,
				Take = this.Take,
				CollectionName = this.CollectionName,
				MethodCall = this.MethodCall,
				AggregatePropName = this.AggregatePropName,
				IsComplex = this.IsComplex,
				TypeName = this.TypeName,
				Query = this.WhereExpression,
				Select = this.SelectLambda,
				OriginalSelectType = this.OriginalSelectType
			};
		}
		private void ProcessGuards()
		{
			if (!this._isDeepGraphWithArrays || !this.IsComplex)
			{
				return;
			}
			string[] array = new string[]
			{
				"Max",
				"Min",
				"Sum",
				"Average"
			};
			if (array.Contains(this.MethodCall))
			{
				throw new NotSupportedException("You cannot use deep graph resolution when using the following aggregates: " + string.Join(", ", array));
			}
			throw new NotSupportedException("You cannot use deep graph resolution if the query is considered complex");
		}
		private void TransformToFlyWeightWhere()
		{
			string whereExpression = this.WhereExpression;
			if (!string.IsNullOrEmpty(whereExpression) && this.IsComplex)
			{
				this.FlyWeight = new Expando();
				if (whereExpression.StartsWith("function"))
				{
					this.FlyWeight["$where"] = whereExpression;
					return;
				}
				this.FlyWeight["$where"] = " function(){return " + whereExpression + ";}";
			}
		}
		protected override Expression VisitMemberAccess(MemberExpression m)
		{
			if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
			{
				string text = this.VisitAlias(m);
				this.VisitDateTimeProperty(m);
				if (this.UseScopedQualifier)
				{
					this._sbWhere.Append("this.");
				}
				this._sbWhere.Append(text);
				this._lastFlyProperty = text;
				return m;
			}
			if (m.Member.DeclaringType == typeof(string))
			{
				string name;
				if ((name = m.Member.Name) != null && name == "Length")
				{
					this.IsComplex = true;
					this.Visit(m.Expression);
					this._sbWhere.Append(".length");
					return m;
				}
			}
			else
			{
				if (!(m.Member.DeclaringType == typeof(DateTime)) && !(m.Member.DeclaringType == typeof(DateTimeOffset)))
				{
					string text2 = this.VisitDeepAlias(m);
					this.VisitDateTimeProperty(m);
					if (this.UseScopedQualifier)
					{
						this._sbWhere.Append("this.");
					}
					this._sbWhere.Append(text2);
					this._lastFlyProperty = text2;
					return m;
				}
				this.IsComplex = true;
				string name2;
				switch (name2 = m.Member.Name)
				{
				case "Day":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getDate()");
					return m;
				case "Month":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getMonth()");
					return m;
				case "Year":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getFullYear()");
					return m;
				case "Hour":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getHours()");
					return m;
				case "Minute":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getMinutes()");
					return m;
				case "Second":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getSeconds()");
					return m;
				case "DayOfWeek":
					this.Visit(m.Expression);
					this._sbWhere.Append(".getDay()");
					return m;
				}
			}
			throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
		}
		private string VisitDeepAlias(MemberExpression m)
		{
			string[] array = m.ToString().Split(new char[]
			{
				'.'
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = (
				from x in array.Skip(1)
				select Regex.Replace(x, "^get_Item\\(([0-9]+)\\)$", "$1|Ind") into x
				select Regex.Replace(x, "\\[([0-9]+)\\]$", "$1|Ind") into x
				select x.Replace("First()", "0|Ind")).ToArray<string>();
			if (!this._isDeepGraphWithArrays)
			{
				this._isDeepGraphWithArrays = (array.Length - array2.Length != 1);
			}
			ParameterExpression parameterExpression = MongoQueryTranslator.GetParameterExpression(m.Expression);
			if (parameterExpression != null)
			{
				array2 = MongoQueryTranslator.GetDeepAlias(parameterExpression.Type, array2);
			}
			return string.Join(".", (
				from x in array2
				select x.Replace("|Ind", "")).ToArray<string>());
		}
		private string VisitAlias(MemberExpression m)
		{
			string text = MongoConfiguration.GetPropertyAlias(m.Expression.Type, m.Member.Name);
			MagicProperty magicProperty = ReflectionHelper.GetHelperForType(m.Expression.Type).FindIdProperty();
			if (magicProperty != null && magicProperty.Name == text)
			{
				text = "_id";
			}
			return text;
		}
		private void VisitDateTimeProperty(MemberExpression m)
		{
			if (m.Member.MemberType == MemberTypes.Property || m.Member.MemberType == MemberTypes.Field)
			{
				PropertyInfo propertyInfo = m.Member as PropertyInfo;
				if (propertyInfo != null && (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)))
				{
					this._sbWhere.Append("+");
				}
			}
		}
		private string GetOperator(UnaryExpression u)
		{
			switch (u.NodeType)
			{
			case ExpressionType.Negate:
			case ExpressionType.NegateChecked:
				return "-";
			case ExpressionType.UnaryPlus:
				return "+";
			case ExpressionType.Not:
				if (!this.IsBoolean(u.Operand.Type))
				{
					return "";
				}
				return "!";
			}
			return "";
		}
		protected override Expression VisitUnary(UnaryExpression u)
		{
			string @operator = this.GetOperator(u);
			ExpressionType nodeType = u.NodeType;
			if (nodeType != ExpressionType.Convert)
			{
				switch (nodeType)
				{
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
					this._sbWhere.Append(@operator);
					this.Visit(u.Operand);
					return u;
				case ExpressionType.UnaryPlus:
					this.Visit(u.Operand);
					return u;
				case ExpressionType.Not:
					if (this.IsBoolean(u.Operand.Type))
					{
						this._sbWhere.Append(@operator);
						this.VisitPredicate(u.Operand, true);
						return u;
					}
					this._sbWhere.Append(@operator);
					this.Visit(u.Operand);
					return u;
				}
				throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
			}
			this.Visit(u.Operand);
			return u;
		}
		private bool IsBoolean(Type type)
		{
			return type == typeof(bool) || type == typeof(bool?);
		}
		private bool IsPredicate(Expression expr)
		{
			ExpressionType nodeType = expr.NodeType;
			return (nodeType == ExpressionType.Convert || nodeType == ExpressionType.MemberAccess) && this.IsBoolean(expr.Type);
		}
		private Expression VisitPredicate(Expression expr, bool IsNotOperator)
		{
			this.Visit(expr);
			if (this.IsPredicate(expr))
			{
				this.SetFlyValue(!IsNotOperator);
			}
			return expr;
		}
		private Expression VisitPredicate(Expression expr)
		{
			return this.VisitPredicate(expr, false);
		}
		private static ParameterExpression GetParameterExpression(Expression expression)
		{
			bool flag = false;
			Expression expression2 = expression;
			while (!flag)
			{
				if (expression2.NodeType == ExpressionType.MemberAccess)
				{
					expression2 = ((MemberExpression)expression2).Expression;
					flag = (expression2 is ParameterExpression);
				}
				else
				{
					if (expression2.NodeType == ExpressionType.ArrayIndex)
					{
						expression2 = ((BinaryExpression)expression2).Left;
						flag = (expression2 is ParameterExpression);
					}
					else
					{
						if (expression2.NodeType == ExpressionType.Call)
						{
							Expression expression3 = ((MethodCallExpression)expression2).Arguments[0];
							if (expression3.NodeType == ExpressionType.MemberAccess)
							{
								expression2 = ((MemberExpression)expression3).Expression;
							}
							else
							{
								if (expression3.NodeType == ExpressionType.Constant)
								{
									expression2 = ((MemberExpression)((MethodCallExpression)expression2).Object).Expression;
								}
							}
							flag = (expression2 is ParameterExpression);
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			return (ParameterExpression)expression2;
		}
		private static string[] GetDeepAlias(Type type, string[] graph)
		{
			string[] array = new string[graph.Length];
			Type type2 = type;
			for (int i = 0; i < graph.Length; i++)
			{
				if (graph[i].EndsWith("|Ind"))
				{
					array[i] = graph[i];
				}
				else
				{
					PropertyInfo propertyInfo = ReflectionHelper.FindProperty(type2, graph[i]);
					array[i] = MongoConfiguration.GetPropertyAlias(type2, graph[i]);
					if (propertyInfo.PropertyType.IsGenericType)
					{
						type2 = propertyInfo.PropertyType.GetGenericArguments()[0];
					}
					else
					{
						type2 = (propertyInfo.PropertyType.HasElementType ? propertyInfo.PropertyType.GetElementType() : propertyInfo.PropertyType);
					}
				}
			}
			return array;
		}
		private void VisitBinaryOperator(BinaryExpression b)
		{
			ExpressionType nodeType = b.NodeType;
			string value;
			switch (nodeType)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
				this._lastOperator = " + ";
				value = this._lastOperator;
				this.IsComplex = true;
				goto IL_279;
			case ExpressionType.And:
				value = " & ";
				this.IsComplex = true;
				goto IL_279;
			case ExpressionType.AndAlso:
				value = " && ";
				goto IL_279;
			case ExpressionType.ArrayLength:
			case ExpressionType.ArrayIndex:
			case ExpressionType.Call:
			case ExpressionType.Conditional:
			case ExpressionType.Constant:
			case ExpressionType.Convert:
			case ExpressionType.ConvertChecked:
			case ExpressionType.Invoke:
			case ExpressionType.Lambda:
			case ExpressionType.ListInit:
			case ExpressionType.MemberAccess:
			case ExpressionType.MemberInit:
			case ExpressionType.Modulo:
				break;
			case ExpressionType.Coalesce:
				this._lastOperator = " || ";
				value = this._lastOperator;
				this.IsComplex = true;
				goto IL_279;
			case ExpressionType.Divide:
				this._lastOperator = " / ";
				value = this._lastOperator;
				this.IsComplex = true;
				goto IL_279;
			case ExpressionType.Equal:
				this._lastOperator = " === ";
				value = this._lastOperator;
				goto IL_279;
			case ExpressionType.ExclusiveOr:
				this._lastOperator = " ^ ";
				value = this._lastOperator;
				this.IsComplex = true;
				goto IL_279;
			case ExpressionType.GreaterThan:
				this._lastOperator = " > ";
				value = this._lastOperator;
				goto IL_279;
			case ExpressionType.GreaterThanOrEqual:
				this._lastOperator = " >= ";
				value = this._lastOperator;
				goto IL_279;
			case ExpressionType.LeftShift:
				this._lastOperator = " << ";
				value = this._lastOperator;
				this.IsComplex = true;
				goto IL_279;
			case ExpressionType.LessThan:
				this._lastOperator = " < ";
				value = this._lastOperator;
				goto IL_279;
			case ExpressionType.LessThanOrEqual:
				this._lastOperator = " <= ";
				value = this._lastOperator;
				goto IL_279;
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
				this._lastOperator = " * ";
				value = this._lastOperator;
				this.IsComplex = true;
				goto IL_279;
			default:
				switch (nodeType)
				{
				case ExpressionType.NotEqual:
					this._lastOperator = " !== ";
					value = this._lastOperator;
					goto IL_279;
				case ExpressionType.Or:
					value = " | ";
					this.IsComplex = true;
					goto IL_279;
				case ExpressionType.OrElse:
					value = " || ";
					this.IsComplex = true;
					goto IL_279;
				case ExpressionType.RightShift:
					this._lastOperator = " >> ";
					value = this._lastOperator;
					this.IsComplex = true;
					goto IL_279;
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
					this._lastOperator = " - ";
					value = this._lastOperator;
					this.IsComplex = true;
					goto IL_279;
				}
				break;
			}
			throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
			IL_279:
			this._sbWhere.Append(value);
		}
		protected override Expression VisitBinary(BinaryExpression b)
		{
			this.ConditionalCount++;
			this._sbWhere.Append("(");
			bool flag = false;
			ExpressionType nodeType = b.NodeType;
			switch (nodeType)
			{
			case ExpressionType.And:
			case ExpressionType.AndAlso:
				break;
			default:
				switch (nodeType)
				{
				case ExpressionType.Or:
				case ExpressionType.OrElse:
					break;
				default:
					goto IL_81;
				}
				break;
			}
			if (this.IsBoolean(b.Left.Type))
			{
				this.VisitPredicate(b.Left);
				this.VisitBinaryOperator(b);
				this.VisitPredicate(b.Right);
				flag = true;
			}
			IL_81:
			if (!flag)
			{
				this.Visit(b.Left);
				this.VisitBinaryOperator(b);
				this.Visit(b.Right);
			}
			this._sbWhere.Append(")");
			return b;
		}
		protected override Expression VisitConstant(ConstantExpression c)
		{
			IQueryable queryable = c.Value as IQueryable;
			if (queryable != null)
			{
				this.TypeName = queryable.ElementType.Name;
				IMongoQuery mongoQuery = (IMongoQuery)c.Value;
				Expression expression = mongoQuery.GetExpression();
				if (expression.NodeType == ExpressionType.Call)
				{
					this.VisitMethodCall(expression as MethodCallExpression);
				}
			}
			else
			{
				if (c.Value == null)
				{
					this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
					this.SetFlyValue(null);
				}
				else
				{
					TypeCode typeCode = Type.GetTypeCode(c.Value.GetType());
					switch (typeCode)
					{
					case TypeCode.Object:
						if (c.Value is ObjectId)
						{
							if (this._lastOperator == " === " || this._lastOperator == " !== ")
							{
								this._sbWhere.Remove(this._sbWhere.Length - 2, 1);
							}
							this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
							this.SetFlyValue(c.Value);
							return c;
						}
						if (c.Value is Guid)
						{
							this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
							this.SetFlyValue(c.Value);
							return c;
						}
						throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
					case TypeCode.DBNull:
						break;
					case TypeCode.Boolean:
						this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
						this.SetFlyValue(c.Value);
						return c;
					default:
						switch (typeCode)
						{
						case TypeCode.DateTime:
							this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
							this.SetFlyValue(c.Value);
							return c;
						case TypeCode.String:
							this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
							this.SetFlyValue(c.Value);
							return c;
						}
						break;
					}
					this._sbWhere.Append(this.GetJavaScriptConstantValue(c.Value));
					this.SetFlyValue(c.Value);
				}
			}
			return c;
		}
		private string GetJavaScriptConstantValue(object value)
		{
			if (value == null)
			{
				return "null";
			}
			TypeCode typeCode = Type.GetTypeCode(value.GetType());
			switch (typeCode)
			{
			case TypeCode.Object:
				if (value is ObjectId || value is Guid)
				{
					return string.Format("\"{0}\"", value);
				}
				throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));
			case TypeCode.DBNull:
				break;
			case TypeCode.Boolean:
				if (!(bool)value)
				{
					return "false";
				}
				return "true";
			default:
				switch (typeCode)
				{
				case TypeCode.DateTime:
					return "+(" + (long)((DateTime)value).ToUniversalTime().Subtract(BsonHelper.EPOCH).TotalMilliseconds + ")";
				case TypeCode.String:
					return "\"" + value.ToString().EscapeJavaScriptString() + "\"";
				}
				break;
			}
			return value.ToString();
		}
		protected override Expression VisitMethodCall(MethodCallExpression m)
		{
			if (string.IsNullOrEmpty(this.MethodCall))
			{
				this.MethodCall = m.Method.Name;
			}
			if (m.Method.DeclaringType == typeof(string))
			{
				string name;
				switch (name = m.Method.Name)
				{
				case "StartsWith":
				{
					string constantValue = m.Arguments[0].GetConstantValue<string>();
					this._sbWhere.Append("(");
					this.Visit(m.Object);
					this._sbWhere.AppendFormat(".indexOf(\"{0}\")===0)", constantValue.EscapeJavaScriptString());
					this.SetFlyValue(new Regex("^" + Regex.Escape(constantValue)));
					return m;
				}
				case "EndsWith":
				{
					string constantValue2 = m.Arguments[0].GetConstantValue<string>();
					this._sbWhere.Append("((");
					this.Visit(m.Object);
					this._sbWhere.AppendFormat(".length - {0}) >= 0 && ", constantValue2.Length);
					this.Visit(m.Object);
					this._sbWhere.AppendFormat(".lastIndexOf(\"{0}\") === (", constantValue2.EscapeJavaScriptString());
					this.Visit(m.Object);
					this._sbWhere.AppendFormat(".length - {0}))", constantValue2.Length);
					this.SetFlyValue(new Regex(Regex.Escape(constantValue2) + "$"));
					return m;
				}
				case "Contains":
				{
					string constantValue3 = m.Arguments[0].GetConstantValue<string>();
					this._sbWhere.Append("(");
					this.Visit(m.Object);
					this._sbWhere.AppendFormat(".indexOf(\"{0}\")>-1)", constantValue3.EscapeJavaScriptString());
					this.SetFlyValue(new Regex(Regex.Escape(constantValue3)));
					return m;
				}
				case "IndexOf":
					this.Visit(m.Object);
					this._sbWhere.Append(".indexOf(");
					this.Visit(m.Arguments[0]);
					this._sbWhere.Append(")");
					this.IsComplex = true;
					return m;
				case "LastIndexOf":
					this.Visit(m.Object);
					this._sbWhere.Append(".lastIndexOf(");
					this.Visit(m.Arguments[0]);
					this._sbWhere.Append(")");
					this.IsComplex = true;
					return m;
				case "IsNullOrEmpty":
					this._sbWhere.Append("(");
					this.Visit(m.Arguments[0]);
					this._sbWhere.Append(" == '' ||  ");
					this.Visit(m.Arguments[0]);
					this._sbWhere.Append(" == null  )");
					this.IsComplex = true;
					return m;
				case "ToLower":
				case "ToLowerInvariant":
					this.Visit(m.Object);
					this._sbWhere.Append(".toLowerCase()");
					this.IsComplex = true;
					return m;
				case "ToUpper":
				case "ToUpperInvariant":
					this.Visit(m.Object);
					this._sbWhere.Append(".toUpperCase()");
					this.IsComplex = true;
					return m;
				case "Substring":
					this.Visit(m.Object);
					this._sbWhere.Append(".substr(");
					this.Visit(m.Arguments[0]);
					if (m.Arguments.Count == 2)
					{
						this._sbWhere.Append(",");
						this.Visit(m.Arguments[1]);
					}
					this._sbWhere.Append(")");
					this.IsComplex = true;
					return m;
				case "Replace":
					this.Visit(m.Object);
					this._sbWhere.Append(".replace(new RegExp(");
					this._sbWhere.Append(this.GetJavaScriptConstantValue(Regex.Escape(m.Arguments[0].GetConstantValue<string>())));
					this._sbWhere.Append(",'g'),");
					this.Visit(m.Arguments[1]);
					this._sbWhere.Append(")");
					this.IsComplex = true;
					return m;
				}
			}
			else
			{
				if (m.Method.DeclaringType == typeof(Regex))
				{
					if (m.Method.Name == "IsMatch")
					{
						this.HandleRegexIsMatch(m);
						return m;
					}
					throw new NotSupportedException(string.Format("Only the static Regex.IsMatch is supported.", m.Method.Name));
				}
				else
				{
					if (!(m.Method.DeclaringType == typeof(DateTime)))
					{
						if (m.Method.DeclaringType == typeof(Queryable) && MongoQueryTranslator.IsCallableMethod(m.Method.Name))
						{
							return this.HandleMethodCall(m);
						}
						if (typeof(IEnumerable).IsAssignableFrom(m.Method.DeclaringType))
						{
							if (m.Method.Name == "Contains")
							{
								this.HandleContains(m);
								return m;
							}
							throw new NotSupportedException(string.Format("Subqueries with {0} are not currently supported", m.Method.Name));
						}
						else
						{
							if (typeof(Enumerable).IsAssignableFrom(m.Method.DeclaringType))
							{
								if (m.Method.Name == "Count" && m.Arguments.Count == 1)
								{
									this.HandleSubCount(m);
									return m;
								}
								if (m.Method.Name == "Any")
								{
									this.HandleSubAny(m);
									return m;
								}
								throw new NotSupportedException(string.Format("Subqueries with {0} are not currently supported", m.Method.Name));
							}
						}
					}
				}
			}
			throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
		}
		private static bool IsCallableMethod(string methodName)
		{
			return MongoQueryTranslator._callableMethods.Contains(methodName);
		}
		private void SetFlyValue(object value)
		{
			if (this._prefixAlias.Count > 0)
			{
				this._lastFlyProperty = (string.Join(".", this._prefixAlias.ToArray()) + "." + this._lastFlyProperty).TrimEnd(new char[]
				{
					'.'
				});
			}
			this.SetFlyValue(this._lastFlyProperty, value);
		}
		private void SetFlyValue(string key, object value)
		{
			if (!this.CanGetQualifier(this._lastOperator, value))
			{
				this.IsComplex = true;
				return;
			}
			if (this.FlyWeight.Contains(key))
			{
				Expando expando = this.FlyWeight[key] as Expando;
				if (expando != null)
				{
					Expando expando2 = this.GetQualifier(this._lastOperator, value) as Expando;
					if (expando2 != null)
					{
						expando.Merge(expando2);
						return;
					}
				}
				this.IsComplex = true;
				return;
			}
			this.FlyWeight[key] = this.GetQualifier(this._lastOperator, value);
		}
		private bool CanGetQualifier(string op, object value)
		{
			return op == " !== " || op == " === " || (value != null && (value.GetType().IsAssignableFrom(typeof(double)) || value.GetType().IsAssignableFrom(typeof(double?)) || value.GetType().IsAssignableFrom(typeof(int)) || value.GetType().IsAssignableFrom(typeof(int?)) || value.GetType().IsAssignableFrom(typeof(long)) || value.GetType().IsAssignableFrom(typeof(long?)) || value.GetType().IsAssignableFrom(typeof(float)) || value.GetType().IsAssignableFrom(typeof(float?)) || value.GetType().IsAssignableFrom(typeof(DateTime)) || value.GetType().IsAssignableFrom(typeof(DateTime?))) && op != null && (op == " > " || op == " < " || op == " <= " || op == " >= "));
		}
		private object GetQualifier(string op, object value)
		{
			if (op != null)
			{
				if (op == " === ")
				{
					return value;
				}
				if (op == " !== ")
				{
					return Q.NotEqual<object>(value).AsExpando();
				}
				if (op == " > ")
				{
					return Q.GreaterThan(value).AsExpando();
				}
				if (op == " < ")
				{
					return Q.LessThan(value).AsExpando();
				}
				if (op == " <= ")
				{
					return Q.LessOrEqual(value).AsExpando();
				}
				if (op == " >= ")
				{
					return Q.GreaterOrEqual(value).AsExpando();
				}
			}
			return null;
		}
		private void HandleSkip(Expression exp)
		{
			this.Skip = exp.GetConstantValue<int>();
		}
		private void HandleTake(Expression exp)
		{
			this.Take = exp.GetConstantValue<int>();
		}
		private void HandleSort(Expression exp, OrderBy orderby)
		{
			LambdaExpression lambda = MongoQueryTranslator.GetLambda(exp);
			MemberExpression memberExpression = lambda.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new NotSupportedException("Order clause supplied is not supported");
			}
			this.SortFly[this.VisitDeepAlias(memberExpression)] = orderby;
		}
		private void HandleAggregate(MethodCallExpression exp)
		{
			if (exp.Arguments.Count == 2)
			{
				LambdaExpression lambda = MongoQueryTranslator.GetLambda(exp.Arguments[1]);
				MemberExpression memberExpression = lambda.Body as MemberExpression;
				if (memberExpression == null)
				{
					throw new NotSupportedException("Aggregate clause supplied is not supported");
				}
				this.AggregatePropName = this.VisitDeepAlias(memberExpression);
			}
		}
		private void TranslateToWhere(MethodCallExpression exp)
		{
			if (exp.Arguments.Count == 2)
			{
				this.HandleWhere(exp.Arguments[1]);
			}
		}
		private void HandleWhere(Expression exp)
		{
			if (this._whereWritten)
			{
				this._sbWhere.Append(" && ");
			}
			this.VisitPredicate(MongoQueryTranslator.GetLambda(exp).Body);
			this._whereWritten = true;
		}
		private void HandleContains(MethodCallExpression m)
		{
			object[] array = m.Object.GetConstantValue<IEnumerable>().Cast<object>().ToArray<object>();
			string text = this.VisitDeepAlias((MemberExpression)m.Arguments[0]);
			if (array.Length > 0)
			{
				this._sbWhere.Append("(");
				object[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					object value = array2[i];
					if (this.UseScopedQualifier)
					{
						this._sbWhere.Append("this.");
					}
					this._sbWhere.Append(text);
					this._sbWhere.Append(" === ");
					this._sbWhere.Append(this.GetJavaScriptConstantValue(value));
					this._sbWhere.Append(" || ");
				}
				this._sbWhere.Remove(this._sbWhere.Length - 4, 4);
				this._sbWhere.Append(")");
			}
			else
			{
				this._sbWhere.Append("(1===2)");
			}
			this.SetFlyValue(text, Q.In<object>(array).AsExpando());
		}
		private void HandleSubCount(MethodCallExpression m)
		{
			this.Visit(m.Arguments[0]);
			this._sbWhere.Append(".length");
			this.IsComplex = true;
		}
		private void HandleSubAny(MethodCallExpression m)
		{
			if (m.Arguments.Count == 1)
			{
				this.Visit(m.Arguments[0]);
				this._sbWhere.Append(".length > 0");
				this.IsComplex = true;
				return;
			}
			if (m.Arguments.Count == 2)
			{
				this._prefixAlias.Add(this.VisitDeepAlias((MemberExpression)m.Arguments[0]));
				this.VisitPredicate(MongoQueryTranslator.GetLambda(m.Arguments[1]).Body);
				this._prefixAlias.RemoveAt(this._prefixAlias.Count - 1);
				if (this.IsComplex)
				{
					throw new NotSupportedException("Subqueries with Any are not supported with complex queries");
				}
			}
		}
		private void HandleRegexIsMatch(MethodCallExpression m)
		{
			RegexOptions options = RegexOptions.None;
			string arg = "g";
			if (m.Arguments.Count == 3)
			{
				options = m.Arguments[2].GetConstantValue<RegexOptions>();
				arg = MongoQueryTranslator.VisitRegexOptions(m, options);
			}
			string constantValue = m.Arguments[1].GetConstantValue<string>();
			this._sbWhere.AppendFormat("(new RegExp(\"{0}\",\"{1}\")).test(", constantValue.EscapeJavaScriptString(), arg);
			this.Visit(m.Arguments[0]);
			this._sbWhere.Append(")");
			this.SetFlyValue(new Regex(constantValue, options));
		}
		private static string VisitRegexOptions(MethodCallExpression m, RegexOptions options)
		{
			RegexOptions[] array = new RegexOptions[3];
			array[0] = RegexOptions.IgnoreCase;
			array[1] = RegexOptions.Multiline;
			RegexOptions[] source = array;
			foreach (RegexOptions regexOptions in Enum.GetValues(typeof(RegexOptions)))
			{
				if ((options & regexOptions) == regexOptions && !source.Contains(regexOptions))
				{
					throw new NotSupportedException(string.Format("Only the RegexOptions.Ignore and RegexOptions.Multiline options are supported.", m.Method.Name));
				}
			}
			string text = "g";
			if ((options & RegexOptions.IgnoreCase) == RegexOptions.IgnoreCase)
			{
				text += "i";
			}
			if ((options & RegexOptions.Multiline) == RegexOptions.Multiline)
			{
				text += "m";
			}
			return text;
		}
		private void HandleSelect(MethodCallExpression m)
		{
			this.SelectLambda = MongoQueryTranslator.GetLambda(m.Arguments[1]);
			this.OriginalSelectType = this.SelectLambda.Parameters[0].Type;
		}
		private Expression HandleMethodCall(MethodCallExpression m)
		{
			string name;
			switch (name = m.Method.Name)
			{
			case "Any":
			case "Single":
			case "SingleOrDefault":
			case "First":
			case "FirstOrDefault":
			case "Where":
				this.TranslateToWhere(m);
				goto IL_21E;
			case "OrderBy":
			case "ThenBy":
				this.HandleSort(m.Arguments[1], OrderBy.Ascending);
				goto IL_21E;
			case "OrderByDescending":
			case "ThenByDescending":
				this.HandleSort(m.Arguments[1], OrderBy.Descending);
				goto IL_21E;
			case "Skip":
				this.HandleSkip(m.Arguments[1]);
				goto IL_21E;
			case "Take":
				this.HandleTake(m.Arguments[1]);
				goto IL_21E;
			case "Min":
			case "Max":
			case "Sum":
			case "Average":
				this.HandleAggregate(m);
				goto IL_21E;
			case "Select":
				this.HandleSelect(m);
				goto IL_21E;
			}
			this.Take = 1;
			this.MethodCall = m.Method.Name;
			if (m.Arguments.Count > 1)
			{
				LambdaExpression lambda = MongoQueryTranslator.GetLambda(m.Arguments[1]);
				if (lambda != null)
				{
					this.Visit(lambda.Body);
				}
			}
			IL_21E:
			this.Visit(m.Arguments[0]);
			return m;
		}
		private static LambdaExpression GetLambda(Expression e)
		{
			while (e.NodeType == ExpressionType.Quote)
			{
				e = ((UnaryExpression)e).Operand;
			}
			if (e.NodeType == ExpressionType.Constant)
			{
				return ((ConstantExpression)e).Value as LambdaExpression;
			}
			return e as LambdaExpression;
		}
	}
}
