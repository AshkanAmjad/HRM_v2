using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Portal");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Security",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                schema: "Portal",
                columns: table => new
                {
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserUploader = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleUploader = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverRole = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransferId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Security",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Employment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Insurance = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    LastActived = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Security",
                columns: table => new
                {
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_Departments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Security",
                columns: table => new
                {
                    UserRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTransfers",
                schema: "Portal",
                columns: table => new
                {
                    DepartmentTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTransfers", x => x.DepartmentTransferId);
                    table.ForeignKey(
                        name: "FK_DepartmentTransfers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Security",
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentTransfers_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalSchema: "Portal",
                        principalTable: "Transfers",
                        principalColumn: "TransferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "Portal",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Security",
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UserId",
                schema: "Security",
                table: "Departments",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTransfers_DepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTransfers_TransferId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DepartmentId",
                schema: "Portal",
                table: "Documents",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Security",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "Security",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentTransfers",
                schema: "Portal");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "Portal");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "Portal");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Security");
        }
    }
}
