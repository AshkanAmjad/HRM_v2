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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(15);
            builder.Property(u => u.Password)
                .IsRequired();
            builder.Property(u => u.FirstName)
                .IsRequired();
            builder.Property(u => u.LastName)
                .IsRequired();
            builder.Property(u => u.Gender)
                .IsRequired();
            builder.Property(u => u.Employment)
                .IsRequired();
            builder.Property(u => u.Insurance)
                .IsRequired();
            builder.Property(u => u.PhoneNumber)
                .IsRequired();
            builder.Property(u => u.DateOfBirth)
                .IsRequired();
            builder.Property(u => u.City)
                .IsRequired();
            builder.Property(u => u.Address)
                .IsRequired();
            builder.Property(u => u.IsActived)
                .IsRequired();
            builder.Property(u => u.MaritalStatus)
                .IsRequired();
            builder.Property(u => u.RegisterDate)
                .IsRequired();
        }
    }
}
