using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consolidation.API.Migrations.Sales
{
    /// <inheritdoc />
    public partial class InitialSqlServerSales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sales");

            migrationBuilder.CreateTable(
                name: "currency",
                schema: "sales",
                columns: table => new
                {
                    code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    numeric_code = table.Column<int>(type: "int", nullable: true),
                    minor_unit = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currency", x => x.code);
                },
                comment: "master currency list with ISO codes, symbols, and minor-unit precision");

            migrationBuilder.CreateTable(
                name: "module",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_module", x => x.id);
                },
                comment: "ui control modules used to group ERP navigation areas");

            migrationBuilder.CreateTable(
                name: "salesitem_brand",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salesitem_brand", x => x.id);
                },
                comment: "brand reference data for catalog items");

            migrationBuilder.CreateTable(
                name: "salesitem_type",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salesitem_type", x => x.id);
                },
                comment: "type or category reference data for catalog items");

            migrationBuilder.CreateTable(
                name: "system_config",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    config_key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    config_value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_system_config", x => x.id);
                },
                comment: "key-value configuration entries used by the sales catalog subsystem");

            migrationBuilder.CreateTable(
                name: "unit_measurement",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    measurement_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit_measurement", x => x.id);
                },
                comment: "reference units for length, weight, and quantity measurements");

            migrationBuilder.CreateTable(
                name: "exchange_rate",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_currency_code = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    to_currency_code = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    rate = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    effective_utc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exchange_rate", x => x.id);
                    table.ForeignKey(
                        name: "fk_exchange_rate_currency_from_currency_code",
                        column: x => x.from_currency_code,
                        principalSchema: "sales",
                        principalTable: "currency",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_exchange_rate_currency_to_currency_code",
                        column: x => x.to_currency_code,
                        principalSchema: "sales",
                        principalTable: "currency",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "time-based exchange rates between source and target currencies");

            migrationBuilder.CreateTable(
                name: "menu",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    module_id = table.Column<int>(type: "int", nullable: false),
                    parent_menu_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    icon = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    is_enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    sort_order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_menu", x => x.id);
                    table.ForeignKey(
                        name: "fk_menu_menu_parent_menu_id",
                        column: x => x.parent_menu_id,
                        principalSchema: "sales",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_menu_module_module_id",
                        column: x => x.module_id,
                        principalSchema: "sales",
                        principalTable: "module",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "recursive menu tree used for role-filtered navigation in the UI control layer");

            migrationBuilder.CreateTable(
                name: "salesitem",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    barcode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    base_price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    base_currency_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    picture_file_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sales_item_type_id = table.Column<int>(type: "int", nullable: false),
                    sales_item_brand_id = table.Column<int>(type: "int", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at_utc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    available_stock = table.Column<int>(type: "int", nullable: false),
                    restock_threshold = table.Column<int>(type: "int", nullable: false),
                    max_stock_threshold = table.Column<int>(type: "int", nullable: false),
                    embedding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    on_reorder = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salesitem", x => x.id);
                    table.ForeignKey(
                        name: "fk_salesitem_currency_base_currency_code",
                        column: x => x.base_currency_code,
                        principalSchema: "sales",
                        principalTable: "currency",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_salesitem_salesitem_brand_sales_item_brand_id",
                        column: x => x.sales_item_brand_id,
                        principalSchema: "sales",
                        principalTable: "salesitem_brand",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_salesitem_salesitem_type_sales_item_type_id",
                        column: x => x.sales_item_type_id,
                        principalSchema: "sales",
                        principalTable: "salesitem_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "product catalog items with pricing, stock thresholds, and serialized embedding payload");

            migrationBuilder.CreateTable(
                name: "menu_role",
                schema: "sales",
                columns: table => new
                {
                    menu_id = table.Column<int>(type: "int", nullable: false),
                    role_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_menu_role", x => new { x.menu_id, x.role_name });
                    table.ForeignKey(
                        name: "fk_menu_role_menu_menu_id",
                        column: x => x.menu_id,
                        principalSchema: "sales",
                        principalTable: "menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "role-to-menu visibility mappings used to filter the UI menu tree");

            migrationBuilder.CreateTable(
                name: "salesitem_dimension",
                schema: "sales",
                columns: table => new
                {
                    sales_item_id = table.Column<int>(type: "int", nullable: false),
                    length = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    width = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    height = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    dimension_unit_id = table.Column<int>(type: "int", nullable: true),
                    weight = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    weight_unit_id = table.Column<int>(type: "int", nullable: true),
                    quantity_unit_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salesitem_dimension", x => x.sales_item_id);
                    table.ForeignKey(
                        name: "fk_salesitem_dimension_salesitem_sales_item_id",
                        column: x => x.sales_item_id,
                        principalSchema: "sales",
                        principalTable: "salesitem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_salesitem_dimension_unit_measurement_dimension_unit_id",
                        column: x => x.dimension_unit_id,
                        principalSchema: "sales",
                        principalTable: "unit_measurement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_salesitem_dimension_unit_measurement_quantity_unit_id",
                        column: x => x.quantity_unit_id,
                        principalSchema: "sales",
                        principalTable: "unit_measurement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_salesitem_dimension_unit_measurement_weight_unit_id",
                        column: x => x.weight_unit_id,
                        principalSchema: "sales",
                        principalTable: "unit_measurement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "physical dimensions, weight, and units for a catalog item");

            migrationBuilder.CreateTable(
                name: "salesitem_price",
                schema: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sales_item_id = table.Column<int>(type: "int", nullable: false),
                    currency_code = table.Column<string>(type: "nvarchar(3)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    valid_from = table.Column<DateTime>(type: "datetime2", nullable: true),
                    valid_to = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salesitem_price", x => x.id);
                    table.ForeignKey(
                        name: "fk_salesitem_price_currency_currency_code",
                        column: x => x.currency_code,
                        principalSchema: "sales",
                        principalTable: "currency",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_salesitem_price_salesitem_sales_item_id",
                        column: x => x.sales_item_id,
                        principalSchema: "sales",
                        principalTable: "salesitem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "currency-specific price entries with optional effective date ranges");

            migrationBuilder.CreateIndex(
                name: "ix_currency_code",
                schema: "sales",
                table: "currency",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_exchange_rate_from_currency_code_to_currency_code_effective_utc",
                schema: "sales",
                table: "exchange_rate",
                columns: new[] { "from_currency_code", "to_currency_code", "effective_utc" });

            migrationBuilder.CreateIndex(
                name: "ix_exchange_rate_to_currency_code",
                schema: "sales",
                table: "exchange_rate",
                column: "to_currency_code");

            migrationBuilder.CreateIndex(
                name: "ix_menu_module_id_parent_menu_id_sort_order",
                schema: "sales",
                table: "menu",
                columns: new[] { "module_id", "parent_menu_id", "sort_order" });

            migrationBuilder.CreateIndex(
                name: "ix_menu_parent_menu_id",
                schema: "sales",
                table: "menu",
                column: "parent_menu_id");

            migrationBuilder.CreateIndex(
                name: "ix_menu_role_role_name",
                schema: "sales",
                table: "menu_role",
                column: "role_name");

            migrationBuilder.CreateIndex(
                name: "ix_module_code",
                schema: "sales",
                table: "module",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_barcode",
                schema: "sales",
                table: "salesitem",
                column: "barcode");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_base_currency_code",
                schema: "sales",
                table: "salesitem",
                column: "base_currency_code");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_code",
                schema: "sales",
                table: "salesitem",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_name",
                schema: "sales",
                table: "salesitem",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_sales_item_brand_id",
                schema: "sales",
                table: "salesitem",
                column: "sales_item_brand_id");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_sales_item_type_id",
                schema: "sales",
                table: "salesitem",
                column: "sales_item_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_dimension_dimension_unit_id",
                schema: "sales",
                table: "salesitem_dimension",
                column: "dimension_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_dimension_quantity_unit_id",
                schema: "sales",
                table: "salesitem_dimension",
                column: "quantity_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_dimension_weight_unit_id",
                schema: "sales",
                table: "salesitem_dimension",
                column: "weight_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_price_currency_code",
                schema: "sales",
                table: "salesitem_price",
                column: "currency_code");

            migrationBuilder.CreateIndex(
                name: "ix_salesitem_price_sales_item_id_currency_code",
                schema: "sales",
                table: "salesitem_price",
                columns: new[] { "sales_item_id", "currency_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_system_config_config_key",
                schema: "sales",
                table: "system_config",
                column: "config_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_unit_measurement_measurement_type",
                schema: "sales",
                table: "unit_measurement",
                column: "measurement_type");

            migrationBuilder.CreateIndex(
                name: "ix_unit_measurement_symbol",
                schema: "sales",
                table: "unit_measurement",
                column: "symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exchange_rate",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "menu_role",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "salesitem_dimension",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "salesitem_price",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "system_config",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "menu",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "unit_measurement",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "salesitem",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "module",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "currency",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "salesitem_brand",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "salesitem_type",
                schema: "sales");
        }
    }
}
