using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllEntitiesWithRolesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photos",
                table: "Cars");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0dec909-37b8-4af6-ae4b-536225cf501b", "AQAAAAIAAYagAAAAEFSBzWF6JqaL96jkP3j2I0hOPlnED8peMihyYYJTu+zPmb992qufF8ZZuj39Q4f3dg==", "f7f36521-4238-4e5f-a1f2-9e80dc03526a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "99f136fd-471e-4680-a213-312d429a5a8c", "AQAAAAIAAYagAAAAEMuZB6LOnyNbjX0RsfzbsqhGC7kfFcGHZ98qGzytb30y9LSfmacjqEjDwaH4Be3a0Q==", "778b368a-f0be-4a3b-9658-e8c904b65790" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d843e46c-a1ef-4132-acf0-8613c219cb8c", "AQAAAAIAAYagAAAAEGrXp7/+4ikjSIux2nB5uE5SSfXwVVxbTUg0t1CMoVasznaFobcE9EF+fN0HvyO4BA==", "2b16075d-9530-44e3-934f-9570bf824b87" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photos",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0b3612e9-0f33-473c-bf40-21d00442b7cf", "AQAAAAIAAYagAAAAEJ4/KGlDFij5bwL36+8V8G5ETMX6bI3Ex981T/PX4ZYeEsRDZPUVJKYv3Po2gmafDg==", "83f7ff7e-c94a-4960-ac74-14840a2cf9ee" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a916fdc5-e739-41ce-831f-e86ebe35e201", "AQAAAAIAAYagAAAAED+8dTbjohNBgt2DtXhXX6xBiuHN99lEjpacu6R/PqVm+Xoyiwkrdc0KJ/gFZ7FAZQ==", "510445bd-023b-4d55-8199-e1344e421809" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0bdba5b4-f2b9-44be-9081-04570a44c8b6", "AQAAAAIAAYagAAAAEHSTqhN13RasqsKL4zbRfDzbtJHIIwv9NY+iY/82Kn3alIsd0wiIvLnqO39kB0q6TQ==", "74b4418f-aebf-480e-8a65-ce3241b24dc6" });

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 7,
                column: "Photos",
                value: "");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 8,
                column: "Photos",
                value: "");
        }
    }
}
