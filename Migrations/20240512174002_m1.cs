using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThanhBuoi.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diadiems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Diachi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diadiems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "loaiXes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loaiXes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TinTucs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgayDang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LuotXem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinTucs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Id_momoRes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Tien = table.Column<double>(type: "float", nullable: false),
                    TienPhaiTra = table.Column<double>(type: "float", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Trangthai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaiKhoanId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonHangs_AspNetUsers_TaiKhoanId",
                        column: x => x.TaiKhoanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ID_DiemDi = table.Column<int>(type: "int", nullable: false),
                    ID_DiemDen = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tuyens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tuyens_Diadiems_ID_DiemDen",
                        column: x => x.ID_DiemDen,
                        principalTable: "Diadiems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tuyens_Diadiems_ID_DiemDi",
                        column: x => x.ID_DiemDi,
                        principalTable: "Diadiems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Xes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BienSo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MaXe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_LoaiXe = table.Column<int>(type: "int", nullable: false),
                    Trangthai = table.Column<int>(type: "int", nullable: false),
                    HangXe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrongTaiHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Xes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Xes_loaiXes_ID_LoaiXe",
                        column: x => x.ID_LoaiXe,
                        principalTable: "loaiXes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangChiTiets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_DonHang = table.Column<int>(type: "int", nullable: false),
                    Tien = table.Column<double>(type: "float", nullable: false),
                    LoaiDonHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangChiTiets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonHangChiTiets_DonHangs_ID_DonHang",
                        column: x => x.ID_DonHang,
                        principalTable: "DonHangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ID_Xe = table.Column<int>(type: "int", nullable: true),
                    Id_Tuyen = table.Column<int>(type: "int", nullable: true),
                    DiemDon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ThoiGianDi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Trangthai = table.Column<int>(type: "int", maxLength: 255, nullable: false),
                    Gia = table.Column<double>(type: "float", nullable: false),
                    Ngayle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaTang = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuyens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chuyens_Tuyens_Id_Tuyen",
                        column: x => x.Id_Tuyen,
                        principalTable: "Tuyens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chuyens_Xes_ID_Xe",
                        column: x => x.ID_Xe,
                        principalTable: "Xes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SoDoGhes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoGhe = table.Column<int>(type: "int", nullable: false),
                    LoaiGhe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ID_Xe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoDoGhes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoDoGhes_Xes_ID_Xe",
                        column: x => x.ID_Xe,
                        principalTable: "Xes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangGuis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ID_DonHangChiTiet = table.Column<int>(type: "int", nullable: false),
                    ID_Chuyen = table.Column<int>(type: "int", nullable: false),
                    TenHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TenNguoiGui = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SdtNguoiGui = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SdtNguoiNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DiachiNguoiNhan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrongLuong = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangGuis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HangGuis_AspNetUsers_ID_TaiKhoan",
                        column: x => x.ID_TaiKhoan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HangGuis_Chuyens_ID_Chuyen",
                        column: x => x.ID_Chuyen,
                        principalTable: "Chuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HangGuis_DonHangChiTiets_ID_DonHangChiTiet",
                        column: x => x.ID_DonHangChiTiet,
                        principalTable: "DonHangChiTiets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STT = table.Column<int>(type: "int", nullable: false),
                    ID_SoDoGhe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tangs_SoDoGhes_ID_SoDoGhe",
                        column: x => x.ID_SoDoGhe,
                        principalTable: "SoDoGhes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STT = table.Column<int>(type: "int", nullable: false),
                    ID_Tang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hangs_Tangs_ID_Tang",
                        column: x => x.ID_Tang,
                        principalTable: "Tangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ghes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    STT = table.Column<int>(type: "int", nullable: false),
                    KhoangTrong = table.Column<bool>(type: "bit", nullable: false),
                    ID_Hang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ghes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ghes_Hangs_ID_Hang",
                        column: x => x.ID_Hang,
                        principalTable: "Hangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_TaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ID_Chuyen = table.Column<int>(type: "int", nullable: false),
                    MaVe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_Ghe = table.Column<int>(type: "int", nullable: false),
                    Tien = table.Column<double>(type: "float", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Sdt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CMND = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DiemDon = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    Hanhli = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngaydacbiet = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ves_AspNetUsers_ID_TaiKhoan",
                        column: x => x.ID_TaiKhoan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ves_Chuyens_ID_Chuyen",
                        column: x => x.ID_Chuyen,
                        principalTable: "Chuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ves_Ghes_ID_Ghe",
                        column: x => x.ID_Ghe,
                        principalTable: "Ghes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyens_Id_Tuyen",
                table: "Chuyens",
                column: "Id_Tuyen");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyens_ID_Xe",
                table: "Chuyens",
                column: "ID_Xe");

            migrationBuilder.CreateIndex(
                name: "IX_Diadiems_Ten",
                table: "Diadiems",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHangChiTiets_ID_DonHang",
                table: "DonHangChiTiets",
                column: "ID_DonHang");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_TaiKhoanId",
                table: "DonHangs",
                column: "TaiKhoanId");

            migrationBuilder.CreateIndex(
                name: "IX_Ghes_ID_Hang",
                table: "Ghes",
                column: "ID_Hang");

            migrationBuilder.CreateIndex(
                name: "IX_HangGuis_ID_Chuyen",
                table: "HangGuis",
                column: "ID_Chuyen");

            migrationBuilder.CreateIndex(
                name: "IX_HangGuis_ID_DonHangChiTiet",
                table: "HangGuis",
                column: "ID_DonHangChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_HangGuis_ID_TaiKhoan",
                table: "HangGuis",
                column: "ID_TaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_Hangs_ID_Tang",
                table: "Hangs",
                column: "ID_Tang");

            migrationBuilder.CreateIndex(
                name: "IX_loaiXes_Ten",
                table: "loaiXes",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoDoGhes_ID_Xe",
                table: "SoDoGhes",
                column: "ID_Xe");

            migrationBuilder.CreateIndex(
                name: "IX_Tangs_ID_SoDoGhe",
                table: "Tangs",
                column: "ID_SoDoGhe");

            migrationBuilder.CreateIndex(
                name: "IX_Tuyens_ID_DiemDen",
                table: "Tuyens",
                column: "ID_DiemDen");

            migrationBuilder.CreateIndex(
                name: "IX_Tuyens_ID_DiemDi",
                table: "Tuyens",
                column: "ID_DiemDi");

            migrationBuilder.CreateIndex(
                name: "IX_Tuyens_Ten",
                table: "Tuyens",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ves_ID_Chuyen",
                table: "Ves",
                column: "ID_Chuyen");

            migrationBuilder.CreateIndex(
                name: "IX_Ves_ID_Ghe",
                table: "Ves",
                column: "ID_Ghe");

            migrationBuilder.CreateIndex(
                name: "IX_Ves_ID_TaiKhoan",
                table: "Ves",
                column: "ID_TaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_Xes_BienSo",
                table: "Xes",
                column: "BienSo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Xes_ID_LoaiXe",
                table: "Xes",
                column: "ID_LoaiXe");

            migrationBuilder.CreateIndex(
                name: "IX_Xes_Ten",
                table: "Xes",
                column: "Ten",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HangGuis");

            migrationBuilder.DropTable(
                name: "TinTucs");

            migrationBuilder.DropTable(
                name: "Ves");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DonHangChiTiets");

            migrationBuilder.DropTable(
                name: "Chuyens");

            migrationBuilder.DropTable(
                name: "Ghes");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "Tuyens");

            migrationBuilder.DropTable(
                name: "Hangs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Diadiems");

            migrationBuilder.DropTable(
                name: "Tangs");

            migrationBuilder.DropTable(
                name: "SoDoGhes");

            migrationBuilder.DropTable(
                name: "Xes");

            migrationBuilder.DropTable(
                name: "loaiXes");
        }
    }
}
