using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PersonelAtama_IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PersonelAtama",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PersonelAtama");
        }
    }
}
