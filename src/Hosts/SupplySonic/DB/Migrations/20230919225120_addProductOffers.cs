using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class addProductOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_ProductOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOffers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOfferAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    ProductOfferId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ProductOfferAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOfferAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOfferAttachments_ProductOffers_ProductOfferId",
                        column: x => x.ProductOfferId,
                        principalTable: "ProductOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOfferAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    SubAttributeId = table.Column<int>(type: "int", nullable: false),
                    ProductOfferId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ProductOfferAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOfferAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOfferAttributes_ProductOffers_ProductOfferId",
                        column: x => x.ProductOfferId,
                        principalTable: "ProductOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOfferAttributes_SubAttributes_SubAttributeId",
                        column: x => x.SubAttributeId,
                        principalTable: "SubAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfferAttachments_AttachmentId",
                table: "ProductOfferAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfferAttachments_ProductOfferId",
                table: "ProductOfferAttachments",
                column: "ProductOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfferAttributes_AttributeId",
                table: "ProductOfferAttributes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfferAttributes_ProductOfferId",
                table: "ProductOfferAttributes",
                column: "ProductOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOfferAttributes_SubAttributeId",
                table: "ProductOfferAttributes",
                column: "SubAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOffers_ProductId",
                table: "ProductOffers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductOfferAttachments");

            migrationBuilder.DropTable(
                name: "ProductOfferAttributes");

            migrationBuilder.DropTable(
                name: "ProductOffers");
        }
    }
}
