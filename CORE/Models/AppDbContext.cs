using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CORE.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<sikca_sorulan_sorular> sikca_sorulan_sorulars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.55.142;Database=sbbdb;Username=portal_user;Password=!123456!;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("tr_TR.utf8")
            .HasPostgresExtension("ailesosyal", "pg_cron")
            .HasPostgresExtension("asterisk", "mysql_fdw")
            .HasPostgresExtension("fdw", "postgres_fdw")
            .HasPostgresExtension("ikys", "tablefunc")
            .HasPostgresExtension("file_fdw")
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("tds_fdw")
            .HasPostgresExtension("ukbs", "oracle_fdw");

        modelBuilder.Entity<sikca_sorulan_sorular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sikca_sorulan_sorular_pkey");

            entity.ToTable("sikca_sorulan_sorular", "sbb_portal");

            entity.Property(e => e.OlusturmaTarihi)
                .HasMaxLength(255)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.SoruBaslik)
                .HasMaxLength(255)
                .HasDefaultValueSql("NULL::character varying");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
