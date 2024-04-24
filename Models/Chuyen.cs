using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class Chuyen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Ten { get; set; }

        [ForeignKey("ID_Xe")]
        public Xe xe { get; set; }

        [ForeignKey("Id_Tuyen")] 
        public Tuyen Tuyen { get; set; }
        [Required]
        [StringLength(255)]
        public string Diachibatdau { get; set; }
        [Required]
        [StringLength(255)]
        public string Diachiketthuc { get; set; }
        public DateTime Ngaydi { get; set; }
        public TimeSpan Giodi { get; set; }
        public TimeSpan GioDenNoi { get; set; }
        [Required]
        [StringLength(255)]
        public string Trangthai { get; set; }
        [ForeignKey("ID_GiaSuKien")]
        public GiaSukien GiaSukien { get; set; }
        public double Gia { get; set; }
        public double KhoiluongHang { get; set; }
        public double GiaHangKhoiDiem { get; set; }
    }
}
