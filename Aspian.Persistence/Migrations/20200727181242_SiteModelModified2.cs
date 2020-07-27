using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class SiteModelModified2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadHostDeativatesAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MainHostDeativatesAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ModuleDeativatesAt",
                table: "Sites");

            migrationBuilder.AddColumn<DateTime>(
                name: "DownloadHostExpiresAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MainHostExpiresAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModuleExpiresAt",
                table: "Sites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadHostExpiresAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MainHostExpiresAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ModuleExpiresAt",
                table: "Sites");

            migrationBuilder.AddColumn<DateTime>(
                name: "DownloadHostDeativatesAt",
                table: "Sites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MainHostDeativatesAt",
                table: "Sites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModuleDeativatesAt",
                table: "Sites",
                type: "datetime2",
                nullable: true);
        }
    }
}
