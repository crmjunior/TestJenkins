using CAContext.Infra.Data.EntityConfig;
using CAContext.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CAContext.Infra.Data.Context
{
    public class ContribuicoesContext : DbContext
    {
        private readonly IHostEnvironment _env;

        public ContribuicoesContext(IHostEnvironment env)
        {
            _env = env;
        }

        public DbSet<Contribuicao> Contribuicoes { get; set; }

        public DbSet<ContribuicaoArquivo> ContribuicoesArquivo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContribuicaoConfig());
            modelBuilder.ApplyConfiguration(new ContribuicaoArquivoConfig());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
            
            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("DesenvConnection"));
        }
    }
}