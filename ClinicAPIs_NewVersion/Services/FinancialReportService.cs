using AutoMapper;
using clinicAPIsSystem.IService;
using clinicAPIsSystem.DTOs.FinanialReportDTOs;
using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.Service
{
    public class FinancialReportService:IFinancialReportService
    {
        private readonly IFinancialReportRepository _financialReportRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FinancialReportService> _logger;
        public FinancialReportService(IFinancialReportRepository financialReportRepository, IMapper mapper, ILogger<FinancialReportService> logger)
        {
            _financialReportRepository = financialReportRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FinancialReportDto> CreateFinancialReportAsync(CreateFinancialReportDto createFinancialReportDto)
        {
            _logger.LogInformation(
                "Creating financial report for Date {Date}",
                createFinancialReportDto.Date);
            var financialReport = new FinancialReport
                (
                createFinancialReportDto.MonthlyExpenses,
                createFinancialReportDto.NetProfit,
                createFinancialReportDto.MonthlyRevenue,
                createFinancialReportDto.Date
                );
            financialReport = await _financialReportRepository.CreateFinancialReportAsync(financialReport);
            _logger
                .LogInformation("Creating financial report  with ID {FinancialReportId} ",
                financialReport.Id);
            return _mapper.Map<FinancialReportDto>(financialReport);
        }

        public async Task<List<FinancialReportDto>> GetAllFinancialReportsAsync()
        {
            _logger.
                LogDebug("Retrieving all financial reports");
            var financialReports = await _financialReportRepository.GetAllFinancialReportsAsync();
            return _mapper.Map<List<FinancialReportDto>>(financialReports);
        }

        public async Task<FinancialReportDto> GetFinancialReportAsync(int id)
        {
            _logger
                .LogDebug("Retrieving financial report with ID {FinancialReportId}", id);
            var financialReport = await _financialReportRepository.GetFinancialReportAsync(id);
            if (financialReport == null)
            {
                _logger
                    .LogWarning("Financial report with ID {FinancialReportId} not found", id);
                throw new KeyNotFoundException($"Financial report with ID {id} not found.");
            }
            return _mapper.Map<FinancialReportDto>(financialReport);
        }

        public async Task<List<FinancialReportDto>> GetFinancialReportsByRangeAsync(decimal min, decimal max, DateTime startDate, DateTime endDate)
        { 
            _logger
                .LogDebug("Retrieving financial reports by range: {Min} to {Max}, from {StartDate} to {EndDate}", min, max, startDate, endDate);
            var financialReports = await _financialReportRepository.GetFinancialReportsByRangeAsync(min, max, startDate, endDate);
            return _mapper.Map<List<FinancialReportDto>>(financialReports);
        }
        public async Task<List<FinancialReportDto>> GetFinancialReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            _logger
                .LogDebug("Retrieving financial reports by date range: {StartDate} to {EndDate}", startDate, endDate);
            var financialReports = await _financialReportRepository.GetFinancialReportsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<FinancialReportDto>>(financialReports);
        }
    }
}
