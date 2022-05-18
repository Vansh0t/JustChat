using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JustChat.Migrations
{
    public partial class UsernameToChatMsg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderUsername",
                table: "ChatMessages",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderUsername",
                table: "ChatMessages");
        }
    }
}
