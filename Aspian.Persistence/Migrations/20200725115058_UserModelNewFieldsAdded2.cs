using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aspian.Persistence.Migrations
{
    public partial class UserModelNewFieldsAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginDate",
                table: "AspNetUsers");
        }
    }
}
