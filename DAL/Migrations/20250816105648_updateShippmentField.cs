using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateShippmentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbUserSenders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserSenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "TbUserReceivers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OtherAddress",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "TbUserReceivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "TbShippments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ShipingPackgingId",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingPackgingId",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "TbShippingPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipingPackgingAname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShipingPackgingEname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbShippingPackages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ShippingPackgingId",
                table: "TbShippments",
                column: "ShippingPackgingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShippingPackages_ShippingPackgingId",
                table: "TbShippments",
                column: "ShippingPackgingId",
                principalTable: "TbShippingPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbShippingPackages_ShippingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropTable(
                name: "TbShippingPackages");

            migrationBuilder.DropIndex(
                name: "IX_TbShippments_ShippingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TbUserSenders");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbUserSenders");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserSenders");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserSenders");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "OtherAddress",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "TbUserReceivers");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "ShipingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "ShippingPackgingId",
                table: "TbShippments");
        }
    }
}
