using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class editBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                  name: "AppUserId",
                  schema: "C##SHAIMAA2",
                  table: "BOOKING",
                  type: "NVARCHAR2(450)",
                  nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BOOKING_AspNetUsers_AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                column: "AppUserId",
                principalSchema: "C##SHAIMAA2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BOOKING_AspNetUsers_AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING");

            migrationBuilder.DropIndex(
                name: "IX_BOOKING_AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING");

        }
    }
}
