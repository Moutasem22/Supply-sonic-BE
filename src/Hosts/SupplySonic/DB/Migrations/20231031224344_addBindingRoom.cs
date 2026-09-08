using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class addBindingRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupplier",
                table: "SupplierAppUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SupplierAppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BindingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierAppUserId = table.Column<int>(type: "int", nullable: false),
                    ProductNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductMainCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductSubCategoryId = table.Column<int>(type: "int", nullable: false),
                    MainDescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainDescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    Qaunt = table.Column<int>(type: "int", nullable: false),
                    UOMId = table.Column<int>(type: "int", nullable: true),
                    IsNegotiate = table.Column<bool>(type: "bit", nullable: true),
                    IsSupplier = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BindingRoomStatus = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_BindingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BindingRooms_ProductMainCategories_ProductMainCategoryId",
                        column: x => x.ProductMainCategoryId,
                        principalTable: "ProductMainCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRooms_ProductSubCategories_ProductSubCategoryId",
                        column: x => x.ProductSubCategoryId,
                        principalTable: "ProductSubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRooms_SupplierAppUsers_SupplierAppUserId",
                        column: x => x.SupplierAppUserId,
                        principalTable: "SupplierAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRooms_Units_UOMId",
                        column: x => x.UOMId,
                        principalTable: "Units",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAddresss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierAppUserId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    SecondPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefalut = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_UserAddresss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresss_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAddresss_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAddresss_SupplierAppUsers_SupplierAppUserId",
                        column: x => x.SupplierAppUserId,
                        principalTable: "SupplierAppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BindingRoomAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentId = table.Column<int>(type: "int", nullable: false),
                    BindingRoomId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BindingRoomAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BindingRoomAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRoomAttachments_BindingRooms_BindingRoomId",
                        column: x => x.BindingRoomId,
                        principalTable: "BindingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BindingRoomAttributess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    SubAttributeId = table.Column<int>(type: "int", nullable: false),
                    BindingRoomId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_BindingRoomAttributess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BindingRoomAttributess_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRoomAttributess_BindingRooms_BindingRoomId",
                        column: x => x.BindingRoomId,
                        principalTable: "BindingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRoomAttributess_SubAttributes_SubAttributeId",
                        column: x => x.SubAttributeId,
                        principalTable: "SubAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BindingRoomRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BindingRoomId = table.Column<int>(type: "int", nullable: false),
                    SupplierAppUserId = table.Column<int>(type: "int", nullable: true),
                    IsNegotiate = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_BindingRoomRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BindingRoomRequests_BindingRooms_BindingRoomId",
                        column: x => x.BindingRoomId,
                        principalTable: "BindingRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BindingRoomRequests_SupplierAppUsers_SupplierAppUserId",
                        column: x => x.SupplierAppUserId,
                        principalTable: "SupplierAppUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierAppUserId = table.Column<int>(type: "int", nullable: true),
                    UserAddressId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    ConfirmOrder = table.Column<bool>(type: "bit", nullable: true),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: true),
                    OrderStatus = table.Column<int>(type: "int", nullable: true),
                    ShippingCost = table.Column<double>(type: "float", nullable: true),
                    OrderCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_SupplierAppUsers_SupplierAppUserId",
                        column: x => x.SupplierAppUserId,
                        principalTable: "SupplierAppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_UserAddresss_UserAddressId",
                        column: x => x.UserAddressId,
                        principalTable: "UserAddresss",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductOfferId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Qaunt = table.Column<int>(type: "int", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
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
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_ProductOffers_ProductOfferId",
                        column: x => x.ProductOfferId,
                        principalTable: "ProductOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomAttachments_AttachmentId",
                table: "BindingRoomAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomAttachments_BindingRoomId",
                table: "BindingRoomAttachments",
                column: "BindingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomAttributess_AttributeId",
                table: "BindingRoomAttributess",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomAttributess_BindingRoomId",
                table: "BindingRoomAttributess",
                column: "BindingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomAttributess_SubAttributeId",
                table: "BindingRoomAttributess",
                column: "SubAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomRequests_BindingRoomId",
                table: "BindingRoomRequests",
                column: "BindingRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRoomRequests_SupplierAppUserId",
                table: "BindingRoomRequests",
                column: "SupplierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRooms_ProductMainCategoryId",
                table: "BindingRooms",
                column: "ProductMainCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRooms_ProductSubCategoryId",
                table: "BindingRooms",
                column: "ProductSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRooms_SupplierAppUserId",
                table: "BindingRooms",
                column: "SupplierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BindingRooms_UOMId",
                table: "BindingRooms",
                column: "UOMId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductOfferId",
                table: "OrderDetails",
                column: "ProductOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierAppUserId",
                table: "Orders",
                column: "SupplierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserAddressId",
                table: "Orders",
                column: "UserAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresss_CityId",
                table: "UserAddresss",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresss_CountryId",
                table: "UserAddresss",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresss_SupplierAppUserId",
                table: "UserAddresss",
                column: "SupplierAppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BindingRoomAttachments");

            migrationBuilder.DropTable(
                name: "BindingRoomAttributess");

            migrationBuilder.DropTable(
                name: "BindingRoomRequests");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "BindingRooms");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UserAddresss");

            migrationBuilder.DropColumn(
                name: "IsSupplier",
                table: "SupplierAppUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupplierAppUsers");
        }
    }
}
