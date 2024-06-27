namespace ThanhBuoi.Models.DTO
{
    public class DoanhThuVe
    {
        public double  TotalPrice { get; set; }
        public Chuyen? Chuyen { get; set; }

    }
    public class DoanhTheoNgay
    {
        public DateTime datetime { get; set; }
        public double totalHang { get; set; }
        public double totalHuy { get; set; }
        public double totalVe{ get; set; }

    }
}
