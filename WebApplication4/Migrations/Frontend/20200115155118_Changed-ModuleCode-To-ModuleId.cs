using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication4.Migrations.Frontend
{
    public partial class ChangedModuleCodeToModuleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "module_code",
                table: "group");

            migrationBuilder.AddColumn<int>(
                name: "module_id",
                table: "group",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "module_id",
                table: "group");

            migrationBuilder.AddColumn<string>(
                name: "module_code",
                table: "group",
                nullable: false,
                defaultValue: "");
        }
    }
}
