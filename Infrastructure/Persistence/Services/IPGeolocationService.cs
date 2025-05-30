using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IPGeolocationService : IGeoLookupService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public IPGeolocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["IPGeolocationSettings:ApiKey"] ?? "04222ea265d7402990c23f2b4b4314f4";
            _baseUrl = configuration["IPGeolocationSettings:BaseUrl"] ?? "https://api.ipgeolocation.io/ipgeo";
        }

        public async Task<GeoLookupResult> LookupAsync(string ipAddress)
        {
            try
            {
                var url = $"{_baseUrl}?apiKey={_apiKey}&ip={ipAddress}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new GeoLookupResult
                    {
                        CountryCode = "XX",
                        CountryName = "Error",
                        ISP = $"API Error: Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})."
                    };
                }

                var content = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(content);
                JsonElement root = doc.RootElement;

                string countryCode = "XX";
                string countryName = "Unknown";
                string isp = "Unknown";

                if (root.TryGetProperty("country_code2", out JsonElement countryCodeElement))
                {
                    countryCode = countryCodeElement.GetString() ?? "XX";
                }

                if (root.TryGetProperty("country_name", out JsonElement countryNameElement))
                {
                    countryName = countryNameElement.GetString() ?? "Unknown";
                }

                if (root.TryGetProperty("isp", out JsonElement ispElement))
                {
                    isp = ispElement.GetString() ?? "Unknown";
                }

                return new GeoLookupResult
                {
                    CountryCode = countryCode,
                    CountryName = countryName,
                    ISP = isp
                };
            }
            catch (HttpRequestException ex)
            {
                return new GeoLookupResult
                {
                    CountryCode = "XX",
                    CountryName = "Error",
                    ISP = $"API Error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new GeoLookupResult
                {
                    CountryCode = "XX",
                    CountryName = "Error",
                    ISP = $"Error: {ex.Message}"
                };
            }
        }
    }
}
