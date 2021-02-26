using Norm.BSON;
using Norm.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Norm.Responses
{
	public class ExplainResponse : ExplainPlan, IExpando
	{
		private Dictionary<string, object> _properties = new Dictionary<string, object>();
		public int? NumberOfScannedObjects
		{
			get;
			set;
		}
		public int NumberScanned
		{
			get;
			set;
		}
		public int Number
		{
			get;
			set;
		}
		public int Milliseconds
		{
			get;
			set;
		}
		public ExplainPlan OldPlan
		{
			get;
			set;
		}
		public ExplainPlan[] AllPlans
		{
			get;
			set;
		}
		public object this[string propertyName]
		{
			get
			{
				return this._properties[propertyName];
			}
			set
			{
				this._properties[propertyName] = value;
			}
		}
		static ExplainResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<ExplainResponse>(delegate(ITypeConfiguration<ExplainResponse> a)
				{
					a.ForProperty((ExplainResponse auth) => (object)auth.NumberScanned).UseAlias("nscanned");
					a.ForProperty((ExplainResponse auth) => (object)auth.NumberOfScannedObjects).UseAlias("nscannedObjects");
					a.ForProperty((ExplainResponse auth) => (object)auth.Number).UseAlias("n");
					a.ForProperty((ExplainResponse auth) => (object)auth.Milliseconds).UseAlias("millis");
					a.ForProperty((ExplainResponse auth) => auth.OldPlan).UseAlias("oldPlan");
					a.ForProperty((ExplainResponse auth) => auth.AllPlans).UseAlias("allPlans");
				});
			});
		}
		public IEnumerable<ExpandoProperty> AllProperties()
		{
			return 
				from j in this._properties
				select new ExpandoProperty(j.Key, j.Value);
		}
		public void Delete(string propertyName)
		{
			this._properties.Remove(propertyName);
		}
	}
}
