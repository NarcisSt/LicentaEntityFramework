using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LicentaEntityFrameworkConsole.Models
{
    public partial class PostgresContext : DbContext
    {
        public PostgresContext()
        {
        }

        public PostgresContext(DbContextOptions<PostgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Database=postgres;Username=postgres;Password=admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("players", "licenta");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.LastName).HasColumnName("last_name");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.ShirtNumber).HasColumnName("shirt_number");

                entity.Property(e => e.Team).HasColumnName("team");

                entity.HasOne(d => d.NameNavigation)
                    .WithMany(p => p.PlayerNameNavigations)
                    .HasForeignKey(d => d.Name)
                    .HasConstraintName("fkh02ms6ttnthuhtuy4qe6ahm14");

                entity.HasOne(d => d.TeamNavigation)
                    .WithMany(p => p.PlayerTeamNavigations)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.Team)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("team");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("teams", "licenta");

                entity.HasIndex(e => e.Name, "tea")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(null, null, null, null, true);

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Country).HasColumnName("country");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Points).HasColumnName("points");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
