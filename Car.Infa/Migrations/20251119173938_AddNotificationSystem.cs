using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRental.Infa.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "983beffb-5e13-4f28-9b77-2ad67915d8bc");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9c6aa90b-3e7c-4fb3-964f-a91dff14e644", "f130d233-3fb4-4705-ace8-385a45aba8aa" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c6aa90b-3e7c-4fb3-964f-a91dff14e644");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f130d233-3fb4-4705-ace8-385a45aba8aa");

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailOnReservationCreated = table.Column<bool>(type: "bit", nullable: false),
                    EmailOnReservationUpdated = table.Column<bool>(type: "bit", nullable: false),
                    EmailOnPaymentSuccess = table.Column<bool>(type: "bit", nullable: false),
                    EmailOnPaymentFailed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "014ddded-09e7-4539-b799-e1dc06aac55f", null, "Admin", "ADMIN" },
                    { "bbbc2160-c2ce-4fea-9a7c-74e2d6bdbb94", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "978606ec-3926-4c3c-9a40-9059f1297b36", 0, "0ceeebe7-8bf0-44cd-9a36-e3b1fa58b23e", new DateTime(2025, 11, 19, 17, 39, 37, 704, DateTimeKind.Utc).AddTicks(7265), "admin@car.com", true, "System Admin", false, null, "ADMIN@CAR.COM", "ADMIN@CAR.COM", "AQAAAAIAAYagAAAAED/ltX4A00jilhDYnuGUUO9vNP/WwRWE2Lkg/Rtn0SSe3IQLmmCt99sv0MyZ+kjcVQ==", null, false, "ad0dc489-ea5c-4035-8f66-bc34779c00c5", false, "admin@car.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "014ddded-09e7-4539-b799-e1dc06aac55f", "978606ec-3926-4c3c-9a40-9059f1297b36" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "NotificationPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "NotificationPreferences");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbbc2160-c2ce-4fea-9a7c-74e2d6bdbb94");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "014ddded-09e7-4539-b799-e1dc06aac55f", "978606ec-3926-4c3c-9a40-9059f1297b36" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "014ddded-09e7-4539-b799-e1dc06aac55f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "978606ec-3926-4c3c-9a40-9059f1297b36");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "983beffb-5e13-4f28-9b77-2ad67915d8bc", null, "Customer", "CUSTOMER" },
                    { "9c6aa90b-3e7c-4fb3-964f-a91dff14e644", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f130d233-3fb4-4705-ace8-385a45aba8aa", 0, "c1eae303-d1ff-468a-b652-a3d62c879ba6", new DateTime(2025, 11, 19, 10, 52, 57, 102, DateTimeKind.Utc).AddTicks(4982), "admin@car.com", true, "System Admin", false, null, "ADMIN@CAR.COM", "ADMIN@CAR.COM", "AQAAAAIAAYagAAAAEAU7MXz0yqOIPdRXFqxY7Iwyvzf+NWBM++SvUZfYirWbwiFM/o9AwSPT0dYmo+yuWw==", null, false, "808d3fd1-1282-4aca-9800-ba8755b9f42a", false, "admin@car.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9c6aa90b-3e7c-4fb3-964f-a91dff14e644", "f130d233-3fb4-4705-ace8-385a45aba8aa" });
        }
    }
}
