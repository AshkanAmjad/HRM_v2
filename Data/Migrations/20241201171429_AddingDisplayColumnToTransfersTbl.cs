using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingDisplayColumnToTransfersTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("a04f006d-c80b-4a1a-b438-485ec2f1b807"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("8905869c-f95a-4331-9010-99f5ee0e9faf"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: new Guid("da777268-69b1-4887-9cb4-27f43a7288b8"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("549ab934-3ec0-4522-a2b5-b50d15850a58"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("35d3a8fd-687a-4002-a759-2a7a1a87b982"));

            migrationBuilder.AddColumn<bool>(
                name: "Display",
                schema: "Portal",
                table: "Transfers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Roles",
                columns: new[] { "RoleId", "IsActived", "RegisterDate", "Title" },
                values: new object[,]
                {
                    { new Guid("5f3bc5d4-71b2-4315-82f5-9181de3d6a80"), true, new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1224), "مدیریت" },
                    { new Guid("752233cf-57eb-4196-8568-c10dddd36af0"), true, new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1233), "فناوری اطلاعات" }
                });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Users",
                columns: new[] { "UserId", "Address", "City", "DateOfBirth", "Education", "Email", "Employment", "FirstName", "Gender", "Insurance", "IsActived", "LastActived", "LastName", "MaritalStatus", "Password", "PhoneNumber", "RegisterDate", "UserName" },
                values: new object[] { new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba"), "فردیس، کانال غربی", "کرج", "۱377/۰2/20", "B", "amjad.ashkan@gmial.com", "O", "اشکان", "M", true, true, new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1069), "مطهری امجد", "S", "5azbtevuaoQ26HISs22qug==;gMy6szlOD6Tahllohab+FUajHmWdeBeYxA0FJQvv3jo=", "09351225600", new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1083), "0021047022" });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "Departments",
                columns: new[] { "DepartmentId", "Area", "County", "District", "IsActived", "Province", "RegisterDate", "UserId" },
                values: new object[] { new Guid("401630fe-8f5a-4836-9718-922a6b5779db"), "0", "0", "0", true, "1", new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1183), new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba") });

            migrationBuilder.InsertData(
                schema: "Security",
                table: "UserRoles",
                columns: new[] { "UserRoleId", "IsActived", "RegisterDate", "RoleId", "UserId" },
                values: new object[] { new Guid("61e2c6da-0a8a-43c1-ad3b-80ba1b04ba1e"), true, new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1204), new Guid("5f3bc5d4-71b2-4315-82f5-9181de3d6a80"), new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: new Guid("401630fe-8f5a-4836-9718-922a6b5779db"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("752233cf-57eb-4196-8568-c10dddd36af0"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: new Guid("61e2c6da-0a8a-43c1-ad3b-80ba1b04ba1e"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("5f3bc5d4-71b2-4315-82f5-9181de3d6a80"));

            migrationBuilder.DeleteData(
                schema: "Security",
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba"));

            migrationBuilder.DropColumn(
                name: "Display",
                schema: "Portal",
                table: "Transfers");

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
        }
    }
}
