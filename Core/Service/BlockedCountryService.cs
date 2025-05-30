using AutoMapper;
using Core.Interfaces;
using Core.Models;
using DomainLayer.Contracts;
using Shared.DataTransferObject;

namespace Service
{
    public class BlockedCountryService : IBlockedCountryService
    {
        private readonly IBlockedCountryRepository _repository;
        private readonly IMapper _mapper;

        public BlockedCountryService(IBlockedCountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public bool AddBlockedCountry(string countryCode, int? blockedUntil)
        {
            DateTime? expiresAt = blockedUntil.HasValue ? DateTime.UtcNow.AddMinutes(blockedUntil.Value) : null;
            string countryName = GetCountryNameFromCode(countryCode);
            return _repository.Add(countryCode, countryName, "", expiresAt);
        }

        public bool RemoveBlockedCountry(string countryCode)
        {
            return _repository.Remove(countryCode);
        }
        public IEnumerable<BlockCountryDto> GetBlockedCountries()
        {
            return _mapper.Map<IEnumerable<BlockCountryDto>>(_repository.GetAll());
        }

        public bool IsBlocked(string countryCode)
        {
            return _repository.IsBlocked(countryCode);
        }

        // Async methods for controllers
        public Task AddBlockedCountryAsync(BlockCountryDto request)
        {
            AddBlockedCountry(request.CountryCode, null);
            return Task.CompletedTask;
        }

        public Task RemoveBlockedCountryAsync(string countryCode)
        {
            RemoveBlockedCountry(countryCode);
            return Task.CompletedTask;
        }

        public Task<PaginatedResult<BlockedCountryResponse>> GetBlockedCountriesAsync(int page, int pageSize, string? search = null)
        {
            var allCountries = GetBlockedCountries();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                allCountries = allCountries.Where(c =>
                    c.CountryCode.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.CountryName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = allCountries.Count(); var pagedItems = _mapper.Map<List<BlockedCountryResponse>>(allCountries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList());

            var result = new PaginatedResult<BlockedCountryResponse>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Task.FromResult(result);
        }

        public Task AddTemporalBlockAsync(TemporalBlockRequest request)
        {
            AddBlockedCountry(request.CountryCode, request.DurationMinutes);
            return Task.CompletedTask;
        }

        // Helper method to get country name from code
        private string GetCountryNameFromCode(string countryCode)
        {
            // This is a simple example - in a real application, you would use a proper country database
            var countryCodes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"US", "United States"},
                {"CA", "Canada"},
                {"MX", "Mexico"},
                {"GB", "United Kingdom"},
                {"FR", "France"},
                {"DE", "Germany"},
                {"IT", "Italy"},
                {"ES", "Spain"},
                {"CN", "China"},
                {"JP", "Japan"},
                {"IN", "India"},
                {"BR", "Brazil"},
                {"RU", "Russia"},
                {"AU", "Australia"},
                {"ZA", "South Africa"}
                // Add more countries as needed
            };

            return countryCodes.TryGetValue(countryCode, out var name) ? name : countryCode;
        }
    }
}
