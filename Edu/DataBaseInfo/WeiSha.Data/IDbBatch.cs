using System;
using System.Collections.Generic;
namespace DataBaseInfo
{
	internal interface IDbBatch : IDbProcess
	{
		int Execute(out IList<Exception> errors);
	}
}
