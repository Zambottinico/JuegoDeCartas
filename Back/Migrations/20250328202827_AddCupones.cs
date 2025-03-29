using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Juego_Sin_Nombre.Migrations
{
    /// <inheritdoc />
    public partial class AddCupones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cupones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroDiamantes = table.Column<int>(type: "int", nullable: false),
                    NumeroOro = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CuponUsuario",
                columns: table => new
                {
                    CuponsId = table.Column<int>(type: "int", nullable: false),
                    PlayersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponUsuario", x => new { x.CuponsId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_CuponUsuario_Cupones_CuponsId",
                        column: x => x.CuponsId,
                        principalTable: "Cupones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuponUsuario_usuarios_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuponUsuario_PlayersId",
                table: "CuponUsuario",
                column: "PlayersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuponUsuario");

            migrationBuilder.DropTable(
                name: "Cupones");
        }
    }
}
