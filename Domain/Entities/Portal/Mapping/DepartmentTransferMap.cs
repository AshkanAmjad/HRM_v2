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
            builder.HasKey(dt => new { dt.TransferId, dt.DepartmentId });
            builder.HasOne(d => d.Department)
                            .WithMany(dt => dt.DepartmentTransfers)
                            .HasForeignKey(d => d.DepartmentId);
            builder.HasOne(t => t.Transfer)
                            .WithMany(dt => dt.DepartmentTransfers)
                            .HasForeignKey(t => t.TransferId);
        }
    }
}
