using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e7fa948-a324-48cf-bc0f-d24de73a36dd", null, "Admin", "ADMIN" },
                    { "c75688cd-d01b-46d1-80fa-19d3e0bce9b7", null, "User", "USER" },
                    { "e6fe486b-7d13-4207-950a-031760e4e5b0", null, "Editor", "EDITOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e7fa948-a324-48cf-bc0f-d24de73a36dd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c75688cd-d01b-46d1-80fa-19d3e0bce9b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6fe486b-7d13-4207-950a-031760e4e5b0");
        }
    }
}
