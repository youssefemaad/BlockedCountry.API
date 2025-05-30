using DomainLayer.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObject;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedAttemptLogService _logService;

        public LogsController(IBlockedAttemptLogService logService)
        {
            _logService = logService;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? isBlocked = null,
            [FromQuery] string? countryCode = null)
        {
            if (page < 1)
                page = 1;

            if (pageSize < 1 || pageSize > 100)
                pageSize = 10;

            var allLogs = _logService.GetLogs();

            // Apply filters
            var filteredLogs = allLogs.AsQueryable();

            if (isBlocked.HasValue)
            {
                filteredLogs = filteredLogs.Where(l => l.BlockedStatus == isBlocked.Value);
            }

            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                filteredLogs = filteredLogs.Where(l => !string.IsNullOrEmpty(l.CountryCode) && l.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = filteredLogs.Count();

            var pagedLogs = filteredLogs
                .OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                data = pagedLogs,
                pagination = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalItems = totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    filters = new
                    {
                        isBlocked,
                        countryCode
                    }
                }
            });
        }
    }
}
