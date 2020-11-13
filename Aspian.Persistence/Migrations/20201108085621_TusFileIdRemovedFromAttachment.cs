using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class TusFileIdRemovedFromAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_FileTusId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "FileTusId",
                table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Attachments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FileName",
                table: "Attachments",
                column: "FileName",
                unique: true,
                filter: "[FileName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attachments_FileName",
                table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileTusId",
                table: "Attachments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FileTusId",
                table: "Attachments",
                column: "FileTusId",
                unique: true,
                filter: "[FileTusId] IS NOT NULL");
        }
    }
}
