using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Juego_Sin_Nombre.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "games_id_seq");

            migrationBuilder.CreateTable(
                name: "CardOferts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    CharacterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoldPrice = table.Column<int>(type: "int", nullable: false),
                    DiamondPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOferts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    lore = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("characters_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "decision",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    population = table.Column<int>(type: "int", nullable: true),
                    army = table.Column<int>(type: "int", nullable: true),
                    economy = table.Column<int>(type: "int", nullable: true),
                    magic = table.Column<int>(type: "int", nullable: true),
                    unlockable_character = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("decision_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DiamondOfert",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecioEnPesos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoDeDiamantes = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiamondOfert", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LifeRechargePrice = table.Column<int>(type: "int", nullable: false),
                    DaysToEarnDiamond = table.Column<int>(type: "int", nullable: false),
                    MinutesToEarnLife = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("types_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    maxdays = table.Column<int>(type: "int", nullable: true),
                    maxlives = table.Column<int>(type: "int", nullable: true),
                    lives = table.Column<int>(type: "int", nullable: true),
                    last_life_recharge = table.Column<DateTime>(type: "datetime", nullable: true),
                    gold = table.Column<int>(type: "int", nullable: true),
                    diamonds = table.Column<int>(type: "int", nullable: true),
                    clave = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "card",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeid = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    decision1 = table.Column<int>(type: "int", nullable: true),
                    decision2 = table.Column<int>(type: "int", nullable: true),
                    is_playable = table.Column<bool>(type: "bit", nullable: true),
                    character_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("card_pkey", x => x.id);
                    table.ForeignKey(
                        name: "card_decision1_fkey",
                        column: x => x.decision1,
                        principalTable: "decision",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "card_decision2_fkey",
                        column: x => x.decision2,
                        principalTable: "decision",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "card_typeid_fkey",
                        column: x => x.typeid,
                        principalTable: "types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cards_characters",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DiamondOfferId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_DiamondOfert_DiamondOfferId",
                        column: x => x.DiamondOfferId,
                        principalTable: "DiamondOfert",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "player_characters",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "int", nullable: false),
                    character_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("player_characters_pkey", x => new { x.player_id, x.character_id });
                    table.ForeignKey(
                        name: "player_characters_character_id_fkey",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "player_characters_player_id_fkey",
                        column: x => x.player_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(type: "int", nullable: true),
                    gamestate = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    populationstate = table.Column<int>(type: "int", nullable: true),
                    armystate = table.Column<int>(type: "int", nullable: true),
                    economystate = table.Column<int>(type: "int", nullable: true),
                    magicstate = table.Column<int>(type: "int", nullable: true),
                    lastcardid = table.Column<int>(type: "int", nullable: true),
                    day = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("games_pkey", x => x.id);
                    table.ForeignKey(
                        name: "games_lastcardid_fkey",
                        column: x => x.lastcardid,
                        principalTable: "card",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "games_userid_fkey",
                        column: x => x.userid,
                        principalTable: "usuarios",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_character_id",
                table: "card",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "IX_card_decision1",
                table: "card",
                column: "decision1");

            migrationBuilder.CreateIndex(
                name: "IX_card_decision2",
                table: "card",
                column: "decision2");

            migrationBuilder.CreateIndex(
                name: "IX_card_typeid",
                table: "card",
                column: "typeid");

            migrationBuilder.CreateIndex(
                name: "IX_games_lastcardid",
                table: "games",
                column: "lastcardid");

            migrationBuilder.CreateIndex(
                name: "IX_games_userid",
                table: "games",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DiamondOfferId",
                table: "Invoices",
                column: "DiamondOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_UsuarioId",
                table: "Invoices",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_player_characters_character_id",
                table: "player_characters",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardOferts");

            migrationBuilder.DropTable(
                name: "GameConfig");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "player_characters");

            migrationBuilder.DropTable(
                name: "card");

            migrationBuilder.DropTable(
                name: "DiamondOfert");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "decision");

            migrationBuilder.DropTable(
                name: "types");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropSequence(
                name: "games_id_seq");
        }
    }
}
