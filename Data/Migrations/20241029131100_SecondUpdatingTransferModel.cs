using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondUpdatingTransferModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentTransfers_Departments_DepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers");

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

            migrationBuilder.DropColumn(
                name: "UserIdReceiver",
                schema: "Portal",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "UserIdUploader",
                schema: "Portal",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers",
                newName: "DepartmentIdUploader");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentTransfers_DepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers",
                newName: "IX_DepartmentTransfers_DepartmentIdUploader");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentIdReceiver",
                schema: "Portal",
                table: "DepartmentTransfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("90ed8cb5-2141-4509-a898-5b12ad1303d1"), true, new DateTime(2024, 10, 29, 16, 40, 59, 728, DateTimeKind.Local).AddTicks(5977), "مدیریت" },
                    { new Guid("ab15fa1c-8869-42f5-b77c-ac53dc975f55"), true, new DateTime(2024, 10, 29, 16, 40, 59, 728, DateTimeKind.Local).AddTicks(5991), "فناوری اطلاعات" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentTransfers_Departments_DepartmentIdUploader",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "DepartmentIdUploader",
                principalSchema: "Security",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentTransfers_Departments_DepartmentIdUploader",
                schema: "Portal",
                table: "DepartmentTransfers");

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

            migrationBuilder.DropColumn(
                name: "DepartmentIdReceiver",
                schema: "Portal",
                table: "DepartmentTransfers");

            migrationBuilder.RenameColumn(
                name: "DepartmentIdUploader",
                schema: "Portal",
                table: "DepartmentTransfers",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_DepartmentTransfers_DepartmentIdUploader",
                schema: "Portal",
                table: "DepartmentTransfers",
                newName: "IX_DepartmentTransfers_DepartmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserIdReceiver",
                schema: "Portal",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserIdUploader",
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
                    { new Guid("b0c7b1dc-feb5-4bcd-a343-83824b3d1909"), true, new DateTime(2024, 10, 29, 16, 14, 8, 297, DateTimeKind.Local).AddTicks(709), "مدیریت" },
                    { new Guid("ca1d8818-d2b1-447a-a551-e360d2aaf12c"), true, new DateTime(2024, 10, 29, 16, 14, 8, 297, DateTimeKind.Local).AddTicks(727), "فناوری اطلاعات" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentTransfers_Departments_DepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "DepartmentId",
                principalSchema: "Security",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
