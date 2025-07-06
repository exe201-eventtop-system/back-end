using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UNIQUEIDENTIFIER",
                table: "UsedSessionServices",
                newName: "package_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "package_id",
                table: "UsedSessionServices",
                type: "UNIQUEIDENTIFIER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "package_id",
                table: "UsedSessionServices",
                newName: "UNIQUEIDENTIFIER");

            migrationBuilder.AlterColumn<Guid>(
                name: "UNIQUEIDENTIFIER",
                table: "UsedSessionServices",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UNIQUEIDENTIFIER");
        }
    }
}
