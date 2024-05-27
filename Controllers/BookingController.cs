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
        private List<int> _listIdBooked = new List<int>();
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
            var query = _context.Chuyens.Include(x => x.Xe).OrderByDescending(c => c.ThoiGianDi).Where(c => c.ThoiGianDi >= DateTime.Now).AsQueryable();

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
        public async Task<ActionResult> BookTickets(List<Ve> veList)
        {
            try
            {
                List<String> codeTickets = [];
                foreach (var ve in veList)
                {
                    Ve? existingVe = await _context.Ves
                        .Include(g => g.Ghe)
                        .Include(c => c.Chuyen)
                        .ThenInclude(x => x.Xe)
                        .FirstOrDefaultAsync(v => v.Id == ve.Id);

                    if (existingVe != null)
                    {
                        existingVe.Ten = ve.Ten;
                        existingVe.CMND = ve.CMND;
                        existingVe.Sdt = ve.Sdt;
                        existingVe.MaVe = $"TB-{existingVe.Chuyen.ThoiGianDi.Day}-{existingVe.Chuyen.Xe.MaXe}{existingVe.Id}";
                        existingVe.TrangThai = TrangThaiVe.Waiting;
                        existingVe.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
                        existingVe.Ghe.KhoangTrong = false;
                        existingVe.NgayTao = DateTime.Now;
                        existingVe.Hanhli = ve.Hanhli;

                        if (existingVe.Hanhli > 20)
                        {
                            existingVe.Tien = existingVe.Chuyen.Gia + Math.Round((existingVe.Hanhli - 20) * 0.1 * existingVe.Chuyen.Gia);
                        }
                        else
                        {
                            existingVe.Tien = existingVe.Chuyen.Gia;
                        }

                        _context.Chuyens.Update(existingVe.Chuyen);
                        _context.Ves.Update(existingVe);
                        codeTickets.Add(existingVe.MaVe);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Payment", "Booking", new { codeTickets });
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Ve", "Booking");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Payment(List<string> codetickets)
        {
            List<Ve> veList = new List<Ve>();
            try
            {
                foreach (var code in codetickets)
                {
                    var ve = await _context.Ves
                        .Include(c => c.Chuyen)
                        .ThenInclude(x => x.Xe)
                        .Include(g => g.Ghe)
                        .FirstOrDefaultAsync(v => v.MaVe == code);

                    if (ve != null)
                    {
                        veList.Add(ve);
                    }
                }
                if (veList.Count == 0)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin vé.";
                    return RedirectToAction("Index", "Home");
                }

                var firstChuyenId = veList.First().Chuyen.Id;
                if (veList.Any(ve => ve.Chuyen.Id != firstChuyenId))
                {
                    TempData["ErrorMessage"] = "Vé không thuộc cùng một chuyến.";
                    return RedirectToAction("Index", "Home");
                }
                if (veList.Any(ve => ve.TrangThai == TrangThaiVe.Booked))
                {
                    TempData["ErrorMessage"] = "Một hoặc nhiều vé đã được thanh toán.";
                    return RedirectToAction("Index", "Booking");
                }

                return View(veList);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xử lý yêu cầu: " + e.Message;
                return RedirectToAction("Index", "Booking");
            }
        }

        [HttpPost]
        public IActionResult SubmitTickets(string selectedTickets)
        {
            if (string.IsNullOrEmpty(selectedTickets))
            {
                return RedirectToAction("DatVe");
            }
            var ticketIds = selectedTickets.Split(',').Select(int.Parse).ToList();
            return RedirectToAction("Ve" ,new { ticketIds } );
        }
        [HttpPost]
        public async Task<IActionResult> Payment(string email, string paymentMethod, List<int> codetickets)
        {
            if (codetickets == null || !codetickets.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy vé để thanh toán.";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(paymentMethod))
            {
                TempData["ErrorMessage"] = "Chưa chọn phương thức thanh toán";
                return RedirectToAction("Payment", new { codetickets });
            }

            var tickets = await _context.Ves
                .Include(c => c.Chuyen).ThenInclude(x => x.Xe).ThenInclude(l => l.LoaiXe)
                .Include(g => g.Ghe)
                .Where(v => codetickets.Contains(v.Id))
                .ToListAsync();

            if (!tickets.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy vé để thanh toán.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var ve in tickets)
            {
                ve.TrangThai = TrangThaiVe.Booked;
                ve.email = email;
                _context.Ves.Update(ve);
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(email))
            {
                var emailBody = _emailService.makeBodyTicketBooked(tickets);
                await _emailService.SendEmailAsync(email, "Xác nhận vé xe", emailBody);
            }

            TempData["SuccessMessage"] = "Thanh toán thành công! Nhắc khách hàng kiểm tra email";
            return RedirectToAction("Index", "Booking");
        }

        public async Task<IActionResult> Ve(List<int> ticketIds)
        {
            var tickets = await _context.Ves
                                 .Include(c => c.Chuyen).ThenInclude(x => x.Xe)
                                 .Include(g => g.Ghe)
                                 .Where(v => ticketIds.Contains(v.Id))
                                 .ToListAsync();

            return View(tickets);
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

    }
}
