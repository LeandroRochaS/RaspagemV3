using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspagemV3.Migrations
{
    public partial class teste1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOGROBO",
                columns: table => new
                {
                    iDlOG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoRobo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioRobo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateLog = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Etapa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InformacaoLog = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdProdutoAPI = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOGROBO", x => x.iDlOG);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOGROBO");
        }
    }
}
