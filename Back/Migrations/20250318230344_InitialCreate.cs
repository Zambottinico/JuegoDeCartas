using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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

            migrationBuilder.CreateTable(
                name: "CardOferts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CharacterId = table.Column<int>(type: "integer", nullable: false),
                    GoldPrice = table.Column<int>(type: "integer", nullable: false),
                    DiamondPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardOferts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    lore = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("characters_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "decision",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    population = table.Column<int>(type: "integer", nullable: true),
                    army = table.Column<int>(type: "integer", nullable: true),
                    economy = table.Column<int>(type: "integer", nullable: true),
                    magic = table.Column<int>(type: "integer", nullable: true),
                    unlockable_character = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("decision_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("types_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    maxdays = table.Column<int>(type: "integer", nullable: true),
                    maxlives = table.Column<int>(type: "integer", nullable: true),
                    lives = table.Column<int>(type: "integer", nullable: true),
                    last_life_recharge = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    gold = table.Column<int>(type: "integer", nullable: true),
                    diamonds = table.Column<int>(type: "integer", nullable: true),
                    clave = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuarios_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "card",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    typeid = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    decision1 = table.Column<int>(type: "integer", nullable: true),
                    decision2 = table.Column<int>(type: "integer", nullable: true),
                    is_playable = table.Column<bool>(type: "boolean", nullable: true),
                    character_id = table.Column<int>(type: "integer", nullable: true)
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
                name: "player_characters",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "integer", nullable: false),
                    character_id = table.Column<int>(type: "integer", nullable: false)
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
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    gamestate = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    populationstate = table.Column<int>(type: "integer", nullable: true),
                    armystate = table.Column<int>(type: "integer", nullable: true),
                    economystate = table.Column<int>(type: "integer", nullable: true),
                    magicstate = table.Column<int>(type: "integer", nullable: true),
                    lastcardid = table.Column<int>(type: "integer", nullable: true),
                    day = table.Column<int>(type: "integer", nullable: true)
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
                name: "games");

            migrationBuilder.DropTable(
                name: "player_characters");

            migrationBuilder.DropTable(
                name: "card");

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
