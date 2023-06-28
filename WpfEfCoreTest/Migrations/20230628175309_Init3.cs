using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfEfCoreTest.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleComplected",
                table: "OtchetRemont",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleComplected",
                table: "OtchetRemont");
        }
    }
}
