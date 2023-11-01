using Microsoft.EntityFrameworkCore;
using RapidPayTest.Domain.Entities;
using RapidPayTest.Infrastructure.Data.Configuration;

namespace RapidPayTest.Infrastructure.Data
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

        public virtual DbSet<CardManagement> CardManagement { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelsConfiguration.Configuration(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}