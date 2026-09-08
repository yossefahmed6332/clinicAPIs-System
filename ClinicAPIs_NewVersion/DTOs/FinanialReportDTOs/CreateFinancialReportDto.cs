using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.FinanialReportDTOs
{
    public class CreateFinancialReportDto
    {
        [Required, Range(0, double.MaxValue)]
        public decimal MonthlyExpenses { get;  set; }
        [Required, Range(0, double.MaxValue)]
        public decimal MonthlyRevenue { get; set; }
        [Range(0, double.MaxValue)]
        public decimal NetProfit { get; set; } 
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;




    }
}
