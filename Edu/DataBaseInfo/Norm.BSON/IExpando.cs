using System;
using System.Collections.Generic;
namespace Norm.BSON
{
	public interface IExpando
	{
		object this[string propertyName]
		{
			get;
			set;
		}
		IEnumerable<ExpandoProperty> AllProperties();
		void Delete(string propertyName);
	}
}
