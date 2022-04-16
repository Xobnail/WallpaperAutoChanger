using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WallpaperAutoChanger
{
    public partial class wallpapersdbContext : DbContext
    {
        public wallpapersdbContext()
        {
        }

        public wallpapersdbContext(DbContextOptions<wallpapersdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Wallpaper> Wallpapers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost:5432;Database=wallpapersdb;Username=xobnail;Password=0000");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallpaper>(entity =>
            {
                entity.ToTable("wallpaper");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Url)
                    .HasMaxLength(255)
                    .HasColumnName("url");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
