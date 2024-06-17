using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN,SALER,USER")]
    public class GuiHangController : Controller
    {
        UserManager<TaiKhoan> _userManager;
        Dictionary<string, double> dictGiaHang = new Dictionary<string, double>();
        private readonly DataContext _context;
        public GuiHangController(DataContext context, UserManager<TaiKhoan> userManager)
        {
            _context = context;
            _userManager = userManager;
            dictGiaHang.Add("Xe tay ga", 0.7);
            dictGiaHang.Add("Xe số", 0.3);
            dictGiaHang.Add("Hàng nhỏ", 0.2);
            dictGiaHang.Add("Hàng đặc biệt lớn", 1);

        }
        // GET: GuiHang
        public ActionResult Index()
        {
            ViewBag.loaihang = new List<string>(dictGiaHang.Keys);
            return View();
        }


        // GET: GuiHang/Create
        public ActionResult Create()
        {
            ViewBag.loaihang = new List<string>(dictGiaHang.Keys);
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> DonHang(int id)
        {
            DonHang? donHang = await _context.DonHangs.FirstOrDefaultAsync(d => d.Id == id);
            var listChitietDonhang = await _context.DonHangChiTiets
                                                .Include(d => d.HangGui)
                                                .Where(c => c.DonHang.Id == id)
                                                .ToListAsync();
            ViewBag.listChitietDonhang = listChitietDonhang;
            return View(donHang);
        }

    }
}
