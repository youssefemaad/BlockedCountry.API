using DomainLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObject;

namespace Presentation.Controllers;

[ApiController]
[Route("api/ip")]
public class IpController : ControllerBase
{
    private readonly IGeoLocationService _geoService;
    private readonly IBlockedCountryService _countryService;
    private readonly IBlockedAttemptLogService _logService;

    public IpController(
        IGeoLocationService geoService,
        IBlockedCountryService countryService,
        IBlockedAttemptLogService logService)
    {
        _geoService = geoService;
        _countryService = countryService;
        _logService = logService;
    }

    [HttpGet("ip-lookup")]
    public IActionResult GetCountryByIp([FromQuery] string? ipAddress = null)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "8.8.8.8";
        }

        if (ipAddress == "::1" || ipAddress == "127.0.0.1" || ipAddress.StartsWith("192.168.") || ipAddress.StartsWith("10."))
        {
            ipAddress = "8.8.8.8"; 
        }

        try
        {
            var countryCode = _geoService.GetCountryCodeByIp(ipAddress);

            return Ok(new
            {
                IpAddress = ipAddress,
                CountryCode = countryCode
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error looking up IP: {ex.Message}" });
        }
    }

    [HttpGet("check-block")]
    public IActionResult CheckBlock([FromQuery] string? ipAddress = null)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "8.8.8.8";
        }

        if (ipAddress == "::1" || ipAddress == "127.0.0.1" || ipAddress.StartsWith("192.168.") || ipAddress.StartsWith("10."))
        {
            ipAddress = "8.8.8.8"; 
        }

        try
        {
            var countryCode = _geoService.GetCountryCodeByIp(ipAddress);
            var isBlocked = _countryService.IsBlocked(countryCode);

            _logService.LogAttempt(new BlockedAttemptDto
            {
                IpAddress = ipAddress,
                CountryCode = countryCode,
                BlockedStatus = isBlocked,
                Timestamp = DateTime.UtcNow,
                UserAgent = Request.Headers["User-Agent"].ToString()
            });

            return Ok(new IpCheckResponseDto
            {
                IpAddress = ipAddress,
                CountryCode = countryCode,
                IsBlocked = isBlocked
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Error checking IP block status: {ex.Message}" });
        }
    }
}
