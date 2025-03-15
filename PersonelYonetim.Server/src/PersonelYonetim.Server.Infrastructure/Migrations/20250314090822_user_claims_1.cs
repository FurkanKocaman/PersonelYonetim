using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonelYonetim.Server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user_claims_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim<Guid>",
                table: "IdentityUserClaim<Guid>");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<Guid>",
                newName: "UserClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "IdentityUserClaim<Guid>");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim<Guid>",
                table: "IdentityUserClaim<Guid>",
                column: "Id");
        }
    }
}
