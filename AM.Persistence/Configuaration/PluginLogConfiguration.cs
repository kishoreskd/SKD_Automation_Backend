    using AM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RFIM.Persistence.Configuaration
{
    public class PluginLogConfiguaration : IEntityTypeConfiguration<PluginLog>
    {
        public void Configure(EntityTypeBuilder<PluginLog> builder)
        {
            builder.HasKey(e => e.PluginLogId);

            builder.Property(e => e.JobName).HasMaxLength(200).IsRequired();

            builder.Property(e => e.Activity).HasMaxLength(1000).IsRequired();
        }
    }
}
