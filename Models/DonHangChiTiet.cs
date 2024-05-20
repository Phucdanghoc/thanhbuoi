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

        [ForeignKey("ID_HangGui")]
        public HangGui HangGui { get; set; }

        public double Tien { get; set; }


    }
}
