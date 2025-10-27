using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRental.Infa.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04e5025a-085c-4900-82f6-a957f41a46cc", null, "Customer", "CUSTOMER" },
                    { "4f51621c-34bd-4f94-83b5-bd555651bef7", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d11d1899-741d-445e-8a35-e040cbc71d0c", 0, "2bce2168-2c3c-492f-94db-5d1f6d412507", new DateTime(2025, 10, 24, 11, 46, 45, 977, DateTimeKind.Utc).AddTicks(2531), "admin@car.com", true, "System Admin", false, null, "ADMIN@CAR.COM", "ADMIN@CAR.COM", "AQAAAAIAAYagAAAAEKfLEVInm+n+yc1etxV1nkX5zTrHqgQKZNHQQDZ9W73gmdjoc94SIiOa6cCePp6JFA==", null, false, "c2886b22-9f90-4655-a801-47711c20f294", false, "admin@car.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4f51621c-34bd-4f94-83b5-bd555651bef7", "d11d1899-741d-445e-8a35-e040cbc71d0c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04e5025a-085c-4900-82f6-a957f41a46cc");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4f51621c-34bd-4f94-83b5-bd555651bef7", "d11d1899-741d-445e-8a35-e040cbc71d0c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f51621c-34bd-4f94-83b5-bd555651bef7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d11d1899-741d-445e-8a35-e040cbc71d0c");
        }
    }
}
