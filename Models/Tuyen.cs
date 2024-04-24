using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [ForeignKey("ID_diemDi")]
        public Diadiem DiemDi { get; set; }

        [ForeignKey("ID_diemDen")]
        public Diadiem Diemden { get; set; }

        public double Khoangcach { get; set; }

        [JsonIgnore]
        public ICollection<Chuyen> Chuyens { get; set; }
    }
}
