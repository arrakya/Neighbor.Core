using Xunit;

namespace Neighbor.Application.Test
{
    public class FinanceUnitTest
    {
        [Fact]
        public async void GetMonthlyBalance()
        {
            // Arrange
            var year = 2020;
            var financeRepo = new FinanceRepository();

            // Action
            var monthlyBalanceCollection = await financeRepo.GetMonthlyBalances(year);

            // Assert
            Assert.NotNull(monthlyBalanceCollection);
            Assert.NotEmpty(monthlyBalanceCollection);
            Assert.DoesNotContain(monthlyBalanceCollection, p => p.MonthNo == 0);
            Assert.DoesNotContain(monthlyBalanceCollection, p => string.IsNullOrEmpty(p.MonthName));
        }
    }
}