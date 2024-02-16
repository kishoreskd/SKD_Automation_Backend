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
