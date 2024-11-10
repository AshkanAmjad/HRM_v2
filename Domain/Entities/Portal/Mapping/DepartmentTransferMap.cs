using Domain.Entities.Portal.Models;
using Domain.Entities.Security.Models;
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
    public class DepartmentTransferMap : IEntityTypeConfiguration<DepartmentTransfer>
    {
        public void Configure(EntityTypeBuilder<DepartmentTransfer> builder)
        {
            builder.HasKey(dt => dt.DepartmentTransferId);

            builder.HasOne(dt => dt.UploaderDepartment)
                            .WithMany(dt => dt.UploaderTransfers)
                            .HasForeignKey(dt => dt.UploaderDepartmentId)
                            .OnDelete(DeleteBehavior.Restrict); ;
            builder.HasOne(dt => dt.ReceiverDepartment)
                             .WithMany(dt => dt.ReceiverTransfers)
                             .HasForeignKey(dt => dt.ReceiverDepartmentId)
                             .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(dt => dt.Transfer)
                            .WithMany(dt => dt.DepartmentTransfers)
                            .HasForeignKey(dt => dt.TransferId)
                            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(dt => dt.IsActived)
                   .IsRequired();
        }
    }
}
