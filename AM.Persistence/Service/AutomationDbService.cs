using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AM.Persistence
{
    public class AutomationDbService : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public AutomationDbService(DbContextOptions<AutomationDbService> option, ILoggerFactory loggerFactory) : base(option)
        {
            _loggerFactory = loggerFactory;
        }

        public DbSet<Department> Department { get; set; }
        public DbSet<Plugin> Plugin { get; set; }
        public DbSet<PluginLog> PluginLog { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Module> Module { get; set; }
        public DbSet<UserModulePrivilege> UserModulePrivilege { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure DbContextOptionsBuilder with the provided logger factory
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AutomationDbService).Assembly);
        }
    }
}

