using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
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
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    UploaderDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTransfers", x => x.DepartmentTransferId);
                    table.ForeignKey(
                        name: "FK_DepartmentTransfers_Departments_ReceiverDepartmentId",
                        column: x => x.ReceiverDepartmentId,
                        principalSchema: "Security",
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentTransfers_Departments_UploaderDepartmentId",
                        column: x => x.UploaderDepartmentId,
                        principalSchema: "Security",
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentTransfers_Transfers_TransferId",
                        column: x => x.TransferId,
                        principalSchema: "Portal",
                        principalTable: "Transfers",
                        principalColumn: "TransferId",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("549ab934-3ec0-4522-a2b5-b50d15850a58"), true, new DateTime(2024, 11, 19, 17, 20, 8, 612, DateTimeKind.Local).AddTicks(8618), "مدیریت" },
                    { new Guid("8905869c-f95a-4331-9010-99f5ee0e9faf"), true, new DateTime(2024, 11, 19, 17, 20, 8, 612, DateTimeKind.Local).AddTicks(8621), "فناوری اطلاعات" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "UserId", "Address", "City", "DateOfBirth", "Education", "Email", "Employment", "FirstName", "Gender", "Insurance", "IsActived", "LastActived", "LastName", "MaritalStatus", "Password", "PhoneNumber", "RegisterDate", "UserName" },
                values: new object[] { new Guid("35d3a8fd-687a-4002-a759-2a7a1a87b982"), "فردیس، کانال غربی", "کرج", "۱377/۰2/20", "B", "amjad.ashkan@gmial.com", "O", "اشکان", "M", true, true, new DateTime(2024, 11, 19, 17, 20, 8, 612, DateTimeKind.Local).AddTicks(8458), "مطهری امجد", "S", "5azbtevuaoQ26HISs22qug==;gMy6szlOD6Tahllohab+FUajHmWdeBeYxA0FJQvv3jo=", "09351225600", new DateTime(2024, 11, 19, 17, 20, 8, 612, DateTimeKind.Local).AddTicks(8470), "0021047022" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Departments",
                columns: new[] { "DepartmentId", "Area", "County", "District", "IsActived", "Province", "RegisterDate", "UserId" },
                values: new object[] { new Guid("a04f006d-c80b-4a1a-b438-485ec2f1b807"), "0", "0", "0", true, "1", new DateTime(2024, 11, 19, 17, 20, 8, 612, DateTimeKind.Local).AddTicks(8586), new Guid("35d3a8fd-687a-4002-a759-2a7a1a87b982") });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserRoleId", "IsActived", "RegisterDate", "RoleId", "UserId" },
                values: new object[] { new Guid("da777268-69b1-4887-9cb4-27f43a7288b8"), true, new DateTime(2024, 11, 19, 17, 20, 8, 612, DateTimeKind.Local).AddTicks(8602), new Guid("549ab934-3ec0-4522-a2b5-b50d15850a58"), new Guid("35d3a8fd-687a-4002-a759-2a7a1a87b982") });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UserId",
                schema: "Security",
                table: "Departments",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTransfers_ReceiverDepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "ReceiverDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTransfers_TransferId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTransfers_UploaderDepartmentId",
                schema: "Portal",
                table: "DepartmentTransfers",
                column: "UploaderDepartmentId");

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
