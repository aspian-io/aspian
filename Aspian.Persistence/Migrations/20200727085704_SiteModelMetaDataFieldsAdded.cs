using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class SiteModelMetaDataFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activated",
                table: "Sites");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sites",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Sites",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIPAddress",
                table: "Sites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_ModifiedById",
                table: "Sites",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_AspNetUsers_ModifiedById",
                table: "Sites",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sites_AspNetUsers_ModifiedById",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_ModifiedById",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "UserIPAddress",
                table: "Sites");

            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "Sites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
