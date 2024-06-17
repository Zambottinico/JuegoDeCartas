using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Juego_Sin_Nombre.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "games_id_seq");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "usuarios",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "unlockable_character",
                table: "decision",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "unlockable_character",
                table: "decision");

            migrationBuilder.DropSequence(
                name: "games_id_seq");
        }
    }
}
