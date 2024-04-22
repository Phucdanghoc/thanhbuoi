using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class Tuyen
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Ten { get; set; }

        [Required]
        [StringLength(255)]
        public string Thoigian { get; set; }

        [Required]
        [StringLength(255)]
        public string Diemdi { get; set; }

        [Required]
        [StringLength(255)]
        public string Diemden { get; set; }

        public double Khoangcach { get; set; }

        public DateTime Capnhapngay { get; set; }

        public ICollection<GiaTuyen> GiaTuyens { get; set; }

        public ICollection<Chuyen> Chuyens { get; set; }
    }
}
