using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbor.Server.Finance.Data.Migrations
{
    public partial class AddMonthlyBalancetable : Migration
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
                    MonthNo = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IncomeAmount = table.Column<double>(type: "float", nullable: false),
                    ExpenseAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyBalance", x => new { x.Year, x.MonthNo });
                });

            migrationBuilder.InsertData(
                schema: "Finance",
                table: "MonthlyBalance",
                columns: new[] { "MonthNo", "Year", "ExpenseAmount", "IncomeAmount" },
                values: new object[,]
                {
                    { 1, 2020, -188284.89999999999, 1872340.0 },
                    { 2, 2020, -235860.82000000001, 251070.0 },
                    { 3, 2020, -266707.34000000003, 136430.0 },
                    { 4, 2020, -217859.64999999999, 95760.0 },
                    { 5, 2020, -211140.29999999999, 42390.0 },
                    { 6, 2020, -283869.91999999998, 113990.0 },
                    { 7, 2020, -269949.78000000003, 367770.0 },
                    { 8, 2020, -223690.06, 123620.0 },
                    { 9, 2020, -267824.75, 125480.0 },
                    { 10, 2020, 0.0, 0.0 },
                    { 11, 2020, 0.0, 0.0 },
                    { 12, 2020, 0.0, 0.0 }
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
