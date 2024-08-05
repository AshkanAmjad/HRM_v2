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
    [Migration("20240805181342_DeleteKeyAttributes")]
    partial class DeleteKeyAttributes
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

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TransferId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DepartmentTransferId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("TransferId");

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

                    b.Property<string>("FileFormat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<Guid>("ReceiverRole")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ReceiverUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleUploader")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserUploader")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TransferId");

                    b.ToTable("Transfers", "Portal");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Department", b =>
                {
                    b.Property<Guid>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("County")
                        .HasColumnType("int");

                    b.Property<int>("District")
                        .HasColumnType("int");

                    b.Property<bool>("IsActived")
                        .HasColumnType("bit");

                    b.Property<int>("Province")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("DepartmentId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Departments", "Security");
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.Role", b =>
                {
                    b.Property<Guid>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles", "Security");
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
                });

            modelBuilder.Entity("Domain.Entities.Security.Models.UserRole", b =>
                {
                    b.Property<Guid>("UserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserRoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles", "Security");
                });

            modelBuilder.Entity("Domain.Entities.Portal.Models.DepartmentTransfer", b =>
                {
                    b.HasOne("Domain.Entities.Security.Models.Department", "Department")
                        .WithMany("DepartmentTransfers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Portal.Models.Transfer", "Transfer")
                        .WithMany("DepartmentTransfers")
                        .HasForeignKey("TransferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Transfer");
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
                    b.Navigation("DepartmentTransfers");

                    b.Navigation("Documents");
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
