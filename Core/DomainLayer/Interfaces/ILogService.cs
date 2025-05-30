using DomainLayer.Models;
using System;

namespace DomainLayer.Interfaces
{
    public interface ILogService
    {
        void LogAttempt(BlockedAttemptLog log);
        IEnumerable<BlockedAttemptLog> GetLogs(int page, int pageSize);

        // Enhanced methods
        IEnumerable<BlockedAttemptLog> GetFilteredLogs(int page, int pageSize, bool? isBlocked = null, string? countryCode = null);
        int GetLogsCount(bool? isBlocked = null, string? countryCode = null);
    }
}
