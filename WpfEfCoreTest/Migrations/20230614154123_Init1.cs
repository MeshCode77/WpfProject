using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfEfCoreTest.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvNum",
                table: "OtchetRemont",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZavodNum",
                table: "OtchetRemont",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvNum",
                table: "OtchetRemont");

            migrationBuilder.DropColumn(
                name: "ZavodNum",
                table: "OtchetRemont");
        }
    }
}
