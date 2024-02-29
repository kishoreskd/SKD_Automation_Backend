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
    public class PluginConfiguaration : IEntityTypeConfiguration<Plugin>
    {
        public void Configure(EntityTypeBuilder<Plugin> builder)
        {
            builder.HasKey(e => e.PluginId);

            builder.Property(e => e.PluginName).HasMaxLength(200).IsRequired();

            builder.Property(e => e.Description).HasMaxLength(1000).IsRequired();

            builder.Property(e => e.ManualMinutes).IsRequired();

            builder.Property(e => e.AutomatedMinutes).IsRequired();

            builder.Property(e => e.Description).IsRequired();

            builder.Property(e => e.PluginKey).HasMaxLength(1000).IsRequired();



            builder.HasMany(e => e.PluginLogCol)
                .WithOne(e => e.Plugin)
                .HasForeignKey(e => e.PluginId)
                .HasConstraintName("FK_Plugin_PluginLogCol_PluginId");
        }
    }
}
