using Domain.Entities.Security.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security.Mapping
{
    public class DepartmentMap : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);
            builder.Property(d => d.Area)
                .IsRequired();
            builder.Property(d => d.Province)
                .IsRequired();
            builder.Property(d=>d.County)
                .IsRequired();
            builder.Property(d=>d.District)
                .IsRequired();
            builder.Property(d=>d.IsActived)
                .IsRequired();
            builder.Property(d=>d.RegisterDate)
                .IsRequired();
            builder.HasOne(d => d.User)
                .WithOne(d => d.Department)
                .HasForeignKey<Department>(d => d.UserId);
        }
    }
}
