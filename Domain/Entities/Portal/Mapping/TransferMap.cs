using Domain.Entities.Portal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Portal.Mapping
{
    public class TransferMap : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.HasKey(t => t.TransferId);
            builder.Property(t => t.Title)
                            .IsRequired()
                            .HasMaxLength(100);
            builder.Property(t => t.UploadDate)
                .IsRequired();
            builder.Property(t => t.IsActived)
                .IsRequired();
            builder.Property(t => t.Display)
                .IsRequired();
        }
    }
}
