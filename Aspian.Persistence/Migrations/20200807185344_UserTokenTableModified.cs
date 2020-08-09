using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class UserTokenTableModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_AspNetUsers_UserId1",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_UserId1",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Tokens",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_CreatedById",
                table: "Tokens",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_AspNetUsers_CreatedById",
                table: "Tokens",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_AspNetUsers_CreatedById",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_CreatedById",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Tokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Tokens",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId1",
                table: "Tokens",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_AspNetUsers_UserId1",
                table: "Tokens",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
