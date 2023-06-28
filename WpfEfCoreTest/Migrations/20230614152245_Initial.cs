using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfEfCoreTest.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Komplect",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameKompl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komplect", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nameOborud",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameOborud = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nameOborud", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "otchetFormular",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idf111 = table.Column<int>(type: "int", nullable: false),
                    idKomplect = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    numForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    count = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    serial = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DataTO = table.Column<DateTime>(type: "date", nullable: true),
                    DateIn = table.Column<DateTime>(type: "date", nullable: false),
                    DateOut = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "otchetOborud",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameOborud = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvNum = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    zavodNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otchetOborud", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Podr",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamePodr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podr", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sklad",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idformular = table.Column<int>(type: "int", nullable: false),
                    NumForm = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InvNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateSpisania = table.Column<DateTime>(type: "date", nullable: true),
                    DateToSklad = table.Column<DateTime>(type: "date", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sklad", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SprDgm",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idNameOborudKomplekt = table.Column<int>(type: "int", nullable: true),
                    NameOborud = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    gold = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    silver = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    mpg = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprDgm", x => x.id);
                    table.ForeignKey(
                        name: "FK_SprDgm_Komplect",
                        column: x => x.idNameOborudKomplekt,
                        principalTable: "Komplect",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idPodr = table.Column<int>(type: "int", nullable: false),
                    Lname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_Podr",
                        column: x => x.idPodr,
                        principalTable: "Podr",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "f111",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<int>(type: "int", nullable: false),
                    idnameOborud = table.Column<int>(type: "int", nullable: false),
                    Podr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KartNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvNum = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    zavodNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GtDate = table.Column<DateTime>(type: "date", nullable: false),
                    OutDate = table.Column<DateTime>(type: "date", nullable: true),
                    Remont = table.Column<bool>(type: "bit", nullable: false),
                    Spisan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_f111", x => x.id);
                    table.ForeignKey(
                        name: "FK_f111_nameOborud",
                        column: x => x.idnameOborud,
                        principalTable: "nameOborud",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_f111_users",
                        column: x => x.idUser,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "info",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idUser = table.Column<int>(type: "int", nullable: false),
                    NameComp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    pass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mac = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    doljnost = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    vtel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_info_users",
                        column: x => x.idUser,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    pass = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    idRole = table.Column<int>(type: "int", nullable: false),
                    idUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSys", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserSys_Role",
                        column: x => x.idRole,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSys_users",
                        column: x => x.idUser,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "formular",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idf111 = table.Column<int>(type: "int", nullable: false),
                    idKomplect = table.Column<int>(type: "int", nullable: false),
                    NameKomplect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    count = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    serial = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DataTO = table.Column<DateTime>(type: "date", nullable: false),
                    DateIn = table.Column<DateTime>(type: "date", nullable: false),
                    DateOut = table.Column<DateTime>(type: "date", nullable: true),
                    numAkt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    yearProd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    garantyTo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formular", x => x.id);
                    table.ForeignKey(
                        name: "FK_formular_f111",
                        column: x => x.idf111,
                        principalTable: "f111",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_formular_Komplect",
                        column: x => x.idKomplect,
                        principalTable: "Komplect",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtchetRemont",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idf111 = table.Column<int>(type: "int", nullable: false),
                    podr = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    user = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    numForm = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    nameOborud = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BeginDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(210)", maxLength: 210, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtchetRemont", x => x.id);
                    table.ForeignKey(
                        name: "FK_OtchetRemont_f111",
                        column: x => x.idf111,
                        principalTable: "f111",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dgm",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idformular = table.Column<int>(type: "int", nullable: false),
                    numForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gold = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    silver = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    mpg = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dgm", x => x.id);
                    table.ForeignKey(
                        name: "FK_dgm_formular",
                        column: x => x.idformular,
                        principalTable: "formular",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Spisanie",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idformular = table.Column<int>(type: "int", nullable: false),
                    numForm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    zavodNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumAktTechSost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateSpisan = table.Column<DateTime>(type: "date", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spisanie", x => x.id);
                    table.ForeignKey(
                        name: "FK_Spisanie_formular",
                        column: x => x.idformular,
                        principalTable: "formular",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dgm_idformular",
                table: "dgm",
                column: "idformular");

            migrationBuilder.CreateIndex(
                name: "IX_f111_idnameOborud",
                table: "f111",
                column: "idnameOborud");

            migrationBuilder.CreateIndex(
                name: "IX_f111_idUser",
                table: "f111",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "IX_formular_idf111",
                table: "formular",
                column: "idf111");

            migrationBuilder.CreateIndex(
                name: "IX_formular_idKomplect",
                table: "formular",
                column: "idKomplect");

            migrationBuilder.CreateIndex(
                name: "IX_info_idUser",
                table: "info",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "IX_OtchetRemont_idf111",
                table: "OtchetRemont",
                column: "idf111");

            migrationBuilder.CreateIndex(
                name: "IX_Spisanie_idformular",
                table: "Spisanie",
                column: "idformular");

            migrationBuilder.CreateIndex(
                name: "IX_SprDgm_idNameOborudKomplekt",
                table: "SprDgm",
                column: "idNameOborudKomplekt");

            migrationBuilder.CreateIndex(
                name: "IX_users_idPodr",
                table: "users",
                column: "idPodr");

            migrationBuilder.CreateIndex(
                name: "IX_UserSys_idRole",
                table: "UserSys",
                column: "idRole");

            migrationBuilder.CreateIndex(
                name: "IX_UserSys_idUser",
                table: "UserSys",
                column: "idUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dgm");

            migrationBuilder.DropTable(
                name: "info");

            migrationBuilder.DropTable(
                name: "otchetFormular");

            migrationBuilder.DropTable(
                name: "otchetOborud");

            migrationBuilder.DropTable(
                name: "OtchetRemont");

            migrationBuilder.DropTable(
                name: "sklad");

            migrationBuilder.DropTable(
                name: "Spisanie");

            migrationBuilder.DropTable(
                name: "SprDgm");

            migrationBuilder.DropTable(
                name: "UserSys");

            migrationBuilder.DropTable(
                name: "formular");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "f111");

            migrationBuilder.DropTable(
                name: "Komplect");

            migrationBuilder.DropTable(
                name: "nameOborud");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "Podr");
        }
    }
}
