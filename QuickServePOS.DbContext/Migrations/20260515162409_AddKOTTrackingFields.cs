using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickServePOS.DbContextData.Migrations
{
    /// <inheritdoc />
    public partial class AddKOTTrackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmedQuantity",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsKOTGenerated",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ConfirmedQuantity",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "IsKOTGenerated",
                table: "OrderItems");
        }
    }
}
