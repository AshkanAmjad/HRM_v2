using Data.Extensions;
using Domain.Entities.Portal.Mapping;
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

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityAssembly = typeof(BaseEntity).Assembly;
            modelBuilder.Verify<BaseEntity>(entityAssembly);

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
