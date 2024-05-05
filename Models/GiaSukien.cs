using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThanhBuoi.Models
{
    public class GiaSukien
    {
        [Key]
        public int Id { get; set; }

        public String Ten { get; set; }

        public double Gia_hang { get; set; }

        public double Gia_ve { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

    }
}
