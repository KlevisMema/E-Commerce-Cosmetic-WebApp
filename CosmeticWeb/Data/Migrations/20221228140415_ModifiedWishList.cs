using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticWeb.Data.Migrations
{
    public partial class ModifiedWishList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Wishlists_WishlistId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WishlistId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousPrice",
                table: "Wishlists",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Wishlists",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Wishlists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "PreviousPrice",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Wishlists");

            migrationBuilder.AddColumn<Guid>(
                name: "WishlistId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WishlistId",
                table: "Products",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Wishlists_WishlistId",
                table: "Products",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id");
        }
    }
}
