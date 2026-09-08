
namespace clinicAPIsSystem.DTOs.FinanialReportDTOs
{
    public class FinancialReportDto
    {
        public int Id { get;  set; }
        public decimal MonthlyExpenses { get;  set; }
        public decimal MonthlyRevenue { get;  set; }
        public decimal NetProfit { get;  set; }
        public DateTime Date { get;  set; } = DateTime.Now;



    }
}
