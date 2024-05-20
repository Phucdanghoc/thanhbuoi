using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;

namespace ThanhBuoi.Controllers
{
    public class GuiHangController : Controller
    {
        UserManager<TaiKhoan> _userManager;

        Dictionary<string, double> dictGiaHang = new Dictionary<string, double>();
        private readonly DataContext _context;
        public GuiHangController(DataContext context, UserManager<TaiKhoan> userManager)
        {
            _context = context;
            _userManager = userManager;
            dictGiaHang.Add("Xe tay ga ", 0.7);
            dictGiaHang.Add("Xe số ", 0.3);
            dictGiaHang.Add("Hàng nhỏ", 0.2);
            dictGiaHang.Add("Hàng đặc biệt lớn", 1);

        }
        // GET: GuiHang
        public ActionResult Index()
        {
            ViewBag.loaihang = new List<string>(dictGiaHang.Keys);
            return View();
        }

        // GET: GuiHang/Details/5
        public ActionResult Details(int id)
        {
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



        // POST: GuiHang/Create
        /* [HttpPost]
         public async Task<ActionResult<List<DTOHang>>> Create( List<DTOHang> hangGuis)
         {
             if (hangGuis == null || hangGuis.Count == 0)
             {
                 TempData["ErrorMessage"] = "Không có hàng nào được tạo";
                 return BadRequest("No HangGui objects were provided.");
             }
             else
             {
                 foreach (var hang in hangGuis)
                 {
                     hang.NgayTao = DateTime.Now;
                     hang.TrangThai = TrangThaiHang.Waiting;
                     hang.Chuyen = await _context.Chuyens.FirstOrDefaultAsync(c => c.Id == hang.IdChuyen);
                     hang.Tien = ((double)(hang.Chuyen.Gia * hang.Chuyen.GiaTang)) * dictGiaHang[hang.LoaiHang];
                     hang.TaiKhoan = _userManager;
                 }
             }
             try
             {
                 *//*       _context.HangGuis.AddRange(hangGuis);
                        await _context.SaveChangesAsync();*//*
                 return RedirectToAction("Confirm", new { hangGuis });
             }
             catch (Exception ex)
             {
                 return StatusCode(500, $"An error occurred while saving the HangGui objects: {ex.Message}");
             }
         }*/


        // GET: GuiHang/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GuiHang/Edit/5
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

        // GET: GuiHang/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GuiHang/Delete/5
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
