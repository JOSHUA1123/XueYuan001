using System;
namespace Norm.BSON
{
	public class DocumentExceedsSizeLimitsException<T> : Exception
	{
		public int DocumentSize
		{
			get;
			private set;
		}
		public T Document
		{
			get;
			private set;
		}
		public DocumentExceedsSizeLimitsException(T document, int size)
		{
			this.DocumentSize = size;
			this.Document = document;
		}
	}
}
