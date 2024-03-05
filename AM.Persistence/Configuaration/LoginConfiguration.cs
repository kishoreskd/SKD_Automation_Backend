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
    public class LoginConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.UserName).IsRequired();
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.Token).HasMaxLength(500);
            builder.Property(e => e.RefreshToken).HasMaxLength(500);
            builder.Property(e => e.Email).HasMaxLength(100);

        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.RoleId);
            builder.Property(e => e.RoleName).HasMaxLength(200);
        }
    }
}
