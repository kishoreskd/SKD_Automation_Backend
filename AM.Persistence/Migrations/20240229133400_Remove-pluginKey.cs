using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Persistence.Migrations
{
    public partial class RemovepluginKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PluginKey",
                table: "Plugin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PluginKey",
                table: "Plugin",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
