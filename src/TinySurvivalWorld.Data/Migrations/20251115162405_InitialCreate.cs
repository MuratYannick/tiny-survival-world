using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinySurvivalWorld.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Factions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequiredEthnicity = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    FoundedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    IsStackable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    MaxStackSize = table.Column<int>(type: "int", nullable: false, defaultValue: 99),
                    Weight = table.Column<float>(type: "float", nullable: false, defaultValue: 1f),
                    BaseValue = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IconPath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxDurability = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Damage = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    Defense = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    HealthRestore = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    HungerRestore = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    ThirstRestore = table.Column<float>(type: "float", nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seed = table.Column<long>(type: "bigint", nullable: false),
                    GameTime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    Difficulty = table.Column<byte>(type: "tinyint unsigned", nullable: false, defaultValue: (byte)1),
                    IsHardcore = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    WorldSizeX = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WorldSizeY = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SpawnPointX = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    SpawnPointY = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastPlayed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GameVersion = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "0.1.0")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    EthnicityType = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: false, defaultValue: 50),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LeaderId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clans_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ethnicity = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    ClanId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorldId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Experience = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Health = table.Column<float>(type: "float", nullable: false, defaultValue: 100f),
                    MaxHealth = table.Column<float>(type: "float", nullable: false, defaultValue: 100f),
                    Hunger = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    Thirst = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    PositionX = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    PositionY = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Clans_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Players_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Players_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Clans_FactionId",
                table: "Clans",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Clans_LeaderId",
                table: "Clans",
                column: "LeaderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clans_Name",
                table: "Clans",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factions_Name",
                table: "Factions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Code",
                table: "Items",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Type",
                table: "Items",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ClanId",
                table: "Players",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_FactionId",
                table: "Players",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_Name",
                table: "Players",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_WorldId",
                table: "Players",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_Name",
                table: "Worlds",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clans_Players_LeaderId",
                table: "Clans",
                column: "LeaderId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clans_Factions_FactionId",
                table: "Clans");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Factions_FactionId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Clans_Players_LeaderId",
                table: "Clans");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Factions");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Clans");

            migrationBuilder.DropTable(
                name: "Worlds");
        }
    }
}
