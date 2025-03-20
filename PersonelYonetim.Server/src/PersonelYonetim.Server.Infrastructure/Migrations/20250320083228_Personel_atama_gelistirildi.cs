using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Personel_atama_gelistirildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalismaSekli",
                table: "PersonelAtama",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PozisyonBaslamaTarihi",
                table: "PersonelAtama",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PozisyonBitisTarihi",
                table: "PersonelAtama",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SozlesmeBitisTarihi",
                table: "PersonelAtama",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SozlesmeTuru",
                table: "PersonelAtama",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalismaSekli",
                table: "PersonelAtama");

            migrationBuilder.DropColumn(
                name: "PozisyonBaslamaTarihi",
                table: "PersonelAtama");

            migrationBuilder.DropColumn(
                name: "PozisyonBitisTarihi",
                table: "PersonelAtama");

            migrationBuilder.DropColumn(
                name: "SozlesmeBitisTarihi",
                table: "PersonelAtama");

            migrationBuilder.DropColumn(
                name: "SozlesmeTuru",
                table: "PersonelAtama");
        }
    }
}
