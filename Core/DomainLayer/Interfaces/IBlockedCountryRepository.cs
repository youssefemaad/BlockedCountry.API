using Core.Models;

namespace Core.Interfaces
{
    public interface IBlockedCountryRepository
    {
        bool Add(string countryCode, string countryName, string reason = "", DateTime? expiresAt = null);
        bool Remove(string countryCode);
        bool IsBlocked(string countryCode);
        IEnumerable<BlockedCountry> GetAll();
        void CleanupExpired();
    }
}
