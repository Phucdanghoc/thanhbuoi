using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;
using ThanhBuoi.Services;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN,SALER")]
    public class BookingController : Controller
    {
        private const int PageSize = 10;
        UserManager<TaiKhoan> _userManager;
        private readonly MomoServices _momoServices;
        private IEmailService _emailService;
        private List<int> _listIdBooked = new List<int>();
        private readonly DataContext _context;
        public BookingController(DataContext context, UserManager<TaiKhoan> userManager,IEmailService emailService,MomoServices momoServices) {
            _context = context;
            _momoServices = momoServices;
            _userManager = userManager;
            _emailService = emailService;

        }
        // GET: BookingController
        public IActionResult Index(int page = 1, string from = null, string to = null, DateTime? datetime = null, string searchString = null, string type = null)
        {
            try
            {
                var listChuyen = GetPaginatedChuyens(page, from,to,datetime,searchString,type);
                return View(listChuyen);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách chuyến. Vui lòng thử lại sau.";
                return View();
            }
        }

        private IEnumerable<Chuyen> GetPaginatedChuyens(int page, string from = null, string to = null, DateTime? datetime = null, string searchString = null,string type = null)
        {
            var query = _context.Chuyens
                .Include(x => x.Xe)
                    .ThenInclude(l => l.LoaiXe)
                .Include(t => t.Tuyen)
                    .ThenInclude(t => t.DiemDen)
                .Include(t => t.Tuyen)
                    .ThenInclude(t => t.DiemDen)
                .OrderByDescending(c => c.ThoiGianDi)
                .Where(c => c.ThoiGianDi > DateTime.Now)
                .AsQueryable();
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "N")
                {
                    query = query.Where(c => c.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.Ngoi);

                }
                else
                {
                    query = query.Where(c => c.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.GiuongNam);

                }
            }
            if (!string.IsNullOrEmpty(from))
            {
                query = query.Where(c => c.Tuyen.DiemDi.Ten.Contains(from));
            }

            if (!string.IsNullOrEmpty(to))
            {
                query = query.Where(c => c.Tuyen.DiemDen.Ten.Contains(to));
            }

            if (datetime != null)
            {
                query = query.Where(c => c.ThoiGianDi.Date == datetime.Value.Date);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Ten.ToLower().Contains(searchString.ToLower()));
            }

            int startIndex = (page - 1) * PageSize;
            var listChuyen = query.Skip(startIndex).Take(PageSize).ToList();

            int totalChuyens = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalChuyens / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listChuyen;
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
                        existingVe.NgayTao = DateTime.Now;
                        existingVe.Hanhli = ve.Hanhli;

                        if (existingVe.Hanhli > 20)
                        {
                            existingVe.Tien = (double)(existingVe.Chuyen.Gia +
                                              Math.Round((existingVe.Hanhli - 20) * 0.1 * existingVe.Chuyen.Gia) +
                                              (existingVe.Chuyen.Gia * existingVe.Chuyen.GiaTang));
                        }
                        else
                        {
                            existingVe.Tien = (double)(existingVe.Chuyen.Gia +
                                              existingVe.Chuyen.Gia * existingVe.Chuyen.GiaTang);
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
            if (paymentMethod == "momo")
            {
                MomoPaymentResponseDTO momoPaymentResponseDTO = await _momoServices.Pay(new PaymentDTO
                {
                    cost = tickets.Sum(t => t.Tien).ToString(),
                    url = "https://localhost:7273/Booking"
                });
                return Redirect(momoPaymentResponseDTO.ShortLink);
            }
            return RedirectToAction("Index", "Home");


        }

        public async Task<IActionResult> Ve(List<int> ticketIds)
        {
            var tickets = await _context.Ves
                                 .Include(c => c.Chuyen).ThenInclude(x => x.Xe)
                                 .Include(g => g.Ghe)
                                 .Where(v => ticketIds.Contains(v.Id))
                                 .ToListAsync();
            foreach (var item in tickets)
            {
                if (item.TrangThai == TrangThaiVe.Booked)
                {
                    TempData["ErrorMessage"] = "Đã có vé được thanh toán";
                    return RedirectToAction("Index", "Booking");
                }
            }
            return View(tickets);
        }
        
        public async Task<ActionResult> Chuyens(int id) 
        {
            Chuyen? chuyen = await _context.Chuyens
                   .Include(t => t.Tuyen)
                   .Include(x => x.Xe)
                   .ThenInclude(l => l.LoaiXe)
                   .FirstOrDefaultAsync(c => c.Id == id);


            List<Ve> vesList = await _context.Ves
                .Include(c => c.Chuyen)
                .Include(g => g.Ghe)
                .ThenInclude(h => h.Hang).ThenInclude(t=>t.Tang)
                .Where(v => v.Chuyen.Id == chuyen.Id)
                .ToListAsync();

            int vesCount = vesList.Count;
            int halfCount = vesCount / 2;
            List<Ve> listLeft = [];
            List<Ve> listRight = [];
            if (chuyen.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.Ngoi)
            {
                foreach (var item in vesList)
                {
                    if (item.Ghe.STT <= 4)
                    {
                        listLeft.Add(item);
                    }
                    else
                    {
                        listRight.Add(item);
                    }
                }

            }
            else
            {
                foreach (var item in vesList)
                {
                    if (item.Ghe.Hang.Tang.STT == 1)
                    {
                        listLeft.Add(item);
                    }
                    else
                    {
                        listRight.Add(item);
                    }
                }
            }

            ViewBag.FirstHalfVes = listLeft;
            ViewBag.SecondHalfVes = listRight;

            return View(chuyen);
        }
        [HttpPost]
        [Route("Cancel")]
        public async Task<ActionResult> Cancel(List<int> codetickets)
        {
            List<Ve> ves = await _context.Ves
                                    .Where(v => codetickets.Contains(v.Id))
                                    .ToListAsync();

            if (ves == null || !ves.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy vé để hủy.";
                return RedirectToAction("Index", "Booking");
            }

            try
            {
                foreach (var ve in ves)
                {
                    ve.TrangThai = TrangThaiVe.Empty;
                    ve.Ten = null;
                    ve.CMND = null;
                    ve.Sdt = null;
                    ve.MaVe = null;
                    ve.TaiKhoan = null;
                    ve.Hanhli = 0;
                    ve.Tien = 0;
                    _context.Ves.Update(ve);
                }

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Đã hủy vé thành công.";
                return RedirectToAction("Index", "Booking");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi hủy vé: {ex.Message}";
                return RedirectToAction("Index", "Booking");
            }
        }


    }
}
