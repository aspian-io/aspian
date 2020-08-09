using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class TokenTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Jti",
                table: "Tokens");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Tokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Tokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplacedByToken",
                table: "Tokens",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "Tokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "ReplacedByToken",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "Tokens");

            migrationBuilder.AddColumn<Guid>(
                name: "Jti",
                table: "Tokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
