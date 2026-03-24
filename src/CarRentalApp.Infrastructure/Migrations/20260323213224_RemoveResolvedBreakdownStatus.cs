using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResolvedBreakdownStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKkwlqoIimbVAAhZdYuIC0oNoo/lQ2jPOX+m4rA7GhHkpN4p3euhBq9PD+bWAsqiGw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAECFeEMKgUnK08wdR0J9E3Q5iwRDJL1dNVAl10LTImrrEAbkRDJK9oCokPa4CD4X43Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEM3+PNIdGF9HsldMW0fMQySkDkLWQdyeaC5LUJ0rYPuo468hEfs7WGlcA6wXkTEcAA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIE5LqAj3SLC1tdaYfLBvgB6lh3zZIU7C/3B3ndIkRD6dOY6cNza9+oVp7hekfORFw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKiJEpWK5r9sa0fsw/3OAuQh2UspDfkKSQw+VBCWEZ7U1WRZZsxcQlLXL5AQAwbbsA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHx8V6M1CH4H8htSe+9VyyulHpnMzwT+uUPjyq6uL2U/r8/cULL4AHX+VxVRPS34yQ==");
        }
    }
}
