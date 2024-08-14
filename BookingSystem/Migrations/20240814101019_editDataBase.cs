using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class editDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            
         

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                type: "DECIMAL(18, 2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                type: "DECIMAL(18, 2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    CustomerId = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    RoleId = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    RoleName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HashedPassword = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RoleId = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true),
                    CustomerId = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_UserLogins_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TESTIMONIAL_CustomerId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_CustomerId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_CustomerId",
                schema: "C##SHAIMAA2",
                table: "UserLogins",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_RoleId",
                schema: "C##SHAIMAA2",
                table: "UserLogins",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BOOKING_Customers_CustomerId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                column: "CustomerId",
                principalSchema: "C##SHAIMAA2",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TESTIMONIAL_Customers_CustomerId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                column: "CustomerId",
                principalSchema: "C##SHAIMAA2",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BOOKING_Customers_CustomerId",
                schema: "C##SHAIMAA2",
                table: "BOOKING");

            migrationBuilder.DropForeignKey(
                name: "FK_TESTIMONIAL_Customers_CustomerId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "C##SHAIMAA2");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "C##SHAIMAA2");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "C##SHAIMAA2");

            migrationBuilder.DropIndex(
                name: "IX_TESTIMONIAL_CustomerId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL");

            migrationBuilder.DropIndex(
                name: "IX_BOOKING_CustomerId",
                schema: "C##SHAIMAA2",
                table: "BOOKING");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                schema: "C##SHAIMAA2",
                table: "BOOKING");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                type: "NVARCHAR2(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                type: "NVARCHAR2(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Discriminator = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LastName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RoleId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RoleId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "C##SHAIMAA2",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "C##SHAIMAA2",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TESTIMONIAL_AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BOOKING_AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "C##SHAIMAA2",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "C##SHAIMAA2",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "\"NormalizedName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "C##SHAIMAA2",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "C##SHAIMAA2",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "C##SHAIMAA2",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "C##SHAIMAA2",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "C##SHAIMAA2",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "\"NormalizedUserName\" IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BOOKING_AspNetUsers_AppUserId",
                schema: "C##SHAIMAA2",
                table: "BOOKING",
                column: "AppUserId",
                principalSchema: "C##SHAIMAA2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TESTIMONIAL_AspNetUsers_AppUserId",
                schema: "C##SHAIMAA2",
                table: "TESTIMONIAL",
                column: "AppUserId",
                principalSchema: "C##SHAIMAA2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
