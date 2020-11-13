using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class LinkAccessibilityAddedToAtachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkAccessibility",
                table: "Attachments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkAccessibility",
                table: "Attachments");
        }
    }
}
