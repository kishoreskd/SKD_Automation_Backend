using AM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Persistence.Configuaration
{
    internal class UserPrivilegeConfiguaration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasMany(e => e.UserModulePrivileges)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("FK_User_UserModulePrivileges_UserId");
        }
    }

}
