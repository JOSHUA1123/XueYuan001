using Norm.BSON;
using Norm.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Norm.Responses
{
	public class ExplainPlan
	{
		public string Cursor
		{
			get;
			set;
		}
		internal Expando StartKey
		{
			get;
			set;
		}
		internal Expando EndKey
		{
			get;
			set;
		}
		public Dictionary<string, List<object[]>> IndexBounds
		{
			get;
			set;
		}
		public Dictionary<string, string> ExplainStartKey
		{
			get
			{
				Dictionary<string, string> keys = new Dictionary<string, string>();
				if (this.StartKey != null)
				{
					IEnumerable<ExpandoProperty> source = this.StartKey.AllProperties();
					source.ToList<ExpandoProperty>().ForEach(delegate(ExpandoProperty p)
					{
						keys.Add(p.PropertyName, p.Value.ToString());
					});
				}
				return keys;
			}
		}
		public Dictionary<string, string> ExplainEndKey
		{
			get
			{
				Dictionary<string, string> keys = new Dictionary<string, string>();
				if (this.EndKey != null)
				{
					IEnumerable<ExpandoProperty> source = this.EndKey.AllProperties();
					source.ToList<ExpandoProperty>().ForEach(delegate(ExpandoProperty p)
					{
						keys.Add(p.PropertyName, p.Value.ToString());
					});
				}
				return keys;
			}
		}
		static ExplainPlan()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<ExplainPlan>(delegate(ITypeConfiguration<ExplainPlan> a)
				{
					a.ForProperty((ExplainPlan auth) => auth.Cursor).UseAlias("cursor");
					a.ForProperty((ExplainPlan auth) => auth.StartKey).UseAlias("startKey");
					a.ForProperty((ExplainPlan auth) => auth.IndexBounds).UseAlias("indexBounds");
				});
			});
		}
	}
}
