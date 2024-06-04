using ThanhBuoi.Models;

namespace ThanhBuoi.Services
{
    public interface IEmailService
    {
        Task  SendEmailAsync(string email,string subject,string body);

        string makeBodyEmailOrder(DonHang donhang);

        string makeBodyTicketBooked(List<Ve> ve);
        string makeBodyTicketCancel(Ve ve,double refund,string momoreq = null);
    }
}
