using Microsoft.EntityFrameworkCore;
using RappidPayTest.Domain.Entities;
using RappidPayTest.Infrastructure.Data.Configuration;

namespace RappidPayTest.Infrastructure.Data
{
    public partial class PrincipalContext : DbContext
    {
        public PrincipalContext()
        {
        }

        public PrincipalContext(string CnString) : base(GetOptions(CnString))
        {
        }

        public static DbContextOptions GetOptions(string CnString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), CnString).Options;
        }

        public PrincipalContext(DbContextOptions<PrincipalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Prioridades> Prioridades { get; set; }
        public virtual DbSet<CardManagement> CardManagement { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelsConfiguration.Configuration(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}