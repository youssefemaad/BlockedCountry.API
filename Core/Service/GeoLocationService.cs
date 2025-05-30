using DomainLayer.Contracts;
using Microsoft.Extensions.Configuration;
using Core.Interfaces;

namespace Service;

public class GeoLocationService : IGeoLocationService
{
    private readonly IGeoLookupService _geoLookupService;

    public GeoLocationService(IGeoLookupService geoLookupService)
    {
        _geoLookupService = geoLookupService;
    }

    public string GetCountryCodeByIp(string ipAddress)
    {
        try
        {
            // Since this interface requires synchronous operation, we'll use GetAwaiter().GetResult()
            // In a real-world scenario, consider making this interface async
            var result = _geoLookupService.LookupAsync(ipAddress).GetAwaiter().GetResult();
            return result.CountryCode;
        }
        catch (Exception)
        {
            // Return a default value if the lookup fails
            return "XX";
        }
    }
}
