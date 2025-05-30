using DomainLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObject;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockedCountryService _countryService;

        public CountriesController(IBlockedCountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost("block")]
        public IActionResult AddBlockedCountry([FromBody] BlockCountryDto request, [FromQuery] int? durationMinutes = null)
        {
            if (string.IsNullOrWhiteSpace(request?.CountryCode))
            {
                return BadRequest("Country code is required");
            }

            // Normalize country code to uppercase
            string normalizedCode = request.CountryCode.ToUpper();

            if (_countryService.IsBlocked(normalizedCode))
                return Conflict(new { message = "Country already blocked", countryCode = normalizedCode });

            bool success = _countryService.AddBlockedCountry(normalizedCode, durationMinutes);
            if (!success)
                return StatusCode(500, new { message = "Failed to add country to blocked list" });

            string message = $"Country {normalizedCode} successfully blocked";
            if (durationMinutes.HasValue && durationMinutes.Value > 0)
            {
                var expiresAt = DateTime.UtcNow.AddMinutes(durationMinutes.Value);
                message += $" until {expiresAt:yyyy-MM-dd HH:mm:ss} UTC";
            }

            return Ok(new { message });
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult RemoveBlockedCountry(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return BadRequest("Country code is required");
            }

            // Normalize country code to uppercase
            string normalizedCode = countryCode.ToUpper();

            if (!_countryService.IsBlocked(normalizedCode))
                return NotFound(new { message = $"Country {normalizedCode} is not blocked" });

            bool success = _countryService.RemoveBlockedCountry(normalizedCode);
            if (!success)
                return StatusCode(500, new { message = "Failed to remove country from blocked list" });

            return NoContent();
        }
        
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
        {
            if (page < 1)
                page = 1;

            if (pageSize < 1 || pageSize > 100)
                pageSize = 20;

            var allCountries = _countryService.GetBlockedCountries();
            
            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                allCountries = allCountries.Where(c => 
                    c.CountryCode.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                    c.CountryName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            
            var totalCount = allCountries.Count(); 
            
            var pagedCountries = allCountries
                .OrderBy(c => c.CountryCode)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                data = pagedCountries,
                pagination = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalItems = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    search = search 
                }
            });
        }

        [HttpPost("temporal-block")]
        public IActionResult AddTemporalBlock([FromBody] TemporalBlockRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.CountryCode))
            {
                return BadRequest("Country code is required");
            }

            if (request.DurationMinutes <= 0)
            {
                return BadRequest("Duration must be greater than 0 minutes");
            }

            // Normalize country code to uppercase
            string normalizedCode = request.CountryCode.ToUpper();

            bool success = _countryService.AddBlockedCountry(normalizedCode, request.DurationMinutes);
            if (!success)
                return StatusCode(500, new { message = "Failed to add country to blocked list" });

            var expiresAt = DateTime.UtcNow.AddMinutes(request.DurationMinutes);
            return Ok(new
            {
                message = $"Country {normalizedCode} successfully blocked until {expiresAt:yyyy-MM-dd HH:mm:ss} UTC",
                countryCode = normalizedCode,
                expiresAt = expiresAt
            });
        }
    }
}