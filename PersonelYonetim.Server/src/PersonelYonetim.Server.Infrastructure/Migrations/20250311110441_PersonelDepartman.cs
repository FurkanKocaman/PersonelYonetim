using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PersonelDepartman : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmanId",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "PozisyonId",
                table: "Personeller");

            migrationBuilder.CreateTable(
                name: "PersonelDepartman",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PozisyonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelDepartman", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelDepartman_Departmanlar_DepartmanId",
                        column: x => x.DepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonelDepartman_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonelDepartman_Pozisyonlar_PozisyonId",
                        column: x => x.PozisyonId,
                        principalTable: "Pozisyonlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDepartman_DepartmanId",
                table: "PersonelDepartman",
                column: "DepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDepartman_PersonelId",
                table: "PersonelDepartman",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDepartman_PozisyonId",
                table: "PersonelDepartman",
                column: "PozisyonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonelDepartman");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmanId",
                table: "Personeller",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PozisyonId",
                table: "Personeller",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
