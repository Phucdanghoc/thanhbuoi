namespace ThanhBuoi.Services
{
    public class VNPayServices
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly VNPayServices vnpayService = new VNPayServices();


        public void Payment()
        {
            var vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
            var vnp_Returnurl = "";
            var vnp_TmnCode = "DL6066PM";
            var vnp_HashSecret = "AWO0OM8E47VB0AYD9UB5J83FDCX90VCB";
            var vnp_TxnRef = "VNP" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var vnp_OrderType = "billpayment";
            var vnp_OrderInfor = "Thanh toán hóa đơn thành bưởi";
            var vnp_Amount = 1000 * 100;
            var vnp_Locale = "vn";
            var vnp_BankCode = "NCB";

            var vpn_IpAddr = "localhost:7723/admin";



        }
    }
}
