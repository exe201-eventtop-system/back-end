using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(1024)", nullable: false),
                    parent_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_id", x => x.id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_parent_id",
                        column: x => x.parent_id,
                        principalTable: "Categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PackageStructures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "NEWID()"),
                    admin_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    name = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStructures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "NVARCHAR(128)", nullable: false),
                    description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    parent_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    category_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    supplier_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    location = table.Column<string>(type: "NVARCHAR(128)", nullable: true),
                    thumbnail_url = table.Column<string>(type: "NVARCHAR(512)", nullable: true),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.id);
                    table.ForeignKey(
                        name: "FK_Services_Categories_category_id",
                        column: x => x.category_id,
                        principalTable: "Categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Services_Services_parent_id",
                        column: x => x.parent_id,
                        principalTable: "Services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PackageStructureServices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "NEWID()"),
                    structure_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    service_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    price = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageStructureServices", x => x.id);
                    table.ForeignKey(
                        name: "FK_PackageStructureServices_PackageStructures_structure_id",
                        column: x => x.structure_id,
                        principalTable: "PackageStructures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageStructureServices_Services_service_id",
                        column: x => x.service_id,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceImages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false, defaultValueSql: "NEWID()"),
                    order = table.Column<int>(type: "INT", nullable: false, defaultValue: 0),
                    image_url = table.Column<string>(type: "NVARCHAR(512)", nullable: false),
                    alternative_text = table.Column<string>(type: "NVARCHAR(64)", nullable: false),
                    product_id = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    last_modified_at = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceImages", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServiceImages_Services_product_id",
                        column: x => x.product_id,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_parent_id",
                table: "Categories",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStructureServices_service_id",
                table: "PackageStructureServices",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_PackageStructureServices_structure_id",
                table: "PackageStructureServices",
                column: "structure_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceImages_product_id",
                table: "ServiceImages",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Services_category_id",
                table: "Services",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Services_parent_id",
                table: "Services",
                column: "parent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageStructureServices");

            migrationBuilder.DropTable(
                name: "ServiceImages");

            migrationBuilder.DropTable(
                name: "PackageStructures");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
