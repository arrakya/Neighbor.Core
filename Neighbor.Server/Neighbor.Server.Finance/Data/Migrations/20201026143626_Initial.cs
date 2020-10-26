using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbor.Server.Finance.MonthlyBalance.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyBalances",
                columns: table => new
                {
                    MonthNo = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    MonthName = table.Column<string>(maxLength: 10, nullable: false),
                    IncomeAmount = table.Column<double>(nullable: false),
                    ExpenseAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyBalances", x => new { x.Year, x.MonthNo });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyBalances");
        }
    }
}
