using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinySurvivalWorld.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPlayerToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clans_Players_LeaderId",
                table: "Clans");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Clans_LeaderId",
                table: "Clans");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Clans");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Clans",
                newName: "FoundedDate");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Clans",
                type: "varchar(5)",
                maxLength: 5,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPlayer = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Ethnicity = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    ClanId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorldId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsClanLeader = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IsFactionLeader = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Clans_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Characters_Factions_FactionId",
                        column: x => x.FactionId,
                        principalTable: "Factions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Characters_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ClanId",
                table: "Characters",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FactionId",
                table: "Characters",
                column: "FactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IsClanLeader",
                table: "Characters",
                column: "IsClanLeader");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IsFactionLeader",
                table: "Characters",
                column: "IsFactionLeader");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_IsPlayer",
                table: "Characters",
                column: "IsPlayer");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_WorldId",
                table: "Characters",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Clans");

            migrationBuilder.RenameColumn(
                name: "FoundedDate",
                table: "Clans",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<Guid>(
                name: "LeaderId",
                table: "Clans",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClanId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FactionId = table.Column<int>(type: "int", nullable: true),
                    WorldId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Ethnicity = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Health = table.Column<float>(type: "float", nullable: false, defaultValue: 100f),
                    Hunger = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    MaxHealth = table.Column<float>(type: "float", nullable: false, defaultValue: 100f),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PositionX = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    PositionY = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    Thirst = table.Column<float>(type: "float", nullable: false, defaultValue: 0f),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                name: "IX_Clans_LeaderId",
                table: "Clans",
                column: "LeaderId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Clans_Players_LeaderId",
                table: "Clans",
                column: "LeaderId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
