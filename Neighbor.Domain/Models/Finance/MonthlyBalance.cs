namespace Neighbor.Core.Domain.Models.Finance
{
    public class MonthlyBalance
    {
        public int MonthNo { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public double IncomeAmount { get; set; }
        public double ExpenseAmount { get; set; }
        public double TotalIncomeAmount { get; set; }
        public double AverageIncomeAmount { get; set; }
    }
}
