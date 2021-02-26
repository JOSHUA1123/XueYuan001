using System;
using System.Runtime.Serialization;
namespace DataBaseInfo
{
	[Serializable]
	public class SNDataException : Exception
	{
		private ExceptionType exceptionType;
		public ExceptionType ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
		}
		public SNDataException(string msg) : base(msg)
		{
			this.exceptionType = ExceptionType.Unknown;
		}
		public SNDataException(string msg, Exception inner) : base(msg, inner)
		{
			this.exceptionType = ExceptionType.Unknown;
		}
		public SNDataException(ExceptionType t) : base("DataBaseInfo Component Error.")
		{
			this.exceptionType = t;
		}
		public SNDataException(ExceptionType t, string msg) : base(msg)
		{
			this.exceptionType = t;
		}
		public SNDataException(ExceptionType t, string msg, Exception inner) : base(msg, inner)
		{
			this.exceptionType = t;
		}
		protected SNDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.exceptionType = (ExceptionType)info.GetValue("ExceptionType", typeof(ExceptionType));
		}
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ExceptionType", this.exceptionType);
		}
	}
}
