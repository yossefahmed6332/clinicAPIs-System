using clinicAPIsSystem.IRepositoryService;
using clinicAPIsSystem.Data;
using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.RepositoryService
{
    public class FinancialReportRepository : IFinancialReportRepository
    {
        private readonly ClinicDbContext _context;
        public FinancialReportRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<FinancialReport> CreateFinancialReportAsync(FinancialReport financialReport)
        {
            await _context.TFinancialReports.AddAsync(financialReport);
            await _context.SaveChangesAsync();
            return financialReport;
        }
        public async Task<List<FinancialReport>> GetAllFinancialReportsAsync()
        {
            return await _context.TFinancialReports.ToListAsync();
        }

        public async Task<FinancialReport?> GetFinancialReportAsync(int id)
        {
            return await _context.TFinancialReports.FindAsync(id);
        }

        public async Task<List<FinancialReport>> GetFinancialReportsByRangeAsync(decimal min, decimal max, DateTime startDate, DateTime endDate)
        {
            return await _context.TFinancialReports
                .Where(fr => fr.NetProfit >= min && fr.NetProfit <= max && fr.Date >= startDate && fr.Date <= endDate)
                .ToListAsync();
        }

        public async Task<List<FinancialReport>> GetFinancialReportsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.TFinancialReports
                .Where(fr => fr.Date >= startDate && fr.Date <= endDate)
                .ToListAsync();
        }


    
    }
}
