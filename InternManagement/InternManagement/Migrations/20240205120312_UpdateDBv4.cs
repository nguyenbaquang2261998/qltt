using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternManagement.Migrations
{
    public partial class UpdateDBv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassName",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Students");
        }
    }
}
