namespace Neighbor.Domain.Models.Finance
{
    public class MonthlyBalance
    {
        public int MonthNo { get; set; }
        public string MonthName { get; set; }
        public double IncomeAmount { get; set; }
        public double ExpenseAmount { get; set; }
    }
}
