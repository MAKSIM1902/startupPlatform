using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Сrowdfunding.Data.Migrations
{
    public partial class Campaign2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "Campaigns");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Campaigns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Campaigns");

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
