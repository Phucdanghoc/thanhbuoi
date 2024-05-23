using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Services;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN,SALER")]
    public class BookingController : Controller
    {
        private const int PageSize = 10;
        UserManager<TaiKhoan> _userManager;
        private IEmailService _emailService;
        private readonly DataContext _context;
        public BookingController(DataContext context, UserManager<TaiKhoan> userManager,IEmailService emailService) {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;

        }
        // GET: BookingController
        public IActionResult Index(int page = 1, string searchString = null)
        {
            try
            {
                var listChuyen = GetPaginatedChuyens(page, searchString);
                return View(listChuyen);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách chuyến. Vui lòng thử lại sau.";
                return View();
            }
        }

        private IEnumerable<Chuyen> GetPaginatedChuyens(int page, string searchString = null)
        {
            var query = _context.Chuyens.Include(x => x.Xe).OrderByDescending(c => c.ThoiGianDi).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Ten.Contains(searchString));
            }

            int startIndex = (page - 1) * PageSize;
            var listChuyen = query.Skip(startIndex).Take(PageSize).ToList();

            int totalChuyens = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalChuyens / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listChuyen;
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: BookingController/Create
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int Id, String SDT, String CMND, String Ten,int HanhLi)
        {
            try
            {
                Ve? ve = await _context.Ves
                    .Include(g =>g.Ghe)
                    .Include(c => c.Chuyen)
                    .ThenInclude(x => x.Xe)
                    .FirstOrDefaultAsync(v => v.Id == Id);
                if (ve != null)
                {
                    ve.Ten = Ten;
                    ve.CMND = CMND;
                    ve.Sdt = SDT;
                    ve.MaVe = $"TB-{ve.Chuyen.ThoiGianDi.Day}-{ve.Chuyen.Xe.MaXe}{Id}";
                    ve.TrangThai = TrangThaiVe.Waiting;
                    ve.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
                    ve.Ghe.KhoangTrong = false;
                    ve.Hanhli = HanhLi;
                    if (ve.Hanhli > 20)
                    {
                        ve.Tien = ve.Chuyen.Gia + Math.Round((ve.Hanhli - 20) * 0.5);
                    }
                    else
                    {
                        ve.Tien = ve.Chuyen.Gia;
                    }
                }
                _context.Chuyens.Update(ve.Chuyen);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();

                return RedirectToAction("Payment", "Booking", new { id = Id });
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Ve", "Booking", new { id = Id });

            }
        }
        [HttpGet]
        public async Task<IActionResult> Payment(int id)
        {
            var ve = await _context.Ves.Include(c => c.Chuyen).ThenInclude(x => x.Xe)
               .Include(g => g.Ghe)
               .FirstOrDefaultAsync(v => v.Id == id);
            return View(ve);
        }
        [HttpPost]
        public async Task<IActionResult> Payment(int id,string email,string paymentmethod)
        {

            if (id != null)
            {
                var ve = await _context.Ves.Include(c => c.Chuyen).ThenInclude(x => x.Xe).ThenInclude(l => l.LoaiXe)
                   .Include(g => g.Ghe)
                   .FirstOrDefaultAsync(v => v.Id == id);
                if (paymentmethod == null)
                {
                    TempData["ErrorrMessage"] = "Chưa chọn phương thức thanh toán";
                    return View(ve);
                }
                ve.TrangThai = TrangThaiVe.Booked;
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();
                if (email != null)
                {
                    string body =  _emailService.makeBodyTicketBooked(ve);
                    await _emailService.SendEmailAsync(email, "Xác nhận vé xe ", body);
                }
                TempData["SuccessMeassge"] = "Thanh toán thành công! Nhắc khách hàng kiẻm tra email";
                return View(ve);
            }
            TempData["ErrorrMessage"] = "Vé không tồn tại";
            return View();
        }
        public async Task<IActionResult> Ve(int id)
        {
            // Lấy danh sách vé cho chuyến này
            var ve = await _context.Ves.Include(c =>  c.Chuyen).ThenInclude(x => x.Xe)
                .Include(g => g.Ghe)
                .FirstOrDefaultAsync(v => v.Id == id);
            return View(ve);
        }
        [HttpPost]
        [Route("Ve/Cancel/{id}")]
        public async Task<ActionResult> Cancel(int id)
        {
            Ve? ve = await _context.Ves
                   .Include(g => g.Ghe)
                   .FirstOrDefaultAsync(v => v.Id == id);
            try
            {
               
                if (ve != null)
                {
                    ve.TrangThai = TrangThaiVe.Cancel;
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
                    TempData["SuccessMessage"] = $"Đã hủy vé của {ve.Ten ?? "Người dùng không xác định"}";
                    return View(ve);
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

            return View(ve);
        }

        public async Task<ActionResult> Chuyens(int id) 
        {
            Chuyen? chuyen = await _context.Chuyens.Include(t => t.Tuyen).Include(x => x.Xe).FirstOrDefaultAsync(c => c.Id == id);
            ViewBag.Ves = await _context.Ves
                 .Include(c => c.Chuyen)
                 .Include(g => g.Ghe)
                 .Where(v => v.Chuyen.Id == chuyen.Id)
                 .ToListAsync();
            return  View(chuyen);
        }
        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
