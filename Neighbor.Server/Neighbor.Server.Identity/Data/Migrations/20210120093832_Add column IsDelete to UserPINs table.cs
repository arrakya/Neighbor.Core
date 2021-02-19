using Microsoft.EntityFrameworkCore.Migrations;

namespace Neighbor.Server.Identity.Data.Migrations
{
    public partial class AddcolumnIsDeletetoUserPINstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "identity",
                table: "UserPINs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "identity",
                table: "UserPINs");
        }
    }
}
