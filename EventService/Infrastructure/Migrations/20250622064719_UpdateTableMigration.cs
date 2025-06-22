using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "returned_time",
                table: "UsedSessionServices",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<string>(
                name: "returned_condition",
                table: "UsedSessionServices",
                type: "NVARCHAR(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)");

            migrationBuilder.AlterColumn<string>(
                name: "initial_condition",
                table: "UsedSessionServices",
                type: "NVARCHAR(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "delivered_time",
                table: "UsedSessionServices",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<string>(
                name: "customer_note",
                table: "UsedSessionServices",
                type: "NVARCHAR(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "returned_time",
                table: "UsedSessionServices",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "returned_condition",
                table: "UsedSessionServices",
                type: "NVARCHAR(256)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "initial_condition",
                table: "UsedSessionServices",
                type: "NVARCHAR(256)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "delivered_time",
                table: "UsedSessionServices",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customer_note",
                table: "UsedSessionServices",
                type: "NVARCHAR(256)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)",
                oldNullable: true);
        }
    }
}
