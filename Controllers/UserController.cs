using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "USER")]
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly int PageSize = 10;
        private readonly UserManager<TaiKhoan> _userManager;
        public UserController(DataContext dbContext, UserManager<TaiKhoan> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }
        public async Task<IActionResult> DonHang(int page = 1, string searchString = null)
        {
            try
            {
                var listDonHang = await GetPaginatedDonHangsAsync(page, searchString);
                return View(listDonHang);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách đơn hàng. Vui lòng thử lại sau.";
                return View();
            }
        }

        public async Task<IActionResult> Ve(int page = 1, string searchString = null)
        {
            try
            {
                var listve = await GetPaginatedVesAsync(page, searchString);
                return View(listve);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách đơn hàng. Vui lòng thử lại sau.";
                return View();
            }
        }

        private async Task<IEnumerable<DonHang>> GetPaginatedDonHangsAsync(int page, string searchString = null)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

             var query = _context.DonHangs
               .Include(dh => dh.DonHangChiTiets)
                              .ThenInclude(dhct => dhct.HangGui).Where(t => t.TaiKhoan == currentUser)
                              .Where(dh => dh.DonHangChiTiets.Any(dhct => dhct.HangGui != null))
                              .AsQueryable();
           


            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => d.PhuongThucThanhToan.Contains(searchString) ||
                                          d.Mota.Contains(searchString) ||
                                          d.email.Contains(searchString));
            }

            int startIndex = (page - 1) * PageSize;
            var listDonHang = query.OrderByDescending(d => d.NgayTao)
                                   .Skip(startIndex)
                                   .Take(PageSize)
            .ToList();

            int totalDonHangs = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalDonHangs / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listDonHang;
        }
        private async Task<IEnumerable<Ve>> GetPaginatedVesAsync(int page, string searchString = null)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var query = _context.Ves.Include(c => c.Chuyen).ThenInclude(x => x.Xe).Include(t => t.TaiKhoan).Where(v => v.TaiKhoan == currentUser);


            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => d.Chuyen.Ten.Contains(searchString) ||
                                          d.email.Contains(searchString));
            }

            int startIndex = (page - 1) * PageSize;
            var listDonHang = query.OrderByDescending(d => d.NgayTao)
                                   .Skip(startIndex)
                                   .Take(PageSize)
            .ToList();

            int totalDonHangs = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalDonHangs / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listDonHang;
        }
        // GET: DonHangs/Details/5
        [HttpGet]
        public async Task<IActionResult> DetailDonHang(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var donHang = await _context.DonHangs
                                        .Include(dh => dh.DonHangChiTiets)
                                        .ThenInclude(dhct => dhct.HangGui)
                                        .FirstOrDefaultAsync(d => d.Id == id);

            if (donHang == null) return NotFound();

            ViewBag.listChitietDonhang = donHang.DonHangChiTiets.ToList();

            return View(donHang);
        }
        [HttpGet]
        public async Task<IActionResult> DetailVe(int id)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var ve = await _context.Ves
                                   .Include(v => v.Chuyen).ThenInclude(c => c.Xe).ThenInclude(l => l.LoaiXe)
                                   .Include(v => v.Ghe).ThenInclude(h => h.Hang)
                                   .FirstOrDefaultAsync(v => v.Id == id && v.TaiKhoan == currentUser);
            if (ve == null)
            {
                return NotFound();
            }

            return View(ve);
        }
    }
}
