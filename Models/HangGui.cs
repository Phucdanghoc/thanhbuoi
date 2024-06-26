using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public enum TrangThaiHang
    {
        Waiting,
        Shipping,
        Recived
    }

    public enum LoaiHang 
    {
        
        HangNho,
        HangLon,
        HangDacBiet,
        HangCongKenh
    }
    public class HangGui
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ID_TaiKhoan")]
        public TaiKhoan? TaiKhoan { get; set; }

        [ForeignKey("ID_Chuyen")]
        public Chuyen? Chuyen { get; set; }

        [Required]
        [StringLength(255)]
        public string TenHang { get; set; }

        [Required]
        [StringLength(255)]
        public string TenNguoiGui { get; set; }

        [Required]
        [StringLength(255)]
        public string SdtNguoiGui { get; set; }

        [Required]
        [StringLength(255)]
        public string TenNguoiNhan { get; set; }

        [Required]
        [StringLength(255)]
        public string SdtNguoiNhan { get; set; }


        [Required]
        [StringLength(255)]
        public string DiaChiNguoiGui { get; set; }

        [Required]
        [StringLength(255)]
        public string DiachiNguoiNhan { get; set; }

        public TrangThaiHang? TrangThai { get; set; }

        public DateTime? NgayTao { get; set; }


        public string? LoaiHang { get;set; }
    }

}
