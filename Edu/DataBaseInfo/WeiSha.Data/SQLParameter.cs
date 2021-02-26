using System;
using System.Data;
using System.Data.Common;
namespace DataBaseInfo
{
	[Serializable]
	public class SQLParameter
	{
		public string Name
		{
			get;
			set;
		}
		public object Value
		{
			get;
			set;
		}
		public ParameterDirection Direction
		{
			get;
			set;
		}
		public SQLParameter()
		{
		}
		public SQLParameter(string pName)
		{
			this.Name = pName;
			this.Direction = ParameterDirection.Input;
		}
		public SQLParameter(string pName, object pValue) : this(pName)
		{
			this.Value = pValue;
		}
		public SQLParameter(string pName, object pValue, ParameterDirection pDirection) : this(pName, pValue)
		{
			this.Direction = pDirection;
		}
		public SQLParameter(DbParameter dbParameter)
		{
			this.Name = dbParameter.ParameterName;
			this.Value = dbParameter.Value;
			this.Direction = dbParameter.Direction;
		}
	}
}
