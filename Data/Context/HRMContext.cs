﻿using Domain.Entities.Portal.Mapping;
using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Mapping;
using Domain.Entities.Security.Models;
using Domain.Interfaces.Base;
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

            base.OnModelCreating(modelBuilder);

        }
        #endregion
    }
}
