using DomainLayer.Contracts;
using Core.Interfaces;

namespace Service
{
    public class TemporalBlockCleanupService
    {
        private readonly IBlockedCountryRepository _repository;

        public TemporalBlockCleanupService(IBlockedCountryRepository repository)
        {
            _repository = repository;
        }

        public void CleanupExpiredBlocks()
        {
            _repository.CleanupExpired();
        }
    }
}