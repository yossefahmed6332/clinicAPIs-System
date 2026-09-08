namespace clinicAPIsSystem.Models
{
    public class FinancialReport
    {
        public int Id { get; private set; }

        public decimal MonthlyExpenses { get; private set; }

        public decimal NetProfit { get; private set; }

        public decimal MonthlyRevenue { get; private set; }

        public DateTime Date { get; private set; }


        // Constructor
        public FinancialReport(
            decimal monthlyExpenses,
            decimal netProfit,
            decimal monthlyRevenue,
            DateTime date)
        {
            MonthlyExpenses = monthlyExpenses;
            NetProfit = netProfit;
            MonthlyRevenue = monthlyRevenue;
            Date = date;
        }


        // Required by EF Core
        public FinancialReport()
        {
        }


        // Update
        public void Update(
            decimal monthlyExpenses,
            decimal netProfit,
            decimal monthlyRevenue,
            DateTime date)
        {
            MonthlyExpenses = monthlyExpenses;
            NetProfit = netProfit;
            MonthlyRevenue = monthlyRevenue;
            Date = date;
        }
    }
}