using Norm.BSON;
using System;
using System.Linq.Expressions;
namespace Norm.Commands.Modifiers
{
	internal class ModifierExpression<T> : IModifierExpression<T>
	{
		public Expando Expression
		{
			get;
			set;
		}
		public ModifierExpression()
		{
			this.Expression = new Expando();
		}
		public void Increment(Expression<Func<T, object>> func, int amountToIncrement)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.Increment(amountToIncrement);
		}
		public void SetValue<X>(Expression<Func<T, object>> func, X rer)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.Set<X>(rer);
		}
		public void Push<X>(Expression<Func<T, object>> func, X valueToPush)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.Push<X>(valueToPush);
		}
		public void PushAll<X>(Expression<Func<T, object>> func, params X[] pushValues)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.PushAll<X>(pushValues);
		}
		public void AddToSet<X>(Expression<Func<T, object>> func, X addToSetValue)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.AddToSet<X>(addToSetValue);
		}
		public void Pull<X>(Expression<Func<T, object>> func, X pullValue)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.Pull<X>(pullValue);
		}
		public void PopFirst(Expression<Func<T, object>> func)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.Pop(PopType.RemoveFirst);
		}
		public void PopLast(Expression<Func<T, object>> func)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.Pop(PopType.RemoveLast);
		}
		public void PullAll<X>(Expression<Func<T, object>> func, params X[] pullValue)
		{
			string propertyName = ReflectionHelper.FindProperty(func);
			this.Expression[propertyName] = M.PullAll<X>(pullValue);
		}
	}
}
