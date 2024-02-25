using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternManagement.Migrations
{
    public partial class UpdateDB20240225 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Topics");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Semesters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Semesters",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Semesters");

            migrationBuilder.AddColumn<string>(
                name: "Deadline",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
