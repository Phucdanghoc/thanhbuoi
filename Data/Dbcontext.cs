﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Models;

namespace ThanhBuoi.Data
{
    public class DataContext : IdentityDbContext<TaiKhoan>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<LoaiXe>()
                   .HasIndex(u => u.Ten)
                   .IsUnique();
            modelBuilder.Entity<Diadiem>()
                    .HasIndex(u => u.Ten)
                    .IsUnique();
            modelBuilder.Entity<Tuyen>()
                    .HasIndex(u => u.Ten)
                    .IsUnique();
            modelBuilder.Entity<Xe>()
                    .HasIndex(u => u.BienSo)
                    .IsUnique();
            modelBuilder.Entity<Xe>()
                    .HasIndex(u => u.Ten)
                    .IsUnique();
            modelBuilder.Entity<Ve>().HasOne(Ve => Ve.Ghe).WithMany(Ghe => Ghe.Ves).HasForeignKey(Ve => Ve.Id).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Tuyen>()
                .HasOne(t => t.DiemDi)
                .WithMany(diadiem => diadiem.Tuyens)
                .HasForeignKey(t => t.Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tuyen>()
                .HasOne(t => t.Diemden)
                .WithMany()
                .HasForeignKey(t => t.Id)
                .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);

        }
        public DbSet<TaiKhoan> DbSet { get; set; }
        public DbSet<Tuyen> Tuyens { get; set; }
        public DbSet<Diadiem> Diadiems { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<GiaSukien> GiaTuyens { get; set; }
        public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }
        public DbSet<HangGui> HangGuis { get; set; }
        public DbSet<Chuyen> Chuyens { get; set; }
        public DbSet<Ve> Ves { get; set; }
        public DbSet<Ghe> Ghes { get; set; }
        public DbSet<Hang> Hangs { get; set; }
        public DbSet<Tang> Tangs { get; set; }
        public DbSet<SoDoGhe> SoDoGhes { get; set; }
        public DbSet<Xe> Xes { get; set; }
        public DbSet<TinTuc> TinTucs { get; set; }
        public DbSet<LoaiXe> loaiXes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
       

    }
}
