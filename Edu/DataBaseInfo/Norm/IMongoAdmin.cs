using Norm.Responses;
using System;
using System.Collections.Generic;
namespace Norm
{
	public interface IMongoAdmin : IDisposable
	{
		IMongoDatabase Database
		{
			get;
		}
		AssertInfoResponse AssertionInfo();
		BuildInfoResponse BuildInfo();
		DroppedDatabaseResponse DropDatabase();
		ForceSyncResponse ForceSync(bool async);
		IEnumerable<DatabaseInfo> GetAllDatabases();
		IEnumerable<CurrentOperationResponse> GetCurrentOperations();
		int GetProfileLevel();
		GenericCommandResponse KillOperation(double operationId);
		PreviousErrorResponse PreviousErrors();
		bool RepairDatabase(bool preserveClonedFilesOnFailure, bool backupOriginalFiles);
		bool ResetLastError();
		ServerStatusResponse ServerStatus();
		bool SetProfileLevel(int value, out int previousLevel);
		bool SetProfileLevel(int value);
	}
}
