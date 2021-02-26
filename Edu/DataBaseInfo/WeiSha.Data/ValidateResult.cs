using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	public class ValidateResult
	{
		public static readonly ValidateResult Default = new ValidateResult();
		private IList<InvalidValue> invalidValues;
		public bool IsSuccess
		{
			get
			{
				return this.invalidValues.Count == 0;
			}
		}
		public IList<InvalidValue> InvalidValues
		{
			get
			{
				return this.invalidValues;
			}
			private set
			{
				this.invalidValues = value;
			}
		}
		private ValidateResult()
		{
			this.invalidValues = new List<InvalidValue>();
		}
		public ValidateResult(IList<InvalidValue> invalidValues) : this()
		{
			if (invalidValues != null)
			{
				this.invalidValues = invalidValues;
			}
		}
	}
}
