using Microsoft.EntityFrameworkCore.Migrations;

namespace Swish.Data.Migrations
{
    public partial class VerifOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "VerificationProfiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VerificationProfiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "VerificationProfiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VerificationProfiles");
        }
    }
}
