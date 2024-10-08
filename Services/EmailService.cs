﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;
using System.Text;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ThanhBuoi.Services
{
    public class EmailService : IEmailService
    {

        private readonly EmailSetting emailSetting;

        public EmailService(IOptions<EmailSetting> options)
        {
            this.emailSetting = options.Value;
        }

        public string makeBodyEmailOrder(DonHang donHang, List<HangGui> hangGuis)
        {
            var stringBuilder = new StringBuilder();

            // Header
            stringBuilder.AppendLine("<!DOCTYPE html>");
            stringBuilder.AppendLine("<html lang='en'>");
            stringBuilder.AppendLine("<head>");
            stringBuilder.AppendLine("<meta charset='UTF-8'>");
            stringBuilder.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            stringBuilder.AppendLine("<title>Chi tiết đơn hàng</title>");
            stringBuilder.AppendLine("<style>");
            stringBuilder.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f8f9fa; }");
            stringBuilder.AppendLine(".container { max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }");
            stringBuilder.AppendLine("h1, h2 { color: #007bff; }");
            stringBuilder.AppendLine("ul { list-style-type: none; padding-left: 0; }");
            stringBuilder.AppendLine("li { margin-bottom: 10px; }");
            stringBuilder.AppendLine("</style>");
            stringBuilder.AppendLine("</head>");
            stringBuilder.AppendLine("<body>");
            stringBuilder.AppendLine("<div class='container'>");

            // Content
            stringBuilder.AppendLine("<h1>Chi tiết đơn hàng</h1>");
            stringBuilder.AppendLine("<hr>");

            // Order details
            stringBuilder.AppendLine("<h2>Thông tin đơn hàng</h2>");
            stringBuilder.AppendLine("<ul>");
            stringBuilder.AppendLine($"<li><strong>Trạng thái:</strong> {donHang.Trangthai}</li>");
            stringBuilder.AppendLine($"<li><strong>Ngày tạo:</strong> {donHang.NgayTao.ToString("dd/MM/yyyy")}</li>");
            stringBuilder.AppendLine($"<li><strong>Tổng tiền:</strong> {donHang.Tien.ToString("N0")} ₫</li>");
            stringBuilder.AppendLine("</ul>");

            // List of items
            stringBuilder.AppendLine("<h2>Danh sách hàng</h2>");
            stringBuilder.AppendLine("<ul>");
            foreach (var hanggui in hangGuis)
            {
                stringBuilder.AppendLine("<li>");
                stringBuilder.AppendLine($"<strong>Tên hàng:</strong> {hanggui.TenHang}<br>");
                stringBuilder.AppendLine($"<strong>Người gửi:</strong> {hanggui.TenNguoiGui}<br>");
                stringBuilder.AppendLine($"<strong>Địa chỉ người nhận:</strong> {hanggui.DiaChiNguoiGui}<br>");
                stringBuilder.AppendLine($"<strong>Số điện thoại người gửi:</strong> {hanggui.SdtNguoiGui}<br>");
                stringBuilder.AppendLine($"<strong>Người nhận:</strong> {hanggui.TenNguoiNhan}<br>");
                stringBuilder.AppendLine($"<strong>Số điện thoại người nhận:</strong> {hanggui.SdtNguoiNhan}<br>");
                stringBuilder.AppendLine($"<strong>Địa chỉ người nhận:</strong> {hanggui.DiachiNguoiNhan}<br>");
                stringBuilder.AppendLine("</li>");
            }
            stringBuilder.AppendLine("</ul>");

            // Additional information
            stringBuilder.AppendLine("<h2>Thông tin thêm</h2>");
            stringBuilder.AppendLine("<ul>");
            stringBuilder.AppendLine($"<li><strong>Email:</strong> {donHang.Email}</li>");
            stringBuilder.AppendLine($"<li><strong>Mô tả:</strong> {donHang.Mota}</li>");
            stringBuilder.AppendLine($"<li><strong>Phương thức thanh toán:</strong> {donHang.PhuongThucThanhToan}</li>");
            stringBuilder.AppendLine("</ul>");

            stringBuilder.AppendLine("</div>");
            stringBuilder.AppendLine("</body>");
            stringBuilder.AppendLine("</html>");

            return stringBuilder.ToString();
        }

        public string makeBodyTicketBooked(List<Ve> tickets)
        {
            var totalPrice = tickets.Sum(ve => ve.Tien);
            string totalFormattedPrice = totalPrice.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<!DOCTYPE html>");
            stringBuilder.AppendLine("<html lang='en'>");
            stringBuilder.AppendLine("<head>");
            stringBuilder.AppendLine("<meta charset='UTF-8'>");
            stringBuilder.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            stringBuilder.AppendLine("<title>Thông tin vé</title>");
            stringBuilder.AppendLine("<style>");
            stringBuilder.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f8f9fa; }");
            stringBuilder.AppendLine(".container { max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }");
            stringBuilder.AppendLine(".ticket-heading { font-weight: bold; margin-bottom: 10px; font-size: 1.2em; }");
            stringBuilder.AppendLine(".ticket-info { margin-bottom: 20px; }");
            stringBuilder.AppendLine(".ticket-info p { margin: 5px 0; }");
            stringBuilder.AppendLine(".special-price { font-weight: bold; color: #007bff; font-size: 1.2em; }");
            stringBuilder.AppendLine(".total-price { font-weight: bold; color: #dc3545; font-size: 1.5em; text-align: right; }");
            stringBuilder.AppendLine("</style>");
            stringBuilder.AppendLine("</head>");
            stringBuilder.AppendLine("<body>");
            stringBuilder.AppendLine("<div class='container'>");

            foreach (var ve in tickets)
            {
                string GiaTien = ve.Tien.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
                string loaighe = ve.Chuyen.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.Ngoi ? "Ghế Ngồi " : "Giường nằm";

                stringBuilder.AppendLine("<div class='ticket-info'>");
                stringBuilder.AppendLine("<p class='ticket-heading'>Thông tin chuyến :</p>");
                stringBuilder.AppendLine($"<p><strong>Tuyến:</strong> {ve.Chuyen.Ten}</p>");
                stringBuilder.AppendLine($"<p><strong>Thời gian khởi hành:</strong> {ve.Chuyen.ThoiGianDi.ToString("HH:mm")} - {ve.Chuyen.ThoiGianDi.ToString("dd/MM/yyyy")}</p>");
                stringBuilder.AppendLine($"<p><strong>Điểm đón:</strong> {ve.Chuyen.DiemDon}</p>");
                stringBuilder.AppendLine($"<p><strong>Giá vé:</strong> <span class='special-price'>{GiaTien}</span></p>");

                stringBuilder.AppendLine("<div class='vehicle-info'>");
                stringBuilder.AppendLine("<p class='ticket-heading'>Thông tin xe :</p>");
                stringBuilder.AppendLine($"<p><strong>Tên xe:</strong> {ve.Chuyen.Xe.Ten}</p>");
                stringBuilder.AppendLine($"<p><strong>Loại xe:</strong> {loaighe}</p>");
                stringBuilder.AppendLine($"<p><strong>Biển số:</strong> {ve.Chuyen.Xe.BienSo}</p>");
                stringBuilder.AppendLine("</div>");

                stringBuilder.AppendLine("</div>");
            }

            stringBuilder.AppendLine("<div class='total-price'>");
            stringBuilder.AppendLine("<p>Tổng giá trị:</p>");
            stringBuilder.AppendLine($"<p>{totalFormattedPrice}</p>");
            stringBuilder.AppendLine("</div>");

            stringBuilder.AppendLine("</div>");
            stringBuilder.AppendLine("</body>");
            stringBuilder.AppendLine("</html>");

            return stringBuilder.ToString();
        }

        public string makeBodyTicketCancel(Ve ve, double refund, string momoreq = null)
        {
            string refundFormatted = refund.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
            string GiaTien = ve.Tien.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
            string loaighe = ve.Chuyen.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.Ngoi ? "Ghế Ngồi" : "Giường nằm";

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<!DOCTYPE html>");
            stringBuilder.AppendLine("<html lang='en'>");
            stringBuilder.AppendLine("<head>");
            stringBuilder.AppendLine("<meta charset='UTF-8'>");
            stringBuilder.AppendLine("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            stringBuilder.AppendLine("<title>Thông tin hủy vé</title>");
            stringBuilder.AppendLine("<style>");
            stringBuilder.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 20px; background-color: #f8f9fa; }");
            stringBuilder.AppendLine(".container { max-width: 600px; margin: 0 auto; background-color: #fff; padding: 20px; border-radius:10 px}");
            stringBuilder.AppendLine(".ticket-heading { font-weight: bold; margin-bottom: 10px; font-size: 1.2em; }");
            stringBuilder.AppendLine(".ticket-info { margin-bottom: 20px; }");
            stringBuilder.AppendLine(".ticket-info p { margin: 5px 0; }");
            stringBuilder.AppendLine(".special-price { font-weight: bold; color: #007bff; font-size: 1.2em; }");
            stringBuilder.AppendLine(".refund-amount { font-weight: bold; color: #28a745; font-size: 1.5em; text-align: right; }");
            stringBuilder.AppendLine("</style>");
            stringBuilder.AppendLine("</head>");
            stringBuilder.AppendLine("<body>");
            stringBuilder.AppendLine("<div class='container'>");

            stringBuilder.AppendLine("<div class='ticket-info'>");
            stringBuilder.AppendLine("<p class='ticket-heading'>Thông tin chuyến :</p>");
            stringBuilder.AppendLine($"<p><strong>Tuyến:</strong> {ve.Chuyen.Ten}</p>");
            stringBuilder.AppendLine($"<p><strong>Thời gian khởi hành:</strong> {ve.Chuyen.ThoiGianDi.ToString("HH:mm")} - {ve.Chuyen.ThoiGianDi.ToString("dd/MM/yyyy")}</p>");
            stringBuilder.AppendLine($"<p><strong>Điểm đón:</strong> {ve.Chuyen.DiemDon}</p>");
            stringBuilder.AppendLine($"<p><strong>Giá vé:</strong> <span class='special-price'>{GiaTien}</span></p>");

            stringBuilder.AppendLine("<div class='vehicle-info'>");
            stringBuilder.AppendLine("<p class='ticket-heading'>Thông tin xe :</p>");
            stringBuilder.AppendLine($"<p><strong>Tên xe:</strong> {ve.Chuyen.Xe.Ten}</p>");
            stringBuilder.AppendLine($"<p><strong>Loại xe:</strong> {loaighe}</p>");
            stringBuilder.AppendLine($"<p><strong>Biển số:</strong> {ve.Chuyen.Xe.BienSo}</p>");
            stringBuilder.AppendLine("</div>");
            stringBuilder.AppendLine("</div>");

            stringBuilder.AppendLine("<div class='refund-amount'>");
            stringBuilder.AppendLine("<p>Số tiền hoàn lại:</p>");
            stringBuilder.AppendLine($"<p>{refundFormatted}</p>");
            stringBuilder.AppendLine("</div>");

            stringBuilder.AppendLine("</div>");
            stringBuilder.AppendLine("</body>");
            stringBuilder.AppendLine("</html>");

            return stringBuilder.ToString();
        }



        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(emailSetting.Displayname, email));
            mail.Sender = MailboxAddress.Parse(emailSetting.Email);
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            mail.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailSetting.Host, emailSetting.Port, SecureSocketOptions.StartTls); // Connect to the SMTP server
            await smtp.AuthenticateAsync(emailSetting.Email, emailSetting.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);

        }
    }
}
