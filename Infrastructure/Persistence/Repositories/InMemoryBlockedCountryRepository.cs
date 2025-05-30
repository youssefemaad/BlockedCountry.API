using Core.Interfaces;
using Core.Models;
using System.Collections.Concurrent;

namespace Infrastructure.Repositories
{
    public class InMemoryBlockedCountryRepository : IBlockedCountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();

        public bool Add(string countryCode, string countryName, string reason = "", DateTime? expiresAt = null)
        {
            var country = new BlockedCountry
            {
                CountryCode = countryCode,
                CountryName = countryName,
                ExpiresAt = expiresAt
            };

            return _blockedCountries.TryAdd(countryCode, country);
        }

        public bool Remove(string countryCode) => _blockedCountries.TryRemove(countryCode, out _);

        public bool IsBlocked(string countryCode)
        {
            if (!_blockedCountries.TryGetValue(countryCode, out var blockedCountry))
            {
                return false;
            }

            if (blockedCountry.IsExpired)
            {
                // Automatically remove expired entries
                _blockedCountries.TryRemove(countryCode, out _);
                return false;
            }

            return true;
        }

        public IEnumerable<BlockedCountry> GetAll()
        {
            // Filter out expired entries while returning
            return _blockedCountries.Values
                .Where(country => !country.IsExpired)
                .ToList();
        }

        public void CleanupExpired()
        {
            var expiredKeys = _blockedCountries
                .Where(kvp => kvp.Value.IsExpired)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _blockedCountries.TryRemove(key, out _);
            }
        }
    }
}
