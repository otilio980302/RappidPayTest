using Microsoft.EntityFrameworkCore;
using RappidPayTest.Domain.Entities;

namespace RappidPayTest.Infrastructure.Data.Configuration
{
    public class ModelsConfiguration
    {

        public static void Configuration(ModelBuilder modelBuilder)
        {

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");


            modelBuilder.Entity<Prioridades>(entity =>
            {
                entity.HasKey(e => e.CodPrioridad);

                entity.ToTable("PRIORIDADES");

                entity.Property(e => e.Prioridad)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CardManagement>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("CardManagement");

                //entity.Property(e => e.Prioridad)
                //    .IsRequired()
                //    .HasMaxLength(150)
                //    .IsUnicode(false);
            });

        }

    }
}
