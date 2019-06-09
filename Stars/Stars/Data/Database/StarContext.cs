using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stars.Data.Database
{
    public class StarContext : DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<StarCountTable> StarCounts { get; set; }

        public StarContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _config["SqlServer:ConnectionString"];
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
