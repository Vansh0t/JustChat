using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JustChat.Migrations
{
    public partial class RefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Users_SenderId",
                table: "ChatMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "AppUser");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username_Email",
                table: "AppUser",
                newName: "IX_AppUser_Username_Email");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordResetToken",
                table: "AppUser",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailVrfToken",
                table: "AppUser",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AppUser",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: true),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_AppUser_SenderId",
                table: "ChatMessage",
                column: "SenderId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_AppUser_SenderId",
                table: "ChatMessage");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_AppUser_Username_Email",
                table: "Users",
                newName: "IX_Users_Username_Email");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailVrfToken",
                table: "Users",
                type: "character varying(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Users_SenderId",
                table: "ChatMessage",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
