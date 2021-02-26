using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	public class Validator<T> where T : Entity
	{
		private T entity;
		private List<Field> vlist;
		private IList<InvalidValue> invalidValue;
		public ValidateResult Result
		{
			get
			{
				return new ValidateResult(this.invalidValue);
			}
		}
		public Validator(T entity)
		{
			this.entity = entity;
			this.invalidValue = new List<InvalidValue>();
			if (entity.As<IEntityBase>().GetObjectState() == EntityState.Insert)
			{
				this.vlist = entity.GetFieldValues().FindAll((FieldValue fv) => !fv.IsChanged).ConvertAll<Field>((FieldValue fv) => fv.Field);
				return;
			}
			this.vlist = entity.GetFieldValues().FindAll((FieldValue fv) => fv.IsChanged).ConvertAll<Field>((FieldValue fv) => fv.Field);
		}
		public Validator<T> Check(Predicate<T> predicate, Field field, string message)
		{
			if (this.vlist.Exists((Field p) => p.Name == field.Name) && predicate(this.entity))
			{
				this.invalidValue.Add(new InvalidValue
				{
					Field = field,
					Message = message
				});
			}
			return this;
		}
	}
}
