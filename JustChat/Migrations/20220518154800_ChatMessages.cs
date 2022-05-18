using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustChat.Migrations
{
    public partial class ChatMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_AppUser_SenderId",
                table: "ChatMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessage",
                table: "ChatMessage");

            migrationBuilder.RenameTable(
                name: "ChatMessage",
                newName: "ChatMessages");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_SenderId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_SenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SendDateTime",
                table: "ChatMessages",
                column: "SendDateTime");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AppUser_SenderId",
                table: "ChatMessages",
                column: "SenderId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AppUser_SenderId",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_SendDateTime",
                table: "ChatMessages");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "ChatMessage");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessage",
                newName: "IX_ChatMessage_SenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessage",
                table: "ChatMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_AppUser_SenderId",
                table: "ChatMessage",
                column: "SenderId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
