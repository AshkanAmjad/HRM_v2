using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstUpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                schema: "Security",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                schema: "Security",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                schema: "Portal",
                table: "DepartmentTransfers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActived",
                schema: "Security",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsActived",
                schema: "Security",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsActived",
                schema: "Portal",
                table: "DepartmentTransfers");
        }
    }
}
