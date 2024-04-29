using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedicalCenter.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedTimeSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#pragma warning disable CA1861 // Avoid constant arrays as arguments
            migrationBuilder.InsertData(
                table: "TimeSlots",
                columns: new[] { "Id", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeOnly(9, 0, 0), new TimeOnly(8, 0, 0) },
                    { 2, new TimeOnly(10, 0, 0), new TimeOnly(9, 0, 0) },
                    { 3, new TimeOnly(11, 0, 0), new TimeOnly(10, 0, 0) },
                    { 4, new TimeOnly(12, 0, 0), new TimeOnly(11, 0, 0) },
                    { 5, new TimeOnly(13, 0, 0), new TimeOnly(12, 0, 0) },
                    { 6, new TimeOnly(14, 0, 0), new TimeOnly(13, 0, 0) },
                    { 7, new TimeOnly(15, 0, 0), new TimeOnly(14, 0, 0) },
                    { 8, new TimeOnly(16, 0, 0), new TimeOnly(15, 0, 0) }
                });
#pragma warning restore CA1861 // Avoid constant arrays as arguments
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TimeSlots",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
