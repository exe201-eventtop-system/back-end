using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class adddatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "planning",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    date_of_event = table.Column<DateTime>(type: "datetime2", nullable: true),
                    budget = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    about_number_people = table.Column<int>(type: "int", nullable: false),
                    main_color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    type_of_event = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planning", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sesstion_service",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    planning_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    service_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    create_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    update_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sesstion_service", x => x.id);
                    table.ForeignKey(
                        name: "FK_sesstion_service_planning_planning_id",
                        column: x => x.planning_id,
                        principalTable: "planning",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sesstion_service_planning_id",
                table: "sesstion_service",
                column: "planning_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sesstion_service");

            migrationBuilder.DropTable(
                name: "planning");
        }
    }
}
