using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class LoaiXe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten { get; set; }

        [Required]
        [StringLength(50)]
        public string Ma { get; set; }


        [Required]
        [StringLength(50)]  
        public string Mota { get; set;}
    }
}
