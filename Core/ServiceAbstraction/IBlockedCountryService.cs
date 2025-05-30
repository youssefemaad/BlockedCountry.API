using Shared.DataTransferObject;

namespace DomainLayer.Contracts;

public interface IBlockedCountryService
{
    bool AddBlockedCountry(string countryCode, int? blockedUntil);
    bool RemoveBlockedCountry(string countryCode);
    IEnumerable<BlockCountryDto> GetBlockedCountries();
    bool IsBlocked(string countryCode);
    
    Task AddBlockedCountryAsync(BlockCountryDto request);
    Task RemoveBlockedCountryAsync(string countryCode);
    Task<PaginatedResult<BlockedCountryResponse>> GetBlockedCountriesAsync(int page, int pageSize, string? search = null);
    Task AddTemporalBlockAsync(TemporalBlockRequest request);
}
