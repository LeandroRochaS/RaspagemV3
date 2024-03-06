using Microsoft.EntityFrameworkCore;
using RaspagemV3.Models;
using System;

namespace RaspagemV3.Data
{
    public class LogContext : DbContext
    {
        public DbSet<LogModel> LOGROBO { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=SQL9001.site4now.net;" +

"Initial Catalog=db_aa5b20_apialmoxarifado;" +
"User id=db_aa5b20_apialmoxarifado_admin;" +
"Password=master@123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
