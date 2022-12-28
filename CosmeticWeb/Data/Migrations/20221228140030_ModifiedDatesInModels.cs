using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticWeb.Data.Migrations
{
    public partial class ModifiedDatesInModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ContactUs",
                newName: "DateCreated");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Blogs",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "ContactUs",
                newName: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedDate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
