using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternManagement.Migrations
{
    public partial class UpdateDBv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "Students",
                newName: "ClassCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClassCode",
                table: "Students",
                newName: "ClassId");
        }
    }
}
