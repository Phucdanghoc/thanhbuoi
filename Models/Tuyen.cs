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

        [ForeignKey("ID_DiemDi")]
        
        public Diadiem DiemDi { get; set; }

        [ForeignKey("ID_DiemDen")]
        public Diadiem DiemDen { get; set; }

        public double Khoangcach { get; set; }

        [JsonIgnore]
        public ICollection<Chuyen> Chuyens { get; set; }
    }
}
