using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThanhBuoi.Models
{
    public class VeHuy
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string  CMND { get; set; }
        public string Email { get; set; }
        public string Mave { get; set; }
        [ForeignKey("ID_Chuyen")]
        public Chuyen chuyen { get; set;}
        public Ghe Ghe { get; set; }
        public double  hoantien { get; set;}
        public DateTime  ngaytao { get; set; }
    }
}
