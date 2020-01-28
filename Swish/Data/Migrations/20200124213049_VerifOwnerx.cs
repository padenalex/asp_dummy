using Microsoft.EntityFrameworkCore.Migrations;

namespace Swish.Data.Migrations
{
    public partial class VerifOwnerx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerID",
                table: "VerificationProfiles",
                newName: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "VerificationProfiles",
                newName: "OwnerID");
        }
    }
}
