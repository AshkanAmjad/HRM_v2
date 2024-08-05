using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbManyToManyRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentTransfers",
                schema: "Portal",
                table: "DepartmentTransfers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserRoleId",
                schema: "Security",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentTransferId",
                schema: "Portal",
                table: "DepartmentTransfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles",
                column: "UserRoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentTransfers",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "DepartmentTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "Security",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTransfers_TransferId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "TransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepartmentTransfers",
                schema: "Portal",
                table: "DepartmentTransfers");

            migrationBuilder.DropIndex(
                name: "IX_DepartmentTransfers_TransferId",
                schema: "Portal",
                table: "DepartmentTransfers");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "DepartmentTransferId",
                schema: "Portal",
                table: "DepartmentTransfers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepartmentTransfers",
                schema: "Portal",
                table: "DepartmentTransfers",
                columns: new[] { "TransferId", "DepartmentId" });
        }
    }
}
