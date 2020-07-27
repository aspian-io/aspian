using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class SiteModelModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Registered",
                table: "Sites");

            migrationBuilder.AddColumn<DateTime>(
                name: "DownloadHostActivatedAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DownloadHostAvailableSpace",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DownloadHostCapacity",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DownloadHostDeativatesAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasDownloadHost",
                table: "Sites",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MainHostActivatedAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MainHostAvailableSpace",
                table: "Sites",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MainHostCapacity",
                table: "Sites",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "MainHostDeativatesAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModuleActivatedAt",
                table: "Sites",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModuleDeativatesAt",
                table: "Sites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadHostActivatedAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "DownloadHostAvailableSpace",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "DownloadHostCapacity",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "DownloadHostDeativatesAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "HasDownloadHost",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MainHostActivatedAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MainHostAvailableSpace",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MainHostCapacity",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MainHostDeativatesAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ModuleActivatedAt",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ModuleDeativatesAt",
                table: "Sites");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Sites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Registered",
                table: "Sites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
