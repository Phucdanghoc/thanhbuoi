using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class Diadiem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Ten { get; set; }

        [Required]
        [StringLength(255)]
        public string Diachi { get; set; }

        public ICollection<Tuyen> Tuyens { get; set; }
    }
}
