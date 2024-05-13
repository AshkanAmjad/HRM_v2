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
    public class DocumentMap : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(d => d.DocumentId);
            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(d => d.UploadDate)
                .IsRequired();
            builder.Property(d => d.IsActived)
                .IsRequired();
            builder.HasOne(d => d.Department)
                .WithMany(d => d.Documents)
                .HasForeignKey(d => d.DepartmentId);
        }
    }
}
