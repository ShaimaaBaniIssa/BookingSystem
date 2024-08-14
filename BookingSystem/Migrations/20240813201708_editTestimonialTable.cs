using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class editTestimonialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                type: "NVARCHAR2(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "C##SHAIMAA2",
                table: "AspNetUserTokens",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "C##SHAIMAA2",
                table: "AspNetUserTokens",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                schema: "C##SHAIMAA2",
                table: "AspNetUserLogins",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "C##SHAIMAA2",
                table: "AspNetUserLogins",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_TESTIMONIAL_AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TESTIMONIAL_AspNetUsers_AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                column: "AppUserId",
                principalSchema: "C##SHAIMAA2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TESTIMONIAL_AspNetUsers_AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL");

            migrationBuilder.DropIndex(
                name: "IX_TESTIMONIAL_AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "C##SHAIMAA2",
                table: "AspNetUserTokens",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "C##SHAIMAA2",
                table: "AspNetUserTokens",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                schema: "C##SHAIMAA2",
                table: "AspNetUserLogins",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "C##SHAIMAA2",
                table: "AspNetUserLogins",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");
        }
    }
}
