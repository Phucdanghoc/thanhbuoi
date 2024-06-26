﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Services;

namespace ThanhBuoi.Controllers
{
    public class VeController : Controller
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;

        public VeController(DataContext dataContext, IEmailService emailService)
        {
            _context = dataContext;
            _emailService = emailService;
        }

        public IActionResult Index(string mave = null)
        {
            if (!string.IsNullOrEmpty(mave))
            {
                var ve = _context.Ves.FirstOrDefault(m => m.MaVe == mave);
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
                                   .Include(v => v.Chuyen).ThenInclude(c => c.Xe).ThenInclude(l => l.LoaiXe)
                                   .Include(v => v.Ghe).ThenInclude(h => h.Hang)
                                   .FirstOrDefaultAsync(v => v.Id == id);

            if (ve == null)
            {
                return NotFound();
            }

            return View(ve);
        }

        public async Task<IActionResult> HoaDon(int id)
        {
            var hoadon = await _context.DonHangs.FirstOrDefaultAsync(d => d.Id == id);
            if (hoadon == null)
            {
                return NotFound();
            }

            var ves = await _context.DonHangChiTiets
                                    .Include(d => d.Ve)
                                    .Where(h => h.DonHang.Id == hoadon.Id)
                                    .ToListAsync();

            return View(ves);
        }

        [HttpPost]
        public async Task<ActionResult> Cancel(int id)
        {
            var ve = await _context.Ves
                .Include(v => v.Chuyen).ThenInclude(c => c.Xe).ThenInclude(l => l.LoaiXe)
                .Include(v => v.Ghe)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (ve == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy vé để hủy.";
                return RedirectToAction("Index", "Chuyens");
            }

            try
            {
                DateTime futureTime = DateTime.Now.AddHours(24);
                if (ve.Chuyen.ThoiGianDi < futureTime)
                {
                    TempData["ErrorMessage"] = "Vé không thể hủy bây giờ";
                    return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
                }

                string phuongthucthanhtoan = ve.phuongthucthanhtoan;
                double refund = Math.Round(ve.Tien * 0.7);
                string body = _emailService.makeBodyTicketCancel(ve, refund);

                var veHuy = new VeHuy
                {
                    chuyen = ve.Chuyen,
                    Name = ve.Ten,
                    ngaytao = DateTime.Now,
                    hoantien = refund,
                    CMND = ve.CMND,
                    Mave = ve.MaVe,
                    Ghe = ve.Ghe,
                    Email = ve.Email
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
                ve.Email = null;

                _context.VeHuys.Add(veHuy);
                await _context.SaveChangesAsync();

                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();

                // Sending email after updating the database
                await _emailService.SendEmailAsync(veHuy.Email, "Xác nhận hủy vé", body);

                TempData["SuccessMessage"] = "Đã hủy vé";
                return RedirectToAction("Details", "Vehuys", new { id = veHuy.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi hủy vé: {ex.Message}";
                return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
            }
        }
    }
}
