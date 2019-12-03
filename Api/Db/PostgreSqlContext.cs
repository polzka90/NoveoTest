using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Db
{
    public class PostgreSqlContext : DbContext
    {
        private readonly IConfigurationRoot _configurationFile;
        private readonly string _connectionString;

        public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) :
            base(options)
        {
            _configurationFile = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            _connectionString = _configurationFile
                                    .GetConnectionString("PostgreSqlNetCoreConnection");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<WorkflowDTO> WorkFlow { get; set; }

    }
}
