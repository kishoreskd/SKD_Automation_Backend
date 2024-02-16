using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AM.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Plugin",
                columns: table => new
                {
                    PluginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PluginName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManualMinutes = table.Column<double>(type: "float", nullable: false),
                    AutomatedMinutes = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plugin", x => x.PluginId);
                    table.ForeignKey(
                        name: "FK_Plugin_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PluginLog",
                columns: table => new
                {
                    PluginLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Activity = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PluginId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginLog", x => x.PluginLogId);
                    table.ForeignKey(
                        name: "FK_Plugin_PluginLogCol_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugin",
                        principalColumn: "PluginId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plugin_DepartmentId",
                table: "Plugin",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PluginLog_PluginId",
                table: "PluginLog",
                column: "PluginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginLog");

            migrationBuilder.DropTable(
                name: "Plugin");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
