using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Marcplaats_Backend.Migrations
{
    public partial class FixedAdvertisement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Categories_CategoryID",
                table: "Advertisements");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryID",
                table: "Advertisements",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryID",
                table: "Advertisements",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Categories_CategoryID",
                table: "Advertisements",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
