using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class DonHangChiTiet
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("ID_DonHang")]
        public DonHang DonHang { get; set; }

        public double Tien { get; set; }

        [Required]
        [StringLength(255)]
        public string LoaiDonHang { get; set; }

    }
}
