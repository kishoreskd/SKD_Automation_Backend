using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AM.Domain.Entities;

namespace RFIM.Persistence.Configuaration
{
    public class LoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EmployeeId).IsRequired();
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.Token).HasMaxLength(500);
            builder.Property(e => e.RefreshToken).HasMaxLength(500);
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.RoleName).HasMaxLength(200);
        }
    }
}
