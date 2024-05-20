using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b269d45b-13e7-4ca9-abdb-661c0126b2b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf188404-129e-4aec-88f9-d9998ae804e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4cc4ce8-c8b6-4457-b153-8034bf17b258");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0978ecbb-b409-4088-9aaf-dfc4ca909774", null, "Admin", "ADMIN" },
                    { "88f8f360-02f6-4784-8498-eab9292f6862", null, "Editor", "EDITOR" },
                    { "dc7405c3-23ad-4aae-b0ce-ae3d85a5f5c6", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Computer Science" },
                    { 2, "Network" },
                    { 3, "Database Management Systems" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0978ecbb-b409-4088-9aaf-dfc4ca909774");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88f8f360-02f6-4784-8498-eab9292f6862");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc7405c3-23ad-4aae-b0ce-ae3d85a5f5c6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b269d45b-13e7-4ca9-abdb-661c0126b2b7", null, "Admin", "ADMIN" },
                    { "cf188404-129e-4aec-88f9-d9998ae804e7", null, "Editor", "EDITOR" },
                    { "d4cc4ce8-c8b6-4457-b153-8034bf17b258", null, "User", "USER" }
                });
        }
    }
}
