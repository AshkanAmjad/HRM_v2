using Domain.Entities.Portal.Mapping;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Mapping;
using Domain.Entities.Security.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class HRMContext:DbContext
    {
        #region Constructor
        public HRMContext(DbContextOptions<HRMContext>options):base(options) { }
        #endregion

        #region Tables
        public DbSet <User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }  
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Transfer> Transfers { get; set; }  
        public DbSet<DepartmentTransfer> DepartmentTransfers { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Tables
            modelBuilder.ApplyConfiguration(new DepartmentTransferMap());
            modelBuilder.ApplyConfiguration(new DocumentMap());
            modelBuilder.ApplyConfiguration(new TransferMap());
            modelBuilder.ApplyConfiguration(new DepartmentMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            #endregion

            #region Filtering
            modelBuilder.Entity<User>()
                        .HasQueryFilter(u => u.IsActived);
            modelBuilder.Entity<Department>()
                        .HasQueryFilter(d => d.IsActived);
            modelBuilder.Entity<Document>()
                        .HasQueryFilter(d => d.IsActived);
            modelBuilder.Entity<Transfer>()
                        .HasQueryFilter(t => t.IsActived);
            modelBuilder.Entity<UserRole>()
                        .HasQueryFilter(ur => ur.IsActived);
            modelBuilder.Entity<DepartmentTransfer>()
                        .HasQueryFilter(dt => dt.IsActived);
            modelBuilder.Entity<Role>()
                        .HasQueryFilter(r => r.IsActived);
            #endregion

            #region Seed data
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(new User()
            {
                UserId = userId,
                UserName = "0021047022",
                Password = "5azbtevuaoQ26HISs22qug==;gMy6szlOD6Tahllohab+FUajHmWdeBeYxA0FJQvv3jo=",
                FirstName = "اشکان",
                LastName = "مطهری امجد",
                Gender ="M",
                Education = "B",
                Employment = "O",
                MaritalStatus = "S",
                Insurance = true,
                PhoneNumber = "09351225600",
                Email = "amjad.ashkan@gmial.com",
                DateOfBirth = "۱377/۰2/20",
                City = "کرج",
                IsActived = true,
                Address = "فردیس، کانال غربی",
                LastActived = DateTime.Now,
                RegisterDate = DateTime.Now
            });

            modelBuilder.Entity<Department>().HasData(new Department()
            {
                DepartmentId = Guid.NewGuid(),
                UserId = userId,
                Area = "0",
                Province = "1",
                County = "0",
                District = "0",
                IsActived = true,
                RegisterDate = DateTime.Now,
            });

            modelBuilder.Entity<UserRole>().HasData(new UserRole()
            {
                UserRoleId = Guid.NewGuid(),
                UserId = userId,
                RoleId = roleId,
                IsActived= true,
                RegisterDate= DateTime.Now
            });

            modelBuilder.Entity<Role>().HasData(new Role()
            {
                RoleId = roleId,
                Title = "مدیریت",
                RegisterDate = DateTime.Now,
                IsActived = true
            },
            new Role()
            {
                RoleId = Guid.NewGuid(),
                Title = "فناوری اطلاعات",
                RegisterDate = DateTime.Now,
                IsActived = true
            });
            #endregion

            base.OnModelCreating(modelBuilder);

        }
        #endregion
    }
}
