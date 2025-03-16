using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Name_Ad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sube",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sube",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sirket",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sirket",
                newName: "Aciklama");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Sube",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "Sube",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Sirket",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "Sirket",
                newName: "Description");
        }
    }
}
