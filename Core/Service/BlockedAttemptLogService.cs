using System.Collections.Concurrent;
using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Shared.DataTransferObject;

namespace Service
{
    public class BlockedAttemptLogService : IBlockedAttemptLogService
    {
        private readonly ConcurrentBag<BlockedAttemptLog> _logs = new();
        private readonly IMapper _mapper;

        public BlockedAttemptLogService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void LogAttempt(BlockedAttemptDto log)
        {
            var logEntry = _mapper.Map<BlockedAttemptLog>(log);
            _logs.Add(logEntry);
        }
        public IEnumerable<BlockedAttemptDto> GetLogs()
        {
            return _mapper.Map<IEnumerable<BlockedAttemptDto>>(_logs);
        }
    }
}
