using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RentalPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalPhotos_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RentalPhotos_RentalId",
                table: "RentalPhotos",
                column: "RentalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalPhotos");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMzCjhmpGDQr0Vx/f/O/+uB8OSBNrdb0PGKxwkEiV8uVK+OTweOMOXKnooNphD1T9g==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELiLL05oLDg3zUwJkv9/vLCdWZGpqmQVnIO8tRzPYBRHYUgfcw362t1rWg9ESxxHkA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFwugOwC2R3FFob4DDqQeNHTu8QrNamIX53Rg7fiaztDNxQ033muVoTfokmgkTb9cA==");
        }
    }
}
