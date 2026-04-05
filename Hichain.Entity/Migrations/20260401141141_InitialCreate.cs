using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hichain.Entity.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "IsDeleted", "Name", "Price", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "电子产品", new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(2483), "高性能办公笔记本", false, "笔记本电脑", 5999.00m, 100, new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(2484) },
                    { 2, "电子产品", new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(2492), "人体工学设计", false, "无线鼠标", 99.00m, 500, new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(2493) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "CreatedAt", "Email", "IsDeleted", "Name", "Password", "Role", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, 30, new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(1704), "admin@example.com", false, "管理员", "admin123", "Admin", new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(1706), "admin" },
                    { 2, 25, new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(1716), "user@example.com", false, "普通用户", "user123", "User", new DateTime(2026, 4, 1, 14, 11, 39, 992, DateTimeKind.Utc).AddTicks(1718), "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
