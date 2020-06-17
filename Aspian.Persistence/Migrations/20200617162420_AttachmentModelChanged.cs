using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class AttachmentModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Attachments");

            migrationBuilder.AddColumn<string>(
                name: "RelativePath",
                table: "Attachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelativePath",
                table: "Attachments");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
