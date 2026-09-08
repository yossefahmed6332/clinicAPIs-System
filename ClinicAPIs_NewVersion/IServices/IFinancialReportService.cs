using clinicAPIsSystem.DTOs.FinanialReportDTOs;
using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.NonMedicalStaffDTO.AccountantDTO;

namespace clinicAPIsSystem.IService
{
    public interface IFinancialReportService
    {
        public Task<FinancialReportDto> CreateFinancialReportAsync(CreateFinancialReportDto createFinancialReportDto);
        public Task<List<FinancialReportDto>> GetAllFinancialReportsAsync();
        public Task<FinancialReportDto> GetFinancialReportAsync(int id);
        public Task<List<FinancialReportDto>> GetFinancialReportsByRangeAsync(decimal min, decimal max, DateTime startDate, DateTime endDate);
        public Task<List<FinancialReportDto>> GetFinancialReportsByDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}
