using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ThanhBuoi.Models
{
    public class TaiKhoan : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Ten { get; set; }

        [JsonIgnore]
        // Navigation property for DonHang
        public ICollection<DonHang> DonHangs { get; set; }
        [JsonIgnore]
        // Navigation property for HangGui
        public ICollection<HangGui> HangGuis { get; set; }
    }
}
