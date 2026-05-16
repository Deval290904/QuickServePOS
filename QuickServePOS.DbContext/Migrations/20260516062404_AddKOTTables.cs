using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickServePOS.DbContextData.Migrations
{
    /// <inheritdoc />
    public partial class AddKOTTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KOTs",
                columns: table => new
                {
                    KOTId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KOTNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RestaurantTableId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreparingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOTs", x => x.KOTId);
                    table.ForeignKey(
                        name: "FK_KOTs_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KOTs_RestaurantTables_RestaurantTableId",
                        column: x => x.RestaurantTableId,
                        principalTable: "RestaurantTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KOTItems",
                columns: table => new
                {
                    KOTItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KOTId = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PreparedQuantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SpecialInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreparingAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KOTItems", x => x.KOTItemId);
                    table.ForeignKey(
                        name: "FK_KOTItems_KOTs_KOTId",
                        column: x => x.KOTId,
                        principalTable: "KOTs",
                        principalColumn: "KOTId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KOTItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KOTItems_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KOTItems_KOTId",
                table: "KOTItems",
                column: "KOTId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTItems_MenuItemId",
                table: "KOTItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTItems_OrderItemId",
                table: "KOTItems",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTs_OrderId",
                table: "KOTs",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_KOTs_RestaurantTableId",
                table: "KOTs",
                column: "RestaurantTableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KOTItems");

            migrationBuilder.DropTable(
                name: "KOTs");
        }
    }
}
