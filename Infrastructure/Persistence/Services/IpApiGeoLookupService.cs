using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class IpApiGeoLookupService : IGeoLookupService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string _jsonEndpoint;

        public IpApiGeoLookupService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration["IpApiSettings:BaseUrl"] ?? "https://ipapi.co/";
            _jsonEndpoint = _configuration["IpApiSettings:JsonEndpoint"] ?? "{0}/json/";
        }
        public async Task<GeoLookupResult> LookupAsync(string ipAddress)
        {
            try
            {
                var endpoint = string.Format(_jsonEndpoint, ipAddress).Replace("{0}", ipAddress);
                var response = await _httpClient.GetFromJsonAsync<GeoLookupResult>(_baseUrl + endpoint);
                return response ?? new GeoLookupResult { CountryCode = "XX", CountryName = "Unknown", ISP = "Unknown" };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return new GeoLookupResult { CountryCode = "XX", CountryName = "Error", ISP = $"API Error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return new GeoLookupResult { CountryCode = "XX", CountryName = "Error", ISP = "Unknown" };
            }
        }
    }
}
