using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Persistence.Migrations
{
    public partial class AddPluginToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PluginToken",
                table: "Plugin",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PluginToken",
                table: "Plugin");
        }
    }
}
