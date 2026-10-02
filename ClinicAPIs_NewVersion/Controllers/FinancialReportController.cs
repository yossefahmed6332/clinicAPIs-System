using clinicAPIsSystem.DTOs.FinanialReportDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialReportController : ControllerBase
    {
        private readonly IFinancialReportService _financialReportService;
        private readonly IMemoryCache _cache;

        public FinancialReportController(
            IFinancialReportService financialReportService,
            IMemoryCache cache)
        {
            _financialReportService = financialReportService;
            _cache = cache;
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpPost]
        public async Task<IActionResult> CreateFinancialReport(
            [FromBody] CreateFinancialReportDto createFinancialReportDto)
        {
            var createdFinancialReport =
                await _financialReportService.CreateFinancialReportAsync(
                    createFinancialReportDto);

            _cache.Remove("allFinancialReports");

            return CreatedAtAction(
                nameof(GetFinancialReport),
                new { id = createdFinancialReport.Id },
                createdFinancialReport);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllFinancialReports()
        {
            const string cacheKey = "allFinancialReports";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<FinancialReportDto>? financialReports))
            {
                financialReports =
                    await _financialReportService
                        .GetAllFinancialReportsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, financialReports, cacheOptions);
            }

            return Ok(financialReports);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinancialReport(int id)
        {
            string cacheKey = $"financialReport:{id}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out FinancialReportDto? financialReport))
            {
                financialReport =
                    await _financialReportService
                        .GetFinancialReportAsync(id);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(6));

                _cache.Set(cacheKey, financialReport, cacheOptions);
            }

            return Ok(financialReport);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet("range")]
        public async Task<IActionResult> GetFinancialReportsByRange(
            [FromQuery] decimal min,
            [FromQuery] decimal max,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            string cacheKey =
                $"financialReports:range:{min}:{max}:{startDate:O}:{endDate:O}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<FinancialReportDto>? financialReports))
            {
                financialReports =
                    await _financialReportService
                        .GetFinancialReportsByRangeAsync(
                            min,
                            max,
                            startDate,
                            endDate);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, financialReports, cacheOptions);
            }

            return Ok(financialReports);
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet("date-range")]
        public async Task<IActionResult> GetFinancialReportsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            string cacheKey =
                $"financialReports:date-range:{startDate:O}:{endDate:O}";

            if (!_cache.TryGetValue(
                    cacheKey,
                    out List<FinancialReportDto>? financialReports))
            {
                financialReports =
                    await _financialReportService
                        .GetFinancialReportsByDateRangeAsync(
                            startDate,
                            endDate);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, financialReports, cacheOptions);
            }

            return Ok(financialReports);
        }
    }
}