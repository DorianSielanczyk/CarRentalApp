using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentitySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin-id",
                column: "ConcurrencyStamp",
                value: "role-admin-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-user-id",
                column: "ConcurrencyStamp",
                value: "role-user-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-worker-id",
                column: "ConcurrencyStamp",
                value: "role-worker-stamp");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-admin-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "user-admin-concurrency", "AQAAAAIAAYagAAAAEMzCjhmpGDQr0Vx/f/O/+uB8OSBNrdb0PGKxwkEiV8uVK+OTweOMOXKnooNphD1T9g==", "user-admin-stamp" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-customer-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "user-customer-concurrency", "AQAAAAIAAYagAAAAELiLL05oLDg3zUwJkv9/vLCdWZGpqmQVnIO8tRzPYBRHYUgfcw362t1rWg9ESxxHkA==", "user-customer-stamp" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user-worker-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "user-worker-concurrency", "AQAAAAIAAYagAAAAEFwugOwC2R3FFob4DDqQeNHTu8QrNamIX53Rg7fiaztDNxQ033muVoTfokmgkTb9cA==", "user-worker-stamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-admin-id",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-user-id",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "role-worker-id",
                column: "ConcurrencyStamp",
                value: null);

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
    }
}
