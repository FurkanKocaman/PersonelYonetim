using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class pozisyon_personel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departmanlar_Subeler_SubeId",
                table: "Departmanlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Pozisyonlar_Departmanlar_DepartmanId",
                table: "Pozisyonlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Subeler_Sirketler_SirketId",
                table: "Subeler");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmanId",
                table: "Pozisyonlar",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "SirketId",
                table: "Pozisyonlar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("0195a8e9-a431-79ec-af2f-b5fb3209ef64"));

            migrationBuilder.AddColumn<Guid>(
                name: "SirketId",
                table: "Departmanlar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("0195a8e9-a431-79ec-af2f-b5fb3209ef64"));

            migrationBuilder.CreateIndex(
                name: "IX_Pozisyonlar_SirketId",
                table: "Pozisyonlar",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Departmanlar_SirketId",
                table: "Departmanlar",
                column: "SirketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departmanlar_Sirketler_SirketId",
                table: "Departmanlar",
                column: "SirketId",
                principalTable: "Sirketler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Departmanlar_Subeler_SubeId",
                table: "Departmanlar",
                column: "SubeId",
                principalTable: "Subeler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pozisyonlar_Departmanlar_DepartmanId",
                table: "Pozisyonlar",
                column: "DepartmanId",
                principalTable: "Departmanlar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pozisyonlar_Sirketler_SirketId",
                table: "Pozisyonlar",
                column: "SirketId",
                principalTable: "Sirketler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subeler_Sirketler_SirketId",
                table: "Subeler",
                column: "SirketId",
                principalTable: "Sirketler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departmanlar_Sirketler_SirketId",
                table: "Departmanlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Departmanlar_Subeler_SubeId",
                table: "Departmanlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Pozisyonlar_Departmanlar_DepartmanId",
                table: "Pozisyonlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Pozisyonlar_Sirketler_SirketId",
                table: "Pozisyonlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Subeler_Sirketler_SirketId",
                table: "Subeler");

            migrationBuilder.DropIndex(
                name: "IX_Pozisyonlar_SirketId",
                table: "Pozisyonlar");

            migrationBuilder.DropIndex(
                name: "IX_Departmanlar_SirketId",
                table: "Departmanlar");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Pozisyonlar");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Departmanlar");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepartmanId",
                table: "Pozisyonlar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departmanlar_Subeler_SubeId",
                table: "Departmanlar",
                column: "SubeId",
                principalTable: "Subeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pozisyonlar_Departmanlar_DepartmanId",
                table: "Pozisyonlar",
                column: "DepartmanId",
                principalTable: "Departmanlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subeler_Sirketler_SirketId",
                table: "Subeler",
                column: "SirketId",
                principalTable: "Sirketler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
