using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DB.Migrations
{
    public partial class deleteLastTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AppUsers_AppUserId",
                table: "RefreshTokens");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "RefreshTokens",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SupplierAppUserId",
                table: "RefreshTokens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthdate",
                table: "AppUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "AppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceCountryId",
                table: "AppUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateName",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameUzbek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRussian = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameUzbek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRussian = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameUzbek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameRussian = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierAppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    resetPasswordCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resetPasswordCodeTimeOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    UserCategory = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileAttachmentId = table.Column<int>(type: "int", nullable: true),
                    ResidenceCountryId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierAppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierAppUsers_Attachments_ProfileAttachmentId",
                        column: x => x.ProfileAttachmentId,
                        principalTable: "Attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierAppUsers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierAppUsers_Countries_ResidenceCountryId",
                        column: x => x.ResidenceCountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_SupplierAppUserId",
                table: "RefreshTokens",
                column: "SupplierAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_CityId",
                table: "AppUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_ResidenceCountryId",
                table: "AppUsers",
                column: "ResidenceCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierAppUsers_CityId",
                table: "SupplierAppUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierAppUsers_ProfileAttachmentId",
                table: "SupplierAppUsers",
                column: "ProfileAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierAppUsers_ResidenceCountryId",
                table: "SupplierAppUsers",
                column: "ResidenceCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Cities_CityId",
                table: "AppUsers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Countries_ResidenceCountryId",
                table: "AppUsers",
                column: "ResidenceCountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AppUsers_AppUserId",
                table: "RefreshTokens",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_SupplierAppUsers_SupplierAppUserId",
                table: "RefreshTokens",
                column: "SupplierAppUserId",
                principalTable: "SupplierAppUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Cities_CityId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Countries_ResidenceCountryId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AppUsers_AppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_SupplierAppUsers_SupplierAppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Nationalities");

            migrationBuilder.DropTable(
                name: "SupplierAppUsers");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_SupplierAppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_CityId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_ResidenceCountryId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "SupplierAppUserId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ResidenceCountryId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "StateName",
                table: "AppUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "RefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AppUsers_AppUserId",
                table: "RefreshTokens",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
