using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateTbShipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CarrierId",
                table: "TbShippments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_ShippingPackgingId",
                table: "TbShippments",
                column: "ShippingPackgingId");

            migrationBuilder.CreateIndex(
                name: "IX_TbShippments_CarrierId",
                table: "TbShippments",
                column: "CarrierId");

            // Foreign Keys
            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbShippingPackages_ShippingPackgingId",
                table: "TbShippments",
                column: "ShippingPackgingId",
                principalTable: "TbShippingPackages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TbShippments_TbCarriers_CarrierId",
                table: "TbShippments",
                column: "CarrierId",
                principalTable: "TbCarriers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                  name: "FK_TbShippments_TbShippingPackages_ShippingPackgingId",
                  table: "TbShippments");

            migrationBuilder.DropForeignKey(
                name: "FK_TbShippments_TbCarriers_CarrierId",
                table: "TbShippments");

            migrationBuilder.DropIndex(
                name: "IX_TbShippments_ShippingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropIndex(
                name: "IX_TbShippments_CarrierId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "ShippingPackgingId",
                table: "TbShippments");

            migrationBuilder.DropColumn(
                name: "CarrierId",
                table: "TbShippments");
        }
    }
}
