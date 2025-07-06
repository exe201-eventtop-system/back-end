using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thumbnail_url = table.Column<string>(type: "NVARCHAR(256)", nullable: false),
                    name = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    description = table.Column<string>(type: "NVARCHAR(256)", nullable: false),
                    is_deleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    customer_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "NVARCHAR(128)", nullable: false),
                    description = table.Column<string>(type: "NVARCHAR(2048)", nullable: false),
                    location = table.Column<string>(type: "NVARCHAR(128)", nullable: false),
                    start_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    end_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    event_thumbnail = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    number_of_people = table.Column<int>(type: "INT", nullable: false),
                    main_color_hex = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    secondary_color_hex = table.Column<string>(type: "NVARCHAR(6)", nullable: false),
                    event_type_id = table.Column<int>(type: "INT", nullable: false),
                    status = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    is_deleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_event_type_id",
                        column: x => x.event_type_id,
                        principalTable: "EventTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsedSessionServices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    event_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    service_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    UNIQUEIDENTIFIER = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    customer_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    supplier_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    rent_start_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    rent_end_time = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    delivered_time = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    returned_time = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    customer_note = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    damage_type = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    initial_condition = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    returned_condition = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    status = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    unit_price = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    quantity = table.Column<int>(type: "INT", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedSessionServices", x => x.id);
                    table.ForeignKey(
                        name: "FK_UsedSessionServices_Events_event_id",
                        column: x => x.event_id,
                        principalTable: "Events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_event_type_id",
                table: "Events",
                column: "event_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_UsedSessionServices_event_id",
                table: "UsedSessionServices",
                column: "event_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsedSessionServices");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");
        }
    }
}
