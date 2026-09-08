using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class AddProductWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductOfferPriceSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductOfferId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    QuantityOption1From = table.Column<int>(type: "int", nullable: false),
                    QuantityOption1To = table.Column<int>(type: "int", nullable: false),
                    QuantityOption1UnitPrice = table.Column<double>(type: "float", nullable: false),
                    QuantityOption2From = table.Column<int>(type: "int", nullable: false),
                    QuantityOption2To = table.Column<int>(type: "int", nullable: false),
                    QuantityOption2UnitPrice = table.Column<double>(type: "float", nullable: false),
                    QuantityOption3From = table.Column<int>(type: "int", nullable: false),
                    QuantityOption3To = table.Column<int>(type: "int", nullable: false),
                    QuantityOption3UnitPrice = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOfferPriceSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOfferPriceSchedules_ProductOffers_ProductOfferId",
                        column: x => x.ProductOfferId,
                        principalTable: "ProductOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductWeights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UOMId = table.Column<int>(type: "int", nullable: true),
                    PackingUnitId = table.Column<int>(type: "int", nullable: true),
                    PackingQuantity = table.Column<double>(type: "float", nullable: false),
                    LoadingUnitId = table.Column<int>(type: "int", nullable: true),
                    LoadingQuantity = table.Column<double>(type: "float", nullable: false),
                    MOQ = table.Column<double>(type: "float", nullable: false),
                    MOQUnitId = table.Column<int>(type: "int", nullable: true),
                    ProductionQuantity = table.Column<double>(type: "float", nullable: false),
                    ProductionUnitId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductWeights_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductWeights_Units_LoadingUnitId",
                        column: x => x.LoadingUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWeights_Units_MOQUnitId",
                        column: x => x.MOQUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWeights_Units_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWeights_Units_ProductionUnitId",
                        column: x => x.ProductionUnitId,
                        principalTable: "Units",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWeights_Units_UOMId",
                        column: x => x.UOMId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfferPriceSchedules_ProductOfferId",
                table: "ProductOfferPriceSchedules",
                column: "ProductOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeights_LoadingUnitId",
                table: "ProductWeights",
                column: "LoadingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeights_MOQUnitId",
                table: "ProductWeights",
                column: "MOQUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeights_PackingUnitId",
                table: "ProductWeights",
                column: "PackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeights_ProductId",
                table: "ProductWeights",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeights_ProductionUnitId",
                table: "ProductWeights",
                column: "ProductionUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeights_UOMId",
                table: "ProductWeights",
                column: "UOMId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductOfferPriceSchedules");

            migrationBuilder.DropTable(
                name: "ProductWeights");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
