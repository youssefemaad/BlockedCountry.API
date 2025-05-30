using DomainLayer.Interfaces;
using DomainLayer.Models;
using System;
using System.Linq;

namespace Infrastructure.Services
{
    public class InMemoryLogService : ILogService
    {
        private readonly List<BlockedAttemptLog> _logs = new();

        public void LogAttempt(BlockedAttemptLog log)
        {
            _logs.Add(log);
        }

        public IEnumerable<BlockedAttemptLog> GetLogs(int page, int pageSize)
        {
            return _logs
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
        public IEnumerable<BlockedAttemptLog> GetFilteredLogs(int page, int pageSize, bool? isBlocked = null, string? countryCode = null)
        {
            var query = _logs.AsQueryable();

            if (isBlocked.HasValue)
            {
                query = query.Where(l => l.IsBlocked == isBlocked.Value);
            }

            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                query = query.Where(l => l.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public int GetLogsCount(bool? isBlocked = null, string? countryCode = null)
        {
            var query = _logs.AsQueryable();

            if (isBlocked.HasValue)
            {
                query = query.Where(l => l.IsBlocked == isBlocked.Value);
            }

            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                query = query.Where(l => l.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
            }

            return query.Count();
        }
    }
}
