using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consolidation.API.Migrations.Identity
{
    /// <inheritdoc />
    public partial class InitialSqlServerIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.CreateTable(
                name: "asp_net_role",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role", x => x.id);
                },
                comment: "application roles used for authorization (e.g., Admin, User)");

            migrationBuilder.CreateTable(
                name: "asp_net_user",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    card_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    security_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    card_holder_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    card_type = table.Column<int>(type: "int", nullable: false),
                    street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zip_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    security_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "bit", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "bit", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "bit", nullable: false),
                    access_failed_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user", x => x.id);
                },
                comment: "application users stored by ASP.NET Identity");

            migrationBuilder.CreateTable(
                name: "open_iddict_application",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    application_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    client_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    client_secret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    client_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    consent_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    display_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_names = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    json_web_key_set = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    post_logout_redirect_uris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redirect_uris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settings = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_application", x => x.id);
                },
                comment: "registered clients (Angular, mobile apps, APIs)");

            migrationBuilder.CreateTable(
                name: "open_iddict_scope",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_names = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resources = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_scope", x => x.id);
                },
                comment: "allowed API scopes");

            migrationBuilder.CreateTable(
                name: "asp_net_role_claim",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    claim_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claim_value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claim_asp_net_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "asp_net_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "claims attached to roles for role-based permissions");

            migrationBuilder.CreateTable(
                name: "asp_net_user_claim",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    claim_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    claim_value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claim_asp_net_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "asp_net_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "claims assigned directly to users (user-specific permissions or attributes)");

            migrationBuilder.CreateTable(
                name: "asp_net_user_login",
                schema: "identity",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    provider_key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    provider_display_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_login_asp_net_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "asp_net_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "external login providers linked to a user (Google, Facebook, etc.)");

            migrationBuilder.CreateTable(
                name: "asp_net_user_role",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    role_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_role_asp_net_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "identity",
                        principalTable: "asp_net_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_role_asp_net_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "asp_net_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "many-to-many (BridgeTable) relationship between users and roles");

            migrationBuilder.CreateTable(
                name: "asp_net_user_token",
                schema: "identity",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    login_provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_token_asp_net_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "identity",
                        principalTable: "asp_net_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "tokens associated with users (password reset tokens, refresh tokens, etc.)");

            migrationBuilder.CreateTable(
                name: "open_iddict_authorization",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    application_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_authorization", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_iddict_authorization_open_iddict_application_application_id",
                        column: x => x.application_id,
                        principalSchema: "identity",
                        principalTable: "open_iddict_application",
                        principalColumn: "id");
                },
                comment: "user consent / login sessions");

            migrationBuilder.CreateTable(
                name: "open_iddict_token",
                schema: "identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    application_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    authorization_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    concurrency_token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expiration_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    redemption_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reference_id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_iddict_token_open_iddict_application_application_id",
                        column: x => x.application_id,
                        principalSchema: "identity",
                        principalTable: "open_iddict_application",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_open_iddict_token_open_iddict_authorization_authorization_id",
                        column: x => x.authorization_id,
                        principalSchema: "identity",
                        principalTable: "open_iddict_authorization",
                        principalColumn: "id");
                },
                comment: "access tokens, refresh tokens");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "identity",
                table: "asp_net_role",
                column: "normalized_name",
                unique: true,
                filter: "[normalized_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claim_role_id",
                schema: "identity",
                table: "asp_net_role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "identity",
                table: "asp_net_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "identity",
                table: "asp_net_user",
                column: "normalized_user_name",
                unique: true,
                filter: "[normalized_user_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claim_user_id",
                schema: "identity",
                table: "asp_net_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_login_user_id",
                schema: "identity",
                table: "asp_net_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_role_role_id",
                schema: "identity",
                table: "asp_net_user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_application_client_id",
                schema: "identity",
                table: "open_iddict_application",
                column: "client_id",
                unique: true,
                filter: "[client_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_authorization_application_id_status_subject_type",
                schema: "identity",
                table: "open_iddict_authorization",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_scope_name",
                schema: "identity",
                table: "open_iddict_scope",
                column: "name",
                unique: true,
                filter: "[name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_token_application_id_status_subject_type",
                schema: "identity",
                table: "open_iddict_token",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_token_authorization_id",
                schema: "identity",
                table: "open_iddict_token",
                column: "authorization_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_token_reference_id",
                schema: "identity",
                table: "open_iddict_token",
                column: "reference_id",
                unique: true,
                filter: "[reference_id] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asp_net_role_claim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "asp_net_user_claim",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "asp_net_user_login",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "asp_net_user_role",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "asp_net_user_token",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "open_iddict_scope",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "open_iddict_token",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "asp_net_role",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "asp_net_user",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "open_iddict_authorization",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "open_iddict_application",
                schema: "identity");
        }
    }
}
