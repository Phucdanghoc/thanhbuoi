using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public Xe? Xe { get; set; }

        [ForeignKey("Id_Tuyen")] 
        public Tuyen? Tuyen { get; set; }
        [Required]
        [StringLength(255)]
        public string? DiemDon { get; set; }
        public DateTime ThoiGianDi { get; set; }
        public DateTime ThoiGianDen { get; set; }
        [Required]
        [StringLength(255)]
        public string? Trangthai { get; set; }
        [ForeignKey("ID_GiaSuKien")]
        public GiaSukien GiaSukien { get; set; }
        public double Gia { get; set; }
        public double KhoiluongHang { get; set; }
        public double GiaHangKhoiDiem { get; set; }

        [JsonIgnore]
        public ICollection<Ve> Ves;
    }
}
