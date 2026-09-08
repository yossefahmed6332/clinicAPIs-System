using clinicAPIsSystem.DTOs.FinanialReportDTOs;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialReportController : ControllerBase
    {
        private readonly IFinancialReportService _financialReportService;

        public FinancialReportController(
            IFinancialReportService financialReportService)
        {
            _financialReportService = financialReportService;
        }

        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpPost]
        public async Task<IActionResult> CreateFinancialReport(
            [FromBody] CreateFinancialReportDto createFinancialReportDto)
        {
            var createdFinancialReport =
                await _financialReportService.CreateFinancialReportAsync(
                    createFinancialReportDto);

            return CreatedAtAction(
                nameof(GetFinancialReport),
                new { id = createdFinancialReport.Id },
                createdFinancialReport);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet]
        public async Task<IActionResult> GetAllFinancialReports()
        {
            var financialReports =
                await _financialReportService.GetAllFinancialReportsAsync();

            return Ok(financialReports);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinancialReport(int id)
        {
            var financialReport =
                await _financialReportService.GetFinancialReportAsync(id);

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
            var financialReports =
                await _financialReportService.GetFinancialReportsByRangeAsync(
                    min,
                    max,
                    startDate,
                    endDate);

            return Ok(financialReports);
        }
        [Authorize($"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}, {nameof(UserRole.Accountant)}")]
        [HttpGet("date-range")]
        public async Task<IActionResult> GetFinancialReportsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var financialReports =
                await _financialReportService.GetFinancialReportsByDateRangeAsync(
                    startDate,
                    endDate);

            return Ok(financialReports);
        }
    }
}