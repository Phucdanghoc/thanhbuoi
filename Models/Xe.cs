using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThanhBuoi.Models
{
    public class Xe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Ten { get; set; }

        [Required]
        [StringLength(255)]
        public string BienSo { get; set; }

        public string MaXe { get; set; }

        [ForeignKey("ID_LoaiXe")]
        public LoaiXe LoaiXe { get; set; }

        public string HangXe { get; set; }
        public ICollection<SoDoGhe> soDoGhes { get; set; }

    }
}
