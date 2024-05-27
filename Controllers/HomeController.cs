using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    [Authorize]
    public class HomeController : Controller
        {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<TaiKhoan> _singManager;
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger,SignInManager<TaiKhoan> signInManager, UserManager<TaiKhoan> _userManager,DataContext dataContext)

        {
            _context = dataContext;
            _logger = logger;
            _singManager = signInManager;
            _userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Chuyens = await _context.Chuyens.Include(c => c.Xe).ThenInclude(l => l.LoaiXe)
                .Where(c => c.ThoiGianDi.Date == DateTime.Today)
                .ToListAsync();
            ViewBag.ListVe = await _context.Ves.Include(v => v.Chuyen).Where(v => v.NgayTao.Date == DateTime.Today && v.TrangThai == TrangThaiVe.Booked).ToListAsync();
            ViewBag.DonHang = await _context.DonHangs.Where(v => v.NgayTao.Date == DateTime.Today).ToListAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
