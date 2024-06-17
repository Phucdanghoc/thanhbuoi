using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;
using ThanhBuoi.Services;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN,SALER,USER")]
    public class BookingController : Controller
    {
        private const int PageSize = 10;
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly MomoServices _momoServices;
        private readonly IEmailService _emailService;
        private readonly DataContext _context;

        public BookingController(DataContext context, UserManager<TaiKhoan> userManager, IEmailService emailService, MomoServices momoServices)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _momoServices = momoServices;
        }

        // GET: BookingController
        public IActionResult Index(int page = 1, string from = null, string to = null, DateTime? datetime = null, string searchString = null, string type = null)
        {
            try
            {
                var listChuyen = GetPaginatedChuyens(page, from, to, datetime, searchString, type);
                ViewBag.listTinh = TinhData.GetInstance().GetTinhThanh();
                return View(listChuyen);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách chuyến. Vui lòng thử lại sau.";
                return View();
            }
        }

        private IEnumerable<Chuyen> GetPaginatedChuyens(int page, string from = null, string to = null, DateTime? datetime = null, string searchString = null, string type = null)
        {
            var query = _context.Chuyens
                .Include(x => x.Xe).ThenInclude(l => l.LoaiXe)
                .Include(t => t.Tuyen).ThenInclude(t => t.DiemDen)
                .Where(c => c.ThoiGianDi > DateTime.Now)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = type == "N" ? query.Where(c => c.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.Ngoi) : query.Where(c => c.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.GiuongNam);
            }
            if (!string.IsNullOrEmpty(from))
            {
                query = query.Where(c => c.Tuyen.DiemDi.Ten.Contains(from));
            }
            if (!string.IsNullOrEmpty(to))
            {
                query = query.Where(c => c.Tuyen.DiemDen.Ten.Contains(to));
            }
            if (datetime.HasValue)
            {
                query = query.Where(c => c.ThoiGianDi.Date == datetime.Value.Date);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Ten.ToLower().Contains(searchString.ToLower()));
            }

            int startIndex = (page - 1) * PageSize;
            var listChuyen = query.OrderByDescending(c => c.ThoiGianDi).Skip(startIndex).Take(PageSize).ToList();
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
                List<string> codeTickets = new();
                foreach (var ve in veList)
                {
                    var existingVe = await _context.Ves.Include(c => c.Chuyen).ThenInclude(x => x.Xe).FirstOrDefaultAsync(v => v.Id == ve.Id);
                    if (existingVe != null)
                    {
                        UpdateVeDetails(existingVe, ve);
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

        private void UpdateVeDetails(Ve existingVe, Ve newVeDetails)
        {
            existingVe.Ten = newVeDetails.Ten;
            existingVe.CMND = newVeDetails.CMND;
            existingVe.Sdt = newVeDetails.Sdt;
            existingVe.MaVe = $"TB-{existingVe.Chuyen.ThoiGianDi.Day}-{existingVe.Chuyen.Xe.MaXe}{existingVe.Id}";
            existingVe.TrangThai = TrangThaiVe.Waiting;
            existingVe.TaiKhoan = _userManager.GetUserAsync(HttpContext.User).Result;
            existingVe.NgayTao = DateTime.Now;
            existingVe.Hanhli = newVeDetails.Hanhli;
            existingVe.Tien = CalculateTien(existingVe);
        }

        private double CalculateTien(Ve ve)
        {
            double basePrice = (double)(ve.Chuyen.Gia + ve.Chuyen.Gia * ve.Chuyen.GiaTang);
            if (ve.Hanhli > 20)
            {
                basePrice += Math.Round((ve.Hanhli - 20) * 0.1 * ve.Chuyen.Gia);
            }
            return basePrice;
        }

        [HttpGet]
        public async Task<IActionResult> Payment(List<string> codetickets)
        {
            try
            {
                var veList = await GetVesByMaVe(codetickets);
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

        private async Task<List<Ve>> GetVesByMaVe(List<string> codetickets)
        {
            List<Ve> veList = new();
            foreach (var code in codetickets)
            {
                var ve = await _context.Ves.Include(c => c.Chuyen).ThenInclude(x => x.Xe).Include(g => g.Ghe).FirstOrDefaultAsync(v => v.MaVe == code);
                if (ve != null)
                {
                    veList.Add(ve);
                }
            }
            return veList;
        }

        [HttpPost]
        public IActionResult SubmitTickets(string selectedTickets)
        {
            if (string.IsNullOrEmpty(selectedTickets))
            {
                return RedirectToAction("DatVe");
            }
            var ticketIds = selectedTickets.Split(',').Select(int.Parse).ToList();
            return RedirectToAction("Ve", new { ticketIds });
        }

        [HttpPost]
        public async Task<IActionResult> Payment(string email, string paymentMethod, List<int> codetickets, string mota)
        {
            if (!codetickets.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy vé để thanh toán.";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(paymentMethod))
            {
                TempData["ErrorMessage"] = "Chưa chọn phương thức thanh toán";
                return RedirectToAction("Payment", new { codetickets });
            }

            var tickets = await GetTicketsByIds(codetickets);
            if (!tickets.Any())
            {
                TempData["ErrorMessage"] = "Không tìm thấy vé để thanh toán.";
                return RedirectToAction("Index", "Home");
            }

            var donhang = await CreateDonHangAsync(email, paymentMethod, mota, tickets);
            double cost = tickets.Sum(t => t.Tien);
            await _context.SaveChangesAsync();

            if (paymentMethod == "momo")
            {
                var momoPaymentResponse = await _momoServices.Pay(new PaymentDTO
                {
                    cost = cost.ToString(),
                    url = "https://localhost:7273/Ve/Detail/" + donhang.Id
                });

                donhang.RequestId = momoPaymentResponse.RequestId;
                donhang.Id_momoRes = momoPaymentResponse.OrderId;
                _context.DonHangs.Update(donhang);
                await _context.SaveChangesAsync();
                return Redirect(momoPaymentResponse.PayUrl);
            }
            else
            {
                return RedirectToAction("Detail", "Ve", new { id = donhang.Id });
            }

        }

        private async Task<List<Ve>> GetTicketsByIds(List<int> ticketIds)
        {
            return await _context.Ves
                .Include(c => c.Chuyen).ThenInclude(x => x.Xe).ThenInclude(l => l.LoaiXe)
                .Include(g => g.Ghe)
                .Where(v => ticketIds.Contains(v.Id))
                .ToListAsync();
        }

        private async Task<DonHang> CreateDonHangAsync(string email, string paymentMethod, string mota, List<Ve> tickets)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            double cost = tickets.Sum(t => t.Tien);
            var donhang = new DonHang
            {
                email = email,
                TaiKhoan = currentUser,
                PhuongThucThanhToan = paymentMethod,
                Tien = cost,
                MaDon = $"{tickets[0].Chuyen.Id}{int.Parse(DateTime.Now.ToString("MMddHHmmss"))}",
                NgayTao = DateTime.Now,
                Trangthai = TrangThaiDonHang.Payment,
                Mota = mota
            };



            foreach (var ve in tickets)
            {
                ve.TrangThai = TrangThaiVe.Booked;
                ve.email = email;

                var donhangChitiet = new DonHangChiTiet
                {
                    DonHang = donhang,
                    Ve = ve,
                    Tien = ve.Tien
                };

                _context.DonHangChiTiets.Add(donhangChitiet);
                _context.Ves.Update(ve);
            }

            if (!string.IsNullOrEmpty(email))
            {
                var emailBody = _emailService.makeBodyTicketBooked(tickets);
                await _emailService.SendEmailAsync(email, "Xác nhận vé xe", emailBody);
            }

            return donhang;
        }

        private async Task ProcessPayment(string paymentMethod, DonHang donhang, List<Ve> tickets)
        {
            double cost = tickets.Sum(t => t.Tien);

            if (paymentMethod == "momo")
            {
                var momoPaymentResponse = await _momoServices.Pay(new PaymentDTO
                {
                    cost = cost.ToString(),
                    url = "https://localhost:7273/Booking"
                });

                donhang.RequestId = momoPaymentResponse.RequestId;
                donhang.Id_momoRes = momoPaymentResponse.OrderId;

                await _context.SaveChangesAsync();
                Redirect(momoPaymentResponse.PayUrl);
            }
            else
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> Ve(List<int> ticketIds)
        {
            var tickets = await GetTicketsByIds(ticketIds);

            if (tickets.Any(t => t.TrangThai == TrangThaiVe.Booked))
            {
                TempData["ErrorMessage"] = "Đã có vé được thanh toán";
                return RedirectToAction("Index", "Booking");
            }

            return View(tickets);
        }

        public async Task<ActionResult> Chuyens(int id)
        {
            var chuyen = await _context.Chuyens
                .Include(t => t.Tuyen)
                .Include(x => x.Xe).ThenInclude(l => l.LoaiXe)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chuyen == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chuyến.";
                return RedirectToAction("Index", "Booking");
            }

            var vesList = await _context.Ves
                .Include(c => c.Chuyen)
                .Include(g => g.Ghe).ThenInclude(h => h.Hang).ThenInclude(t => t.Tang)
                .Where(v => v.Chuyen.Id == chuyen.Id)
                .ToListAsync();

            var listLeft = new List<Ve>();
            var listRight = new List<Ve>();

            foreach (var item in vesList)
            {
                if (chuyen.Xe.LoaiXe.LoaiGheXe == LoaiGheXe.Ngoi)
                {
                    if (item.Ghe.STT <= 4) listLeft.Add(item);
                    else listRight.Add(item);
                }
                else
                {
                    if (item.Ghe.Hang.Tang.STT == 1) listLeft.Add(item);
                    else listRight.Add(item);
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
            var ves = await _context.Ves
                .Where(v => codetickets.Contains(v.Id))
                .ToListAsync();

            if (!ves.Any())
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

                await _context.SaveChangesAsync();

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


