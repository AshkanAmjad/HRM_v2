using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ThirdUpdatingTranfersTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("90ed8cb5-2141-4509-a898-5b12ad1303d1"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("ab15fa1c-8869-42f5-b77c-ac53dc975f55"));

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "Portal",
                table: "Transfers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("028ca6e5-dc41-45d5-bad9-4859c1b59b52"), true, new DateTime(2024, 10, 29, 18, 24, 25, 332, DateTimeKind.Local).AddTicks(4562), "مدیریت" },
                    { new Guid("8c337d7b-8d7f-40a1-93f5-4e62af650c50"), true, new DateTime(2024, 10, 29, 18, 24, 25, 332, DateTimeKind.Local).AddTicks(4578), "فناوری اطلاعات" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("028ca6e5-dc41-45d5-bad9-4859c1b59b52"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("8c337d7b-8d7f-40a1-93f5-4e62af650c50"));

            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "Portal",
                table: "Transfers");

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("90ed8cb5-2141-4509-a898-5b12ad1303d1"), true, new DateTime(2024, 10, 29, 16, 40, 59, 728, DateTimeKind.Local).AddTicks(5977), "مدیریت" },
                    { new Guid("ab15fa1c-8869-42f5-b77c-ac53dc975f55"), true, new DateTime(2024, 10, 29, 16, 40, 59, 728, DateTimeKind.Local).AddTicks(5991), "فناوری اطلاعات" }
                });
        }
    }
}
