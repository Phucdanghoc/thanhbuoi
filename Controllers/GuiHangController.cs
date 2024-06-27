using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using ThanhBuoi.APIS;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;
using ThanhBuoi.Services;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN,SALER,USER")]
    public class GuiHangController : Controller
    {
        UserManager<TaiKhoan> _userManager;
        Dictionary<string, double> dictGiaHang = new Dictionary<string, double>();
        private readonly DataContext _context;
        private IEmailService _emailService;
        private MomoServices _momoServices;
        public GuiHangController(DataContext context, UserManager<TaiKhoan> userManager,IEmailService emailSerive,MomoServices momoService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailSerive;
            _momoServices = momoService;
            // Các dạng hàng ....
            dictGiaHang.Add("Hàng nhỏ ( 1 - 5 kg)", 0.2);
            dictGiaHang.Add("Hàng vừa ( 5 - 10 kg)", 0.4);
            dictGiaHang.Add("Hàng to (10 - 20 kg )", 0.6);
            dictGiaHang.Add("Hàng đặc biệt lớn ( hơn 20 kg)", 1);

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
            List<DTOHang> list = new List<DTOHang>();
            list.Add(new DTOHang());
            ViewBag.loaihang = new List<string>(dictGiaHang.Keys);
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Create(List<DTOHang> list)
        {
            var chuyen = await _context.Chuyens.FirstOrDefaultAsync(c => c.Id == list[0].IdChuyen);
            List<HangGui> hangGuis = new List<HangGui>();
            List<DonHangChiTiet> donHangChiTiets = new List<DonHangChiTiet>();

            DonHang donHang = new DonHang
            {
                NgayTao = DateTime.Now,
                TaiKhoan = await _userManager.GetUserAsync(HttpContext.User),
                Trangthai = TrangThaiDonHang.Waiting,
                MaDon = $"DH{int.Parse(DateTime.Now.ToString("MMddHHmmss"))}"
            };
            foreach (var hang in list)
            {
                hang.NgayTao = DateTime.Now;
                hang.TrangThai = TrangThaiHang.Waiting;
                hang.Chuyen = chuyen;
/*                hang.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
*/                double basePrice = (double)(hang.Chuyen.Gia * (1 + hang.Chuyen.GiaTang));
                double finalPrice = basePrice + (basePrice * dictGiaHang[hang.LoaiHang]);
                hang.Tien = finalPrice;
                DonHangChiTiet donHangChiTiet = new DonHangChiTiet
                {
                    DonHang = donHang,
                    HangGui = hang,
                    Tien = hang.Tien
                };
                hangGuis.Add(hang);
                donHangChiTiets.Add(donHangChiTiet);
            }
            donHang.Tien = list.Sum(hang => hang.Tien);

            HttpContext.Session.SetString("ListHangGui", JsonSerializer.Serialize(hangGuis));
            HttpContext.Session.SetString("ListDonHangChiTiet", JsonSerializer.Serialize(donHangChiTiets));
            HttpContext.Session.SetString("DonHang", JsonSerializer.Serialize(donHang));
            ViewBag.loaihang = new List<string>(dictGiaHang.Keys);

            return RedirectToAction("Payment");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var ListHangGui = HttpContext.Session.GetString("ListHangGui");
            var ListDonHangChiTiet = HttpContext.Session.GetString("ListDonHangChiTiet");
            var DonHang = HttpContext.Session.GetString("DonHang");
            List<HangGui> hangGuis = new List<HangGui>();
            List<DonHangChiTiet> donHangChiTiets = new List<DonHangChiTiet>();
            DonHang donHang = new DonHang();
            if (ListHangGui != null && ListDonHangChiTiet != null && DonHang != null)
            {
                hangGuis = JsonSerializer.Deserialize<List<HangGui>>(ListHangGui);
                donHangChiTiets = JsonSerializer.Deserialize<List<DonHangChiTiet>>(ListDonHangChiTiet);
                donHang = JsonSerializer.Deserialize<DonHang>(DonHang);
                ViewBag.listChitietDonhang = donHangChiTiets;
                View(donHang);
            }
            else
            {
                TempData["ErrorMessage"] = "Không có đơn hàng nào cần thanh toán";
                return View();
            }

            return View();
        }
        public HangGui CreateHangGui(HangGui oldHangGui)
        {
            var chuyen = _context.Chuyens.FirstOrDefault(c => c.Id == oldHangGui.Chuyen.Id);

            HangGui combinedHangGui = new HangGui
            {
                Id = oldHangGui.Id,
                TaiKhoan = _userManager.GetUserAsync(HttpContext.User).Result,
                Chuyen = chuyen,
                TenHang = oldHangGui.TenHang,
                TenNguoiGui = oldHangGui.TenNguoiGui,
                SdtNguoiGui = oldHangGui.SdtNguoiGui,
                TenNguoiNhan = oldHangGui.TenNguoiNhan,
                SdtNguoiNhan = oldHangGui.SdtNguoiNhan,
                DiaChiNguoiGui = oldHangGui.DiaChiNguoiGui,
                DiachiNguoiNhan = oldHangGui.DiachiNguoiNhan,
                TrangThai = oldHangGui.TrangThai,
                NgayTao = DateTime.Now,
                LoaiHang = oldHangGui.LoaiHang
            };

            return combinedHangGui;
        }
        [HttpPost]
        public async Task<ActionResult> Payment(string Mota, string PhuongThucThanhToan, string Email)
        {
            var ListHangGui = HttpContext.Session.GetString("ListHangGui");
            var ListDonHangChiTiet = HttpContext.Session.GetString("ListDonHangChiTiet");
            var DonHangSession = HttpContext.Session.GetString("DonHang");

            if (ListHangGui == null || ListDonHangChiTiet == null || DonHangSession == null)
            {
                TempData["ErrorMessage"] = "Không có đơn hàng nào cần thanh toán";
                return View();
            }

            var hangGuis = JsonSerializer.Deserialize<List<HangGui>>(ListHangGui);
            var donHangChiTiets = JsonSerializer.Deserialize<List<DonHangChiTiet>>(ListDonHangChiTiet);
            var donHang = JsonSerializer.Deserialize<DonHang>(DonHangSession);

            donHang.PhuongThucThanhToan = PhuongThucThanhToan;
            donHang.Mota = Mota;
            donHang.Email = Email ?? "";
            donHang.Trangthai = TrangThaiDonHang.Cod;

            try
            {
                DonHang newDonhang = new DonHang
                {
                    PhuongThucThanhToan = PhuongThucThanhToan,
                    Mota = Mota,
                    Email = Email,
                    Tien = donHang.Tien,
                    TaiKhoan = _userManager.GetUserAsync(HttpContext.User).Result,
                    Trangthai = TrangThaiDonHang.Payment,
                    MaDon = donHang.MaDon,
                    NgayTao = donHang.NgayTao,

                };
                List<HangGui> hangs = new List<HangGui>();
;                foreach (var hangGui in hangGuis)
                {
                    HangGui hanggui = CreateHangGui(hangGui);
                    DonHangChiTiet donHangChiTiet = new DonHangChiTiet
                    {
                        DonHang = newDonhang,
                        HangGui = hanggui,
                        Tien = donHang.Tien,

                    };
                    hangs.Add(hanggui);
                    _context.HangGuis.Add(hanggui);
                    _context.DonHangChiTiets.Add(donHangChiTiet);

                };
                _context.DonHangs.Add(newDonhang);
                await _context.SaveChangesAsync();

                if (donHang.Email != null)
                {
                    string bodyEmail = _emailService.makeBodyEmailOrder(donHang, hangs);
                    await _emailService.SendEmailAsync(donHang.Email, "Xác nhận đơn hàng", bodyEmail);
                }

                if (PhuongThucThanhToan == "momo")
                {
                    MomoPaymentResponseDTO momoPaymentResponseDTO = await _momoServices.Pay(new PaymentDTO
                    {
                        url = $"https://localhost:7273/Donhangs/Details/{newDonhang.Id}",
                        cost = donHang.Tien.ToString(),
                    });

                    if (momoPaymentResponseDTO.ResultCode == 0)
                    {
                        donHang.RequestId = momoPaymentResponseDTO.RequestId;
                        donHang.Id_momoRes = momoPaymentResponseDTO.OrderId;
                        await _context.SaveChangesAsync(); 
                        return Redirect(momoPaymentResponseDTO.PayUrl);
                    }
                }

                return RedirectToAction("Details", "Donhangs", new { id = newDonhang.Id });
            }
            catch (DbUpdateConcurrencyException e)
            {
                return BadRequest(e.Message);
            }
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
