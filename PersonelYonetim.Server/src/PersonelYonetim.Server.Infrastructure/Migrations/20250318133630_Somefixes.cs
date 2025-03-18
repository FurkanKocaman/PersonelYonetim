using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Somefixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonelAtama_Personeller_Id",
                table: "PersonelAtama");

            migrationBuilder.AlterColumn<int>(
                name: "YoneticiTipi",
                table: "PersonelAtama",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "SirketId",
                table: "PersonelAtama",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonelAtama_PersonelId",
                table: "PersonelAtama",
                column: "PersonelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonelAtama_Personeller_PersonelId",
                table: "PersonelAtama",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonelAtama_Personeller_PersonelId",
                table: "PersonelAtama");

            migrationBuilder.DropIndex(
                name: "IX_PersonelAtama_PersonelId",
                table: "PersonelAtama");

            migrationBuilder.AlterColumn<int>(
                name: "YoneticiTipi",
                table: "PersonelAtama",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SirketId",
                table: "PersonelAtama",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonelAtama_Personeller_Id",
                table: "PersonelAtama",
                column: "Id",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
