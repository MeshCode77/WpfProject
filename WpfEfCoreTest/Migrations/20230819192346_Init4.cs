using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfEfCoreTest.Migrations
{
    public partial class Init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSys_Role",
                table: "UserSys");

            migrationBuilder.DropIndex(
                name: "IX_UserSys_idRole",
                table: "UserSys");

            migrationBuilder.DropColumn(
                name: "idRole",
                table: "UserSys");

            //migrationBuilder.AddColumn<int>(
            //    name: "RoleId",
            //    table: "UserSys",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "OtchetRemont",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BeginDate",
                table: "OtchetRemont",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserSys_RoleId",
            //    table: "UserSys",
            //    column: "RoleId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_UserSys_Role_RoleId",
            //    table: "UserSys",
            //    column: "RoleId",
            //    principalTable: "Role",
            //    principalColumn: "id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_UserSys_Role_RoleId",
            //    table: "UserSys");

            //migrationBuilder.DropIndex(
            //    name: "IX_UserSys_RoleId",
            //    table: "UserSys");

            //migrationBuilder.DropColumn(
            //    name: "RoleId",
            //    table: "UserSys");

            //migrationBuilder.AddColumn<int>(
            //    name: "idRole",
            //    table: "UserSys",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "OtchetRemont",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BeginDate",
                table: "OtchetRemont",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSys_idRole",
                table: "UserSys",
                column: "idRole");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSys_Role",
                table: "UserSys",
                column: "idRole",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
