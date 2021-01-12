using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class PostAttachmentModificationInfoFieldsRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttachments_AspNetUsers_ModifiedById",
                table: "PostAttachments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "PostAttachments");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                table: "PostAttachments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAttachments_ModifiedById",
                table: "PostAttachments",
                newName: "IX_PostAttachments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostAttachments_AspNetUsers_UserId",
                table: "PostAttachments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAttachments_AspNetUsers_UserId",
                table: "PostAttachments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PostAttachments",
                newName: "ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_PostAttachments_UserId",
                table: "PostAttachments",
                newName: "IX_PostAttachments_ModifiedById");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "PostAttachments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAttachments_AspNetUsers_ModifiedById",
                table: "PostAttachments",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
