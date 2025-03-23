using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Juego_Sin_Nombre.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceModeladdUser4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Invoices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UsuarioId",
                table: "Invoices",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_usuarios_UsuarioId",
                table: "Invoices",
                column: "UsuarioId",
                principalTable: "usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_usuarios_UsuarioId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_UsuarioId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Invoices");
        }
    }
}
