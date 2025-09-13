using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateTbSgipmentState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
      name: "FK_TbShippmentStatus_TbCarriers",
      table: "TbShippmentStatus");



            // 3- Drop Column
            migrationBuilder.DropColumn(
                name: "CarrierId",
                table: "TbShippmentStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                 name: "CarrierId",
                 table: "TbShippmentStatus",
                 type: "uniqueidentifier",
                 nullable: true);

         
            // 3- Add Foreign Key again
            migrationBuilder.AddForeignKey(
                name: "FK_TbShippmentStatus_TbCarriers",
                table: "TbShippmentStatus",
                column: "CarrierId",
                principalTable: "TbCarriers",
                principalColumn: "Id");
        }
    }
}
