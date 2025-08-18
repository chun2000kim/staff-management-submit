using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StaffManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataTotable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffId", "Birthday", "FullName", "Gender" },
                values: new object[,]
                {
                    { "SF00001", new DateOnly(2000, 1, 1), "Staff 1", 1 },
                    { "SF00002", new DateOnly(1995, 5, 30), "Staff 2", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: "SF00001");

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffId",
                keyValue: "SF00002");
        }
    }
}
