using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class GiaTuyen
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("ID_TaiKhoan")]
        public TaiKhoan TaiKhoan { get; set; }

        public double Gia_hang { get; set; }

        public double Gia_ve { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

    }
}
