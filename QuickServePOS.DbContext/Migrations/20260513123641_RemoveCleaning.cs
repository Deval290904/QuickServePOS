using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickServePOS.DbContextData.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCleaning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMerged",
                table: "RestaurantTables");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMerged",
                table: "RestaurantTables",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
