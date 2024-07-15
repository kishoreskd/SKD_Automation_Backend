using AM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Persistence.Configuaration
{
    internal class UserModulePrivilegeConfiguaration : IEntityTypeConfiguration<UserModulePrivilege>
    {
        public void Configure(EntityTypeBuilder<UserModulePrivilege> builder)
        {
            builder.HasKey(e => e.UserModulePrivilegeId);

            builder.Property(e => e.UserId).IsRequired();
        }
    }

}
