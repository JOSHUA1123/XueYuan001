using System;
namespace Norm.Protocol
{
	public enum MongoOp
	{
		Reply = 1,
		Message = 1000,
		Update = 2001,
		Insert,
		GetByOID,
		Query,
		GetMore,
		Delete,
		KillCursors
	}
}
