
using Shared.DataTransferObject;

namespace DomainLayer.Contracts;

public interface IBlockedAttemptLogService
{
    void LogAttempt(BlockedAttemptDto log);
    IEnumerable<BlockedAttemptDto> GetLogs();
}
