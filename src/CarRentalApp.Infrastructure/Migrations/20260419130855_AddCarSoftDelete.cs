using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "BreakdownReports",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "BreakdownReports",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPE4LjcvrIVhLLyGHKlFIw5TI0UpbSlzP2jSd+RRmFBLRW7jLNdtiPNYkwjG5IK8AA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAENhOMViitkVW2CmHakjaOiyo7zUjXhyBlv6h3xhNzcXEmOTaVXSnHy+34V1f08AagQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEK1Z1Hb4W2v8/urP8uaTjCM/86yBIaCjyGTtjsauWGbQe7lIxniS3n8goohr4YZeOg==");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsDeleted",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cars");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "BreakdownReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "BreakdownReports",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true);

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
    }
}
