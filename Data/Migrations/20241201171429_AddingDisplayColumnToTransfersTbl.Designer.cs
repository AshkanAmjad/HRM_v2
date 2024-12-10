﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(HRMContext))]
    [Migration("20241201171429_AddingDisplayColumnToTransfersTbl")]
    partial class AddingDisplayColumnToTransfersTbl
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Portal.Models.DepartmentTransfer", b =>
                {
                    b.Property<Guid>("DepartmentTransferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<Guid>("ReceiverDepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TransferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UploaderDepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DepartmentTransferId");

                    b.HasIndex("ReceiverDepartmentId");

                    b.HasIndex("TransferId");

                    b.HasIndex("UploaderDepartmentId");

                    b.ToTable("DepartmentTransfers", "Portal");
                });

            modelBuilder.Entity("Domain.Entities.Portal.Models.Document", b =>
                {
                    b.Property<Guid>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("DataBytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DocumentId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Documents", "Portal");
                });

            modelBuilder.Entity("Domain.Entities.Portal.Models.Transfer", b =>
                {
                    b.Property<Guid>("TransferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("DataBytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Display")
                        .HasColumnType("bit");

                    b.Property<string>("FileFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TransferId");

                    b.ToTable("Transfers", "Portal");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Department", b =>
                {
                    b.Property<Guid>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DepartmentId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Departments", "Security");

                    b.HasData(
                        new
                        {
                            DepartmentId = new Guid("401630fe-8f5a-4836-9718-922a6b5779db"),
                            Area = "0",
                            County = "0",
                            District = "0",
                            IsActived = true,
                            Province = "1",
                            RegisterDate = new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1183),
                            UserId = new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba")
                        });
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles", "Security");

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("5f3bc5d4-71b2-4315-82f5-9181de3d6a80"),
                            IsActived = true,
                            RegisterDate = new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1224),
                            Title = "مدیریت"
                        },
                        new
                        {
                            RoleId = new Guid("752233cf-57eb-4196-8568-c10dddd36af0"),
                            IsActived = true,
                            RegisterDate = new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1233),
                            Title = "فناوری اطلاعات"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Education")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Insurance")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastActived")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaritalStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("UserId");

                    b.ToTable("Users", "Security");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba"),
                            Address = "فردیس، کانال غربی",
                            City = "کرج",
                            DateOfBirth = "۱377/۰2/20",
                            Education = "B",
                            Email = "amjad.ashkan@gmial.com",
                            Employment = "O",
                            FirstName = "اشکان",
                            Gender = "M",
                            Insurance = true,
                            IsActived = true,
                            LastActived = new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1069),
                            LastName = "مطهری امجد",
                            MaritalStatus = "S",
                            Password = "5azbtevuaoQ26HISs22qug==;gMy6szlOD6Tahllohab+FUajHmWdeBeYxA0FJQvv3jo=",
                            PhoneNumber = "09351225600",
                            RegisterDate = new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1083),
                            UserName = "0021047022"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.UserRole", b =>
                {
                    b.Property<Guid>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles", "Security");

                    b.HasData(
                        new
                        {
                            UserRoleId = new Guid("61e2c6da-0a8a-43c1-ad3b-80ba1b04ba1e"),
                            IsActived = true,
                            RegisterDate = new DateTime(2024, 12, 1, 20, 44, 28, 687, DateTimeKind.Local).AddTicks(1204),
                            RoleId = new Guid("5f3bc5d4-71b2-4315-82f5-9181de3d6a80"),
                            UserId = new Guid("a02ccde1-2f95-46ec-a419-dcc2e7ce43ba")
                        });
                });

            modelBuilder.Entity("Domain.Entities.Portal.Models.DepartmentTransfer", b =>
                {
                    b.HasOne("Domain.Entities.Security.Models.Department", "ReceiverDepartment")
                        .WithMany("ReceiverTransfers")
                        .HasForeignKey("ReceiverDepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Portal.Models.Transfer", "Transfer")
                        .WithMany("DepartmentTransfers")
                        .HasForeignKey("TransferId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Security.Models.Department", "UploaderDepartment")
                        .WithMany("UploaderTransfers")
                        .HasForeignKey("UploaderDepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ReceiverDepartment");

                    b.Navigation("Transfer");

                    b.Navigation("UploaderDepartment");
                });

            modelBuilder.Entity("Domain.Entities.Portal.Models.Document", b =>
                {
                    b.HasOne("Domain.Entities.Security.Models.Department", "Department")
                        .WithMany("Documents")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Department", b =>
                {
                    b.HasOne("Domain.Entities.Security.Models.User", "User")
                        .WithOne("Department")
                        .HasForeignKey("Domain.Entities.Security.Models.Department", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.UserRole", b =>
                {
                    b.HasOne("Domain.Entities.Security.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Security.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Portal.Models.Transfer", b =>
                {
                    b.Navigation("DepartmentTransfers");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Department", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("ReceiverTransfers");

                    b.Navigation("UploaderTransfers");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.User", b =>
                {
                    b.Navigation("Department")
                        .IsRequired();

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}