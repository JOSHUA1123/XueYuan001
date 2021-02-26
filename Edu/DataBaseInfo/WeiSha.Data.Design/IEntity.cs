using System;
namespace DataBaseInfo.Design
{
	public interface IEntity
	{
		void Attach(params Field[] removeFields);
		void AttachSet(params Field[] setFields);
		void AttachAll(params Field[] removeFields);
		void Detach(params Field[] removeFields);
		void DetachSet(params Field[] setFields);
		void DetachAll(params Field[] removeFields);
	}
}
