using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class yoneticiTipi_RolTipi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YoneticiTipi",
                table: "PersonelAtama");

            migrationBuilder.AddColumn<int>(
                name: "RolTipi",
                table: "PersonelAtama",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RolTipi",
                table: "PersonelAtama");

            migrationBuilder.AddColumn<int>(
                name: "YoneticiTipi",
                table: "PersonelAtama",
                type: "int",
                nullable: true);
        }
    }
}
