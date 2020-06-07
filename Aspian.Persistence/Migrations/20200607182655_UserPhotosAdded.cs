using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class UserPhotosAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoOwnerId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PhotoOwnerId",
                table: "Posts",
                column: "PhotoOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_PhotoOwnerId",
                table: "Posts",
                column: "PhotoOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_PhotoOwnerId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PhotoOwnerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PhotoOwnerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");
        }
    }
}
