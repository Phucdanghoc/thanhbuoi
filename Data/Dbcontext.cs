﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;

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
/*            modelBuilder.Entity<Ve>().HasOne(Ve => Ve.Ghe).WithMany(Ghe => Ghe.Ves).HasForeignKey(Ve => Ve.Id).OnDelete(DeleteBehavior.Restrict);*/
            base.OnModelCreating(modelBuilder);

        }

        internal bool FindAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public DbSet<TaiKhoan> DbSet { get; set; }
        public DbSet<Tuyen> Tuyens { get; set; }
        public DbSet<Diadiem> Diadiems { get; set; }
        public DbSet<VeHuy> VeHuys { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
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
        public DbSet<LoaiXe> LoaiXes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<ThanhBuoi.Models.DTO.DTOHang> DTOHang { get; set; } = default!;
       

    }
}
