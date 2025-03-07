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

    public virtual DbSet<galeri> galeris { get; set; }

    public virtual DbSet<haberler> haberlers { get; set; }

    public virtual DbSet<ikys_refresh_token> ikys_refresh_tokens { get; set; }

    public virtual DbSet<ikys_user> ikys_users { get; set; }

    public virtual DbSet<indirim_anlasmalari> indirim_anlasmalaris { get; set; }

    public virtual DbSet<sikca_sorulan_sorular> sikca_sorulan_sorulars { get; set; }

    public virtual DbSet<yemek_kategorileri> yemek_kategorileris { get; set; }

    public virtual DbSet<yemek_tarihleri> yemek_tarihleris { get; set; }

    public virtual DbSet<yemek_tarihleri_yemekler> yemek_tarihleri_yemeklers { get; set; }

    public virtual DbSet<yemekler> yemeklers { get; set; }

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

        modelBuilder.Entity<galeri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("galeri_pkey");

            entity.ToTable("galeri", "sbb_portal");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Fotograf).HasMaxLength(255);
            entity.Property(e => e.GaleriId).HasColumnType("character varying");
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
        });

        modelBuilder.Entity<haberler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("haberler_pkey");

            entity.ToTable("haberler", "sbb_portal");

            entity.Property(e => e.Baslik).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.GaleriId).HasColumnType("character varying");
            entity.Property(e => e.Kapak).HasMaxLength(255);
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
        });

        modelBuilder.Entity<ikys_refresh_token>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ikys_refresh_token", "sbb_portal");

            entity.Property(e => e.Created).HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.CreatedByIp).HasMaxLength(255);
            entity.Property(e => e.Expires).HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.ReasonRevoked).HasMaxLength(255);
            entity.Property(e => e.ReplacedByToken).HasMaxLength(255);
            entity.Property(e => e.Revoked).HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.RevokedByIp).HasMaxLength(50);
            entity.Property(e => e.Token).HasMaxLength(255);
        });

        modelBuilder.Entity<ikys_user>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ikys_user", "sbb_portal");

            entity.Property(e => e.DisplayName).HasColumnType("character varying");
            entity.Property(e => e.Fotograf).HasColumnType("character varying");
            entity.Property(e => e.Guid).HasMaxLength(150);
            entity.Property(e => e.Role).HasColumnType("character varying");
            entity.Property(e => e.UserName).HasColumnType("character varying");
            entity.Property(e => e.UserPassword).HasColumnType("character varying");
        });

        modelBuilder.Entity<indirim_anlasmalari>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("indirim_anlasmalari_pkey");

            entity.ToTable("indirim_anlasmalari", "sbb_portal");

            entity.Property(e => e.Baslik).HasMaxLength(150);
            entity.Property(e => e.Kategori).HasMaxLength(50);
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
        });

        modelBuilder.Entity<sikca_sorulan_sorular>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sikca_sorulan_sorular_pkey");

            entity.ToTable("sikca_sorulan_sorular", "sbb_portal");

            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
            entity.Property(e => e.SoruBaslik)
                .HasMaxLength(255)
                .HasDefaultValueSql("NULL::character varying");
        });

        modelBuilder.Entity<yemek_kategorileri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yemek_kategorileri_pkey");

            entity.ToTable("yemek_kategorileri", "sbb_portal");

            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.KategoriAdi).HasMaxLength(100);
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
        });

        modelBuilder.Entity<yemek_tarihleri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yemek_tarihleri_pkey1");

            entity.ToTable("yemek_tarihleri", "sbb_portal");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
            entity.Property(e => e.Tarih).HasColumnType("timestamp(6) without time zone");
        });

        modelBuilder.Entity<yemek_tarihleri_yemekler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yemek_tarihleri_yemekler_pkey1");

            entity.ToTable("yemek_tarihleri_yemekler", "sbb_portal");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);

            entity.HasOne(d => d.Tarih).WithMany(p => p.yemek_tarihleri_yemeklers)
                .HasForeignKey(d => d.TarihId)
                .HasConstraintName("yemek_tarihleri_yemekler_TarihId_fkey");

            entity.HasOne(d => d.Yemek).WithMany(p => p.yemek_tarihleri_yemeklers)
                .HasForeignKey(d => d.YemekId)
                .HasConstraintName("yemek_tarihleri_yemekler_YemekId_fkey");
        });

        modelBuilder.Entity<yemekler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("yemekler_pkey");

            entity.ToTable("yemekler", "sbb_portal");

            entity.Property(e => e.Aciklama).HasMaxLength(255);
            entity.Property(e => e.Fotograf).HasMaxLength(255);
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone");
            entity.Property(e => e.SilindiMi).HasDefaultValue(false);
            entity.Property(e => e.YemekAdi).HasMaxLength(100);

            entity.HasOne(d => d.Kategori).WithMany(p => p.yemeklers)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("yemekler_KategoriId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
