using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRental.Infa.Migrations
{
    /// <inheritdoc />
    public partial class AddCarImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14b915c7-5ede-42c8-aa61-d4d3bbdfe411", null, "Admin", "ADMIN" },
                    { "1eb1703e-8166-4253-8a5b-db382308ff18", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "01f21577-b21e-4dce-bfc9-386eb0dd2806", 0, "bd27513c-0178-47a9-af72-6709fabf5af0", new DateTime(2025, 11, 21, 15, 59, 39, 757, DateTimeKind.Utc).AddTicks(4230), "admin@car.com", true, "System Admin", false, null, "ADMIN@CAR.COM", "ADMIN@CAR.COM", "AQAAAAIAAYagAAAAEA9yX+BTqF2Iw3ASUPew2M3/0LO1rSTY4NfNNmgsLuiBvk0+y6C8P9iHD2OJboEPIQ==", null, false, "fcb50e51-d115-46a2-bccc-bc2cd5fdd14b", false, "admin@car.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "14b915c7-5ede-42c8-aa61-d4d3bbdfe411", "01f21577-b21e-4dce-bfc9-386eb0dd2806" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1eb1703e-8166-4253-8a5b-db382308ff18");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "14b915c7-5ede-42c8-aa61-d4d3bbdfe411", "01f21577-b21e-4dce-bfc9-386eb0dd2806" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14b915c7-5ede-42c8-aa61-d4d3bbdfe411");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "01f21577-b21e-4dce-bfc9-386eb0dd2806");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cars");

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
        }
    }
}
