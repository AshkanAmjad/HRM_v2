using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstUpdatingTransferModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("2c091b97-d244-48f7-bef6-bcae4bed7b3a"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("b8094aff-e521-4188-869c-7a8399ae3deb"));

            migrationBuilder.DropColumn(
                name: "ReceiverRole",
                schema: "Portal",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                schema: "Portal",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "UserUploader",
                schema: "Portal",
                table: "Transfers",
                newName: "UserIdUploader");

            migrationBuilder.RenameColumn(
                name: "RoleUploader",
                schema: "Portal",
                table: "Transfers",
                newName: "UserIdReceiver");

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("b0c7b1dc-feb5-4bcd-a343-83824b3d1909"), true, new DateTime(2024, 10, 29, 16, 14, 8, 297, DateTimeKind.Local).AddTicks(709), "مدیریت" },
                    { new Guid("ca1d8818-d2b1-447a-a551-e360d2aaf12c"), true, new DateTime(2024, 10, 29, 16, 14, 8, 297, DateTimeKind.Local).AddTicks(727), "فناوری اطلاعات" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("b0c7b1dc-feb5-4bcd-a343-83824b3d1909"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("ca1d8818-d2b1-447a-a551-e360d2aaf12c"));

            migrationBuilder.RenameColumn(
                name: "UserIdUploader",
                schema: "Portal",
                table: "Transfers",
                newName: "UserUploader");

            migrationBuilder.RenameColumn(
                name: "UserIdReceiver",
                schema: "Portal",
                table: "Transfers",
                newName: "RoleUploader");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverRole",
                schema: "Portal",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverUserId",
                schema: "Portal",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("2c091b97-d244-48f7-bef6-bcae4bed7b3a"), true, new DateTime(2024, 10, 6, 11, 14, 8, 608, DateTimeKind.Local).AddTicks(3800), "فناوری اطلاعات" },
                    { new Guid("b8094aff-e521-4188-869c-7a8399ae3deb"), true, new DateTime(2024, 10, 6, 11, 14, 8, 608, DateTimeKind.Local).AddTicks(3781), "مدیریت" }
                });
        }
    }
}
