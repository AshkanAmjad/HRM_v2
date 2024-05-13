using Domain.Entities.Security.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Security.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ru => new { ru.UserId, ru.RoleId });
            builder.HasOne(u => u.User)
                .WithMany(ru => ru.UserRoles)
                .HasForeignKey(u => u.UserId);
            builder.HasOne(r => r.Role)
                .WithMany(ru => ru.UserRoles)
                .HasForeignKey(r => r.RoleId);
        }
    }
}
