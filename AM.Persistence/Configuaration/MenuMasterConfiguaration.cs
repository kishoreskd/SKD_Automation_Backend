using AM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AM.Persistence.Configuaration
{
    internal class ModuleConfiguaration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(e => e.ModuleId);

            builder.Property(e => e.ModuleName).IsRequired().HasMaxLength(500);

            builder.HasMany(e => e.UserModulePrivileges)
                .WithOne(e => e.Module)
                .HasForeignKey(e => e.ModuleId)
                .HasConstraintName("FK_Module_UserModulePrivileges_ModuleId");
        }
    }

}
