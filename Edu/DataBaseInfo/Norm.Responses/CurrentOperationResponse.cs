using Norm.Configuration;
using System;
using System.Collections.Generic;
namespace Norm.Responses
{
	public class CurrentOperationResponse : BaseStatusMessage
	{
		private Dictionary<string, object> _properties = new Dictionary<string, object>(0);
		public int? OperationId
		{
			get;
			set;
		}
		public string Operation
		{
			get;
			set;
		}
		public string Namespace
		{
			get;
			set;
		}
		public string LockType
		{
			get;
			set;
		}
		public bool WaitingForLock
		{
			get;
			set;
		}
		public bool Active
		{
			get;
			set;
		}
		public string Client
		{
			get;
			set;
		}
		public string Query
		{
			get;
			set;
		}
		public double? InLock
		{
			get;
			set;
		}
		public int? SecondsRunning
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		static CurrentOperationResponse()
		{
			MongoConfiguration.Initialize(delegate(IConfigurationContainer c)
			{
				c.For<CurrentOperationResponse>(delegate(ITypeConfiguration<CurrentOperationResponse> a)
				{
					a.ForProperty((CurrentOperationResponse op) => (object)op.OperationId).UseAlias("opid");
					a.ForProperty((CurrentOperationResponse op) => op.Operation).UseAlias("op");
					a.ForProperty((CurrentOperationResponse op) => op.Namespace).UseAlias("ns");
					a.ForProperty((CurrentOperationResponse op) => (object)op.SecondsRunning).UseAlias("secs_running");
					a.ForProperty((CurrentOperationResponse op) => op.Description).UseAlias("desc");
				});
			});
		}
	}
}
