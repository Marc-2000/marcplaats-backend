using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Marcplaats_Backend.Migrations
{
    public partial class AddCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryID",
                table: "Advertisements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CategoryID",
                table: "Advertisements",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Categories_CategoryID",
                table: "Advertisements",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Categories_CategoryID",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_CategoryID",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Advertisements");
        }
    }
}
