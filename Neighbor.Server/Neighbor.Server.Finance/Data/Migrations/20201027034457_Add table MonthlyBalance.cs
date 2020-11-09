using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbor.Server.Finance.MonthlyBalance.Data.Migrations
{
    public partial class AddtableMonthlyBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.CreateTable(
                name: "MonthlyBalance",
                schema: "Finance",
                columns: table => new
                {
                    MonthNo = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    IncomeAmount = table.Column<double>(nullable: false),
                    ExpenseAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyBalance", x => new { x.Year, x.MonthNo });
                });

            migrationBuilder.InsertData(
                schema: "Finance",
                table: "MonthlyBalance",
                columns: new[] { "Year", "MonthNo", "ExpenseAmount", "IncomeAmount" },
                values: new object[,]
                {
                    { 2020, 1, -188284.89999999999, 1872340.0 },
                    { 2020, 2, -235860.82000000001, 251070.0 },
                    { 2020, 3, -266707.34000000003, 136430.0 },
                    { 2020, 4, -217859.64999999999, 95760.0 },
                    { 2020, 5, -211140.29999999999, 42390.0 },
                    { 2020, 6, -283869.91999999998, 113990.0 },
                    { 2020, 7, -269949.78000000003, 367770.0 },
                    { 2020, 8, -223690.06, 123620.0 },
                    { 2020, 9, -267824.75, 125480.0 },
                    { 2020, 10, 0.0, 0.0 },
                    { 2020, 11, 0.0, 0.0 },
                    { 2020, 12, 0.0, 0.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyBalance",
                schema: "Finance");
        }
    }
}
