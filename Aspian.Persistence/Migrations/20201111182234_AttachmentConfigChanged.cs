using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class AttachmentConfigChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkAccessibility",
                table: "Attachments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LinkAccessibility",
                table: "Attachments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
