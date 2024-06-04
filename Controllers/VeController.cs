using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;
using ThanhBuoi.Services;

namespace ThanhBuoi.Controllers
{
    public class VeController : Controller
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;
        private readonly MomoServices _momoServices;
        public VeController(DataContext dataContext,IEmailService emailService,MomoServices momoServices) {
            _context = dataContext;
            _emailService = emailService;
            _momoServices = momoServices;
        }
        public IActionResult Index(string mave = null)
        {
            if (!string.IsNullOrEmpty(mave))
            {
                var ve = _context.Ves.Where(m => m.MaVe == mave).FirstOrDefault();
                if (ve == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy vé";

                }
                return View(ve);
            }
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var ve = await _context.Ves
                                   .Include(c => c.Chuyen)
                                   .ThenInclude(x => x.Xe)
                                   .Include(g => g.Ghe)
                                   .FirstOrDefaultAsync(v => v.Id == id);
            return View(ve);
        }
        public async Task<IActionResult> HoaDon(int id)
        {
            var hoadon = _context.DonHangs.FirstAsync(d => d.Id == id);
            List<DonHangChiTiet> ves   = _context.DonHangChiTiets.Include(v => v.Ve).Where(h => h.Id == hoadon.Id).ToList();
            return View(ves);
        }
        [HttpPost]
        public async Task<ActionResult> Cancel(int id)
        {
            Ve? ve = await _context.Ves
                .Include(c => c.Chuyen)
                    .ThenInclude(x => x.Xe).ThenInclude(l => l.LoaiXe)
                .Include(g => g.Ghe)
                .FirstOrDefaultAsync(v => v.Id == id);

            try
            {
                if (ve == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy vé để hủy.";
                    return RedirectToAction("Index", "Chuyens");
                }

                DateTime futureTime = DateTime.Now.AddHours(24);
                if (ve.Chuyen.ThoiGianDi < futureTime)
                {
                    TempData["ErrorMessage"] = "Vé không thể hủy bây giờ";
                    return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
                }

                string phuongthucthanhtoan = ve.phuongthucthanhtoan;
                double refund = Math.Round(ve.Tien * 0.7);
                string body = _emailService.makeBodyTicketCancel(ve, refund);

                VeHuy veHuy = new VeHuy
                {
                    chuyen = ve.Chuyen,
                    Name = ve.Ten,
                    ngaytao = DateTime.Now,
                    hoantien = refund,
                    CMND = ve.CMND,
                    Mave = ve.MaVe,
                    Ghe = ve.Ghe,
                    Email = ve.email
                };

                ve.TrangThai = TrangThaiVe.Empty;
                ve.Ten = null;
                ve.CMND = null;
                ve.Sdt = null;
                ve.MaVe = null;
                ve.TaiKhoan = null;
                ve.Hanhli = 0;
                ve.Tien = 0;
                ve.phuongthucthanhtoan = null;
                ve.isCancel = true;
                ve.email = null;

                _context.VeHuys.Add(veHuy);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();

                // Sending email after updating the database
                await _emailService.SendEmailAsync(veHuy.Email, "Xác nhận hủy vé", body);

                TempData["SuccessMessage"] = $"Đã hủy vé";
                return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi hủy vé: {ex.Message}";
            }

            return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
        }


    }
}
