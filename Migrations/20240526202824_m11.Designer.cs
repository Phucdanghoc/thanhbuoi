﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThanhBuoi.Data;

#nullable disable

namespace ThanhBuoi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240526202824_m11")]
    partial class m11
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ThanhBuoi.Models.Chuyen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DiemDon")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Gia")
                        .HasColumnType("float");

                    b.Property<double?>("GiaTang")
                        .HasColumnType("float");

                    b.Property<int?>("ID_Xe")
                        .HasColumnType("int");

                    b.Property<int?>("Id_Tuyen")
                        .HasColumnType("int");

                    b.Property<string>("Ngayle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("ThoiGianDi")
                        .HasColumnType("datetime2");

                    b.Property<int>("Trangthai")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Xe");

                    b.HasIndex("Id_Tuyen");

                    b.ToTable("Chuyens");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Diadiem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Diachi")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Ten")
                        .IsUnique();

                    b.ToTable("Diadiems");
                });

            modelBuilder.Entity("ThanhBuoi.Models.DonHang", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Id_momoRes")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MaDon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mota")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhuongThucThanhToan")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("RequestId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TaiKhoanId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Tien")
                        .HasColumnType("float");

                    b.Property<double>("TienPhaiTra")
                        .HasColumnType("float");

                    b.Property<int?>("Trangthai")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TaiKhoanId");

                    b.ToTable("DonHangs");
                });

            modelBuilder.Entity("ThanhBuoi.Models.DonHangChiTiet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ID_DonHang")
                        .HasColumnType("int");

                    b.Property<int>("ID_HangGui")
                        .HasColumnType("int");

                    b.Property<double>("Tien")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ID_DonHang");

                    b.HasIndex("ID_HangGui");

                    b.ToTable("DonHangChiTiets");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Ghe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ID_Hang")
                        .HasColumnType("int");

                    b.Property<bool>("KhoangTrong")
                        .HasColumnType("bit");

                    b.Property<int>("STT")
                        .HasColumnType("int");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ID_Hang");

                    b.ToTable("Ghes");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Hang", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ID_Tang")
                        .HasColumnType("int");

                    b.Property<int>("STT")
                        .HasColumnType("int");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ID_Tang");

                    b.ToTable("Hangs");
                });

            modelBuilder.Entity("ThanhBuoi.Models.HangGui", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DiachiNguoiNhan")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int?>("ID_Chuyen")
                        .HasColumnType("int");

                    b.Property<string>("ID_TaiKhoan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoaiHang")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("SdtNguoiGui")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SdtNguoiNhan")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TenHang")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TenNguoiGui")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TenNguoiNhan")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("TrangThai")
                        .HasColumnType("int");

                    b.Property<double>("TrongLuong")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ID_Chuyen");

                    b.HasIndex("ID_TaiKhoan");

                    b.ToTable("HangGuis");

                    b.HasDiscriminator<string>("Discriminator").HasValue("HangGui");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ThanhBuoi.Models.LoaiXe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LoaiGheXe")
                        .HasColumnType("int");

                    b.Property<string>("Ma")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Mota")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Ten")
                        .IsUnique();

                    b.ToTable("loaiXes");
                });

            modelBuilder.Entity("ThanhBuoi.Models.SoDoGhe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ID_Xe")
                        .HasColumnType("int");

                    b.Property<string>("LoaiGhe")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("SoGhe")
                        .HasColumnType("int");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ID_Xe");

                    b.ToTable("SoDoGhes");
                });

            modelBuilder.Entity("ThanhBuoi.Models.TaiKhoan", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ThanhBuoi.Models.Tang", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ID_SoDoGhe")
                        .HasColumnType("int");

                    b.Property<int>("STT")
                        .HasColumnType("int");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ID_SoDoGhe");

                    b.ToTable("Tangs");
                });

            modelBuilder.Entity("ThanhBuoi.Models.TinTuc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LuotXem")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayDang")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TieuDe")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("TinTucs");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Tuyen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ID_DiemDen")
                        .HasColumnType("int");

                    b.Property<int>("ID_DiemDi")
                        .HasColumnType("int");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("ID_DiemDen");

                    b.HasIndex("ID_DiemDi");

                    b.HasIndex("Ten")
                        .IsUnique();

                    b.ToTable("Tuyens");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Ve", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CMND")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DiemDon")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Hanhli")
                        .HasColumnType("int");

                    b.Property<int>("ID_Chuyen")
                        .HasColumnType("int");

                    b.Property<int>("ID_Ghe")
                        .HasColumnType("int");

                    b.Property<string>("ID_TaiKhoan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MaVe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Sdt")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Ten")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<double>("Tien")
                        .HasColumnType("float");

                    b.Property<int>("TrangThai")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ID_Chuyen");

                    b.HasIndex("ID_Ghe");

                    b.HasIndex("ID_TaiKhoan");

                    b.ToTable("Ves");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Xe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BienSo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("HangXe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ID_LoaiXe")
                        .HasColumnType("int");

                    b.Property<string>("MaXe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Trangthai")
                        .HasColumnType("int");

                    b.Property<double>("TrongTaiHang")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BienSo")
                        .IsUnique();

                    b.HasIndex("ID_LoaiXe");

                    b.HasIndex("Ten")
                        .IsUnique();

                    b.ToTable("Xes");
                });

            modelBuilder.Entity("ThanhBuoi.Models.DTO.DTOHang", b =>
                {
                    b.HasBaseType("ThanhBuoi.Models.HangGui");

                    b.HasDiscriminator().HasValue("DTOHang");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ThanhBuoi.Models.TaiKhoan", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ThanhBuoi.Models.TaiKhoan", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThanhBuoi.Models.TaiKhoan", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ThanhBuoi.Models.TaiKhoan", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThanhBuoi.Models.Chuyen", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Xe", "Xe")
                        .WithMany("Chuyens")
                        .HasForeignKey("ID_Xe");

                    b.HasOne("ThanhBuoi.Models.Tuyen", "Tuyen")
                        .WithMany("Chuyens")
                        .HasForeignKey("Id_Tuyen");

                    b.Navigation("Tuyen");

                    b.Navigation("Xe");
                });

            modelBuilder.Entity("ThanhBuoi.Models.DonHang", b =>
                {
                    b.HasOne("ThanhBuoi.Models.TaiKhoan", null)
                        .WithMany("DonHangs")
                        .HasForeignKey("TaiKhoanId");
                });

            modelBuilder.Entity("ThanhBuoi.Models.DonHangChiTiet", b =>
                {
                    b.HasOne("ThanhBuoi.Models.DonHang", "DonHang")
                        .WithMany("DonHangChiTiets")
                        .HasForeignKey("ID_DonHang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThanhBuoi.Models.HangGui", "HangGui")
                        .WithMany()
                        .HasForeignKey("ID_HangGui")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DonHang");

                    b.Navigation("HangGui");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Ghe", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Hang", "Hang")
                        .WithMany("Ghes")
                        .HasForeignKey("ID_Hang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hang");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Hang", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Tang", "Tang")
                        .WithMany("Hangs")
                        .HasForeignKey("ID_Tang")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tang");
                });

            modelBuilder.Entity("ThanhBuoi.Models.HangGui", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Chuyen", "Chuyen")
                        .WithMany()
                        .HasForeignKey("ID_Chuyen");

                    b.HasOne("ThanhBuoi.Models.TaiKhoan", "TaiKhoan")
                        .WithMany("HangGuis")
                        .HasForeignKey("ID_TaiKhoan");

                    b.Navigation("Chuyen");

                    b.Navigation("TaiKhoan");
                });

            modelBuilder.Entity("ThanhBuoi.Models.SoDoGhe", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Xe", "Xe")
                        .WithMany("soDoGhes")
                        .HasForeignKey("ID_Xe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Xe");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Tang", b =>
                {
                    b.HasOne("ThanhBuoi.Models.SoDoGhe", "SoDoGhe")
                        .WithMany("Tangs")
                        .HasForeignKey("ID_SoDoGhe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SoDoGhe");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Tuyen", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Diadiem", "DiemDen")
                        .WithMany()
                        .HasForeignKey("ID_DiemDen")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThanhBuoi.Models.Diadiem", "DiemDi")
                        .WithMany()
                        .HasForeignKey("ID_DiemDi")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiemDen");

                    b.Navigation("DiemDi");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Ve", b =>
                {
                    b.HasOne("ThanhBuoi.Models.Chuyen", "Chuyen")
                        .WithMany()
                        .HasForeignKey("ID_Chuyen")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThanhBuoi.Models.Ghe", "Ghe")
                        .WithMany("Ves")
                        .HasForeignKey("ID_Ghe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThanhBuoi.Models.TaiKhoan", "TaiKhoan")
                        .WithMany("Ves")
                        .HasForeignKey("ID_TaiKhoan");

                    b.Navigation("Chuyen");

                    b.Navigation("Ghe");

                    b.Navigation("TaiKhoan");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Xe", b =>
                {
                    b.HasOne("ThanhBuoi.Models.LoaiXe", "LoaiXe")
                        .WithMany()
                        .HasForeignKey("ID_LoaiXe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoaiXe");
                });

            modelBuilder.Entity("ThanhBuoi.Models.DonHang", b =>
                {
                    b.Navigation("DonHangChiTiets");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Ghe", b =>
                {
                    b.Navigation("Ves");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Hang", b =>
                {
                    b.Navigation("Ghes");
                });

            modelBuilder.Entity("ThanhBuoi.Models.SoDoGhe", b =>
                {
                    b.Navigation("Tangs");
                });

            modelBuilder.Entity("ThanhBuoi.Models.TaiKhoan", b =>
                {
                    b.Navigation("DonHangs");

                    b.Navigation("HangGuis");

                    b.Navigation("Ves");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Tang", b =>
                {
                    b.Navigation("Hangs");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Tuyen", b =>
                {
                    b.Navigation("Chuyens");
                });

            modelBuilder.Entity("ThanhBuoi.Models.Xe", b =>
                {
                    b.Navigation("Chuyens");

                    b.Navigation("soDoGhes");
                });
#pragma warning restore 612, 618
        }
    }
}
