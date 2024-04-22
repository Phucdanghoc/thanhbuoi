using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class DonHang
    {
        [Key]
        public int Id { get; set; }



        [Required]
        [StringLength(255)]
        public string RequestId { get; set; }

        [Required]
        [StringLength(255)]
        public string Id_momoRes { get; set; }

        public double Tien { get; set; }

        public double TienPhaiTra { get; set; }

        [Required]
        [StringLength(255)]
        public string PhuongThucThanhToan { get; set; }

        [Required]
        [StringLength(255)]
        public string Mota { get; set; }

        [Required]
        [StringLength(255)]
        public string Trangthai { get; set; }

        public DateTime NgayTao { get; set; }

        public ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}
