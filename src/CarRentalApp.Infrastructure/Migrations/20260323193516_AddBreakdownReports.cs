using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBreakdownReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreakdownReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    BreakdownType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakdownReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreakdownReports_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreakdownReportNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreakdownReportId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakdownReportNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreakdownReportNotes_BreakdownReports_BreakdownReportId",
                        column: x => x.BreakdownReportId,
                        principalTable: "BreakdownReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BreakdownReportPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreakdownReportId = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreakdownReportPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreakdownReportPhotos_BreakdownReports_BreakdownReportId",
                        column: x => x.BreakdownReportId,
                        principalTable: "BreakdownReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BreakdownReportNotes_BreakdownReportId",
                table: "BreakdownReportNotes",
                column: "BreakdownReportId");

            migrationBuilder.CreateIndex(
                name: "IX_BreakdownReportPhotos_BreakdownReportId",
                table: "BreakdownReportPhotos",
                column: "BreakdownReportId");

            migrationBuilder.CreateIndex(
                name: "IX_BreakdownReports_RentalId",
                table: "BreakdownReports",
                column: "RentalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreakdownReportNotes");

            migrationBuilder.DropTable(
                name: "BreakdownReportPhotos");

            migrationBuilder.DropTable(
                name: "BreakdownReports");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJDChaxZt36KXd3VRpE6inGJb+DqkMGqgxlKBgs20aGij2PXLJ2FJ8Gih7ePKlDsEQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPW1p4Uho2Pvj2nrnZzVZAnUUD4CN96chd7WIhw0wguyH7zxiYfeUPM6b/rgQVeolA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPefc6X/dfaknP4AnSWPB01BmVWOdu2I3qKpxqF84gM6/cSaQA1AOouHB9tu/wcD7A==");
        }
    }
}
