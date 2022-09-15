using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vmapi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MachineInventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineInventory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmountTendered = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Machine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineInventoryId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Machine_MachineInventory_MachineInventoryId",
                        column: x => x.MachineInventoryId,
                        principalTable: "MachineInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineInventoryLineItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineInventoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineInventoryLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineInventoryLineItem_MachineInventory_MachineInventoryId",
                        column: x => x.MachineInventoryId,
                        principalTable: "MachineInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionLineItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionId = table.Column<long>(type: "INTEGER", nullable: false),
                    MachineId = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLineItem_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Machine_MachineInventoryId",
                table: "Machine",
                column: "MachineInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineInventoryLineItem_MachineInventoryId",
                table: "MachineInventoryLineItem",
                column: "MachineInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLineItem_TransactionId",
                table: "TransactionLineItem",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Machine");

            migrationBuilder.DropTable(
                name: "MachineInventoryLineItem");

            migrationBuilder.DropTable(
                name: "TransactionLineItem");

            migrationBuilder.DropTable(
                name: "MachineInventory");

            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
