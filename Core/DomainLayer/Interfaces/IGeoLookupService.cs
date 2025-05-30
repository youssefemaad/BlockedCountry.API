using Core.Models;

namespace Core.Interfaces
{
    public interface IGeoLookupService
    {
        Task<GeoLookupResult> LookupAsync(string ipAddress);
    }
}
