using Microsoft.EntityFrameworkCore;
using RapidPayTest.Domain.Entities;

namespace RapidPayTest.Infrastructure.Data.Configuration
{
    public class ModelsConfiguration
    {

        public static void Configuration(ModelBuilder modelBuilder)
        {

            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CardManagement>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("CardManagement");

                //entity.Property(e => e.Prioridad)
                //    .IsRequired()
                //    .HasMaxLength(150)
                //    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("Transaction");

                //entity.Property(e => e.Prioridad)
                //    .IsRequired()
                //    .HasMaxLength(150)
                //    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("User");

                entity.Property(e => e.RoleID)
                    .IsRequired().HasColumnName("RoleID");
            });
        }

    }
}
