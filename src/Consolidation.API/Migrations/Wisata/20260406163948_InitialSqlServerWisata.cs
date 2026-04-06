using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consolidation.API.Migrations.Wisata
{
    /// <inheritdoc />
    public partial class InitialSqlServerWisata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wisata");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "wisata",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                },
                comment: "user login modul wisata");

            migrationBuilder.CreateTable(
                name: "Wisata",
                schema: "wisata",
                columns: table => new
                {
                    WisataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nama = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Kota = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wisata", x => x.WisataId);
                },
                comment: "master daftar wisata untuk modul interview");

            migrationBuilder.CreateTable(
                name: "UserRefreshToken",
                schema: "wisata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRefreshToken_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "wisata",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "refresh token aktif untuk login wisata");

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_RefreshToken",
                schema: "wisata",
                table: "UserRefreshToken",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshToken_UserId",
                schema: "wisata",
                table: "UserRefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "wisata",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRefreshToken",
                schema: "wisata");

            migrationBuilder.DropTable(
                name: "Wisata",
                schema: "wisata");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "wisata");
        }
    }
}
