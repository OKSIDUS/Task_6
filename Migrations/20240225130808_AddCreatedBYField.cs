using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_6.Migrations
{
    public partial class AddCreatedBYField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Pictures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Pictures");
        }
    }
}
