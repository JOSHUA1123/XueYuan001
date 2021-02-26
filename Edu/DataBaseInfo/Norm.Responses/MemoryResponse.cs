using System;
namespace Norm.Responses
{
	public class MemoryResponse
	{
		public int? Resident
		{
			get;
			set;
		}
		public int? Virtual
		{
			get;
			set;
		}
		public long? Mapped
		{
			get;
			set;
		}
		public bool? Supported
		{
			get;
			set;
		}
		public int? Bits
		{
			get;
			set;
		}
	}
}
