using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consolidation.API.Migrations.Webhooks
{
    /// <inheritdoc />
    public partial class InitialSqlServerWebhooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "webhooksdb");

            migrationBuilder.CreateTable(
                name: "subscription",
                schema: "webhooksdb",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dest_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscription", x => x.id);
                },
                comment: "registered webhook endpoints by event type and subscribing user");

            migrationBuilder.CreateIndex(
                name: "ix_subscription_type",
                schema: "webhooksdb",
                table: "subscription",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "ix_subscription_user_id",
                schema: "webhooksdb",
                table: "subscription",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subscription",
                schema: "webhooksdb");
        }
    }
}
