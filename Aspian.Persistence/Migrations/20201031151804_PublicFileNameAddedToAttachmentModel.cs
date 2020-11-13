using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class PublicFileNameAddedToAttachmentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicFileName",
                table: "Attachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicFileName",
                table: "Attachments");
        }
    }
}
