using clinicAPIsSystem.Models;

namespace clinicAPIsSystem.IRepositoryService
{
    public interface IFinancialReportRepository
    {
        public Task<FinancialReport> CreateFinancialReportAsync(FinancialReport financialReport);
        public Task<List<FinancialReport>> GetAllFinancialReportsAsync();
        public Task<FinancialReport?> GetFinancialReportAsync(int id);
        public Task<List<FinancialReport>> GetFinancialReportsByRangeAsync(decimal min, decimal max,DateTime startDate, DateTime endDate);
        public Task<List<FinancialReport>> GetFinancialReportsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
