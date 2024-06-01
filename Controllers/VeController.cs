using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class VeController : Controller
    {
        private readonly DataContext _context;
        public VeController(DataContext dataContext) {
            _context = dataContext;
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
        [HttpPost]
        public async Task<ActionResult> Cancel(int id)
        {
            Ve? ve = await _context.Ves
                .Include(c => c.Chuyen)
                .ThenInclude(x => x.Xe)
                .Include(g => g.Ghe)
                .FirstOrDefaultAsync(v => v.Id == id);

            try
            {
                DateTime futureTime = DateTime.Now.AddHours(24);
                if (ve.Chuyen.ThoiGianDi < futureTime)
                {
                    TempData["ErrorMessage"] = "Vé không thể hủy bây giờ";
                    return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
                }

                if (ve != null)
                {
                    ve.TrangThai = TrangThaiVe.Empty;
                    ve.Ten = null;
                    ve.CMND = null;
                    ve.Sdt = null;
                    ve.MaVe = null;
                    ve.TaiKhoan = null;
                    ve.Hanhli = 0;
                    ve.Tien = 0;
                    ve.Ghe.KhoangTrong = true;
                    _context.Ghes.Update(ve.Ghe);
                    _context.Ves.Update(ve);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Đã hủy vé";
                    return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy vé để hủy.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi hủy vé: {ex.Message}";
            }

            return RedirectToAction("Details", "Chuyens", new { id = ve.Chuyen.Id });
        }

    }
}
