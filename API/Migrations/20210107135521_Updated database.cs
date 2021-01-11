using Microsoft.EntityFrameworkCore.Migrations;

namespace Marcplaats_Backend.Migrations
{
    public partial class Updateddatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_ChatID",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Users_UserID",
                table: "ChatUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser");

            migrationBuilder.RenameTable(
                name: "ChatUser",
                newName: "ChatUsers");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUser_ChatID",
                table: "ChatUsers",
                newName: "IX_ChatUsers_ChatID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUsers",
                table: "ChatUsers",
                columns: new[] { "UserID", "ChatID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUsers_Chats_ChatID",
                table: "ChatUsers",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUsers_Users_UserID",
                table: "ChatUsers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUsers_Chats_ChatID",
                table: "ChatUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUsers_Users_UserID",
                table: "ChatUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatUsers",
                table: "ChatUsers");

            migrationBuilder.RenameTable(
                name: "ChatUsers",
                newName: "ChatUser");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUsers_ChatID",
                table: "ChatUser",
                newName: "IX_ChatUser_ChatID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatUser",
                table: "ChatUser",
                columns: new[] { "UserID", "ChatID" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_ChatID",
                table: "ChatUser",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Users_UserID",
                table: "ChatUser",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
