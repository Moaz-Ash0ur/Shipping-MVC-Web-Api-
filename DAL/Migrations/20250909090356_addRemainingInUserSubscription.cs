using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addRemainingInUserSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RemainingKm",
                table: "TbUserSubscriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingShipments",
                table: "TbUserSubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RemainingWeight",
                table: "TbUserSubscriptions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingKm",
                table: "TbUserSubscriptions");

            migrationBuilder.DropColumn(
                name: "RemainingShipments",
                table: "TbUserSubscriptions");

            migrationBuilder.DropColumn(
                name: "RemainingWeight",
                table: "TbUserSubscriptions");
        }
    }
}
