using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;


namespace AM.Persistence
{
    public class AutomationDbService : DbContext
    {
        public AutomationDbService()
        {

        }

        public AutomationDbService(DbContextOptions<AutomationDbService> option) : base(option)
        {
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Plugin> Plugin { get; set; }
        public DbSet<PluginLog> PluginLog { get; set; }

    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AutomationDbService).Assembly);
        }
    }
}

