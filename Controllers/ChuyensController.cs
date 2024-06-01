using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ChuyensController : Controller
    {
        private readonly DataContext _context;

        private readonly Dictionary<string, double> _listGiaTang = new Dictionary<string, double>();
        public ChuyensController(DataContext context)
        {
            _listGiaTang.Add("Tết Nguyên Đán", 0.2);
            _listGiaTang.Add("Quốc khánh", 0.1);
            _listGiaTang.Add("30-4, 1-5", 0.15);
            _listGiaTang.Add("Mặc định",0 );
            _context = context;
        }

        // GET: Chuyens
        public async Task<IActionResult> Index()
        {
            ViewBag.listXeTrue = _context.Xes.Where(x => x.Trangthai == TrangThaiXe.NoActive).Include(l => l.LoaiXe).ToList();
            ViewBag.listTuyen = _context.Tuyens.ToList();
            ViewBag.listGiaTang = _listGiaTang;
            return View(await _context.Chuyens.Include(x => x.Xe).ThenInclude(l => l.LoaiXe).ToListAsync());
        }



        // GET: Chuyens/Create
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> Create(string ngayle, int XeId, int TuyenId, string DiemDon, DateTime ThoiGianDi, double Gia)
        {
            try
            {
                Xe? xe = await _context.Xes.Include(x => x.soDoGhes).FirstOrDefaultAsync(x => x.Id == XeId);
                Tuyen? tuyen = await _context.Tuyens.FirstOrDefaultAsync(m => m.Id == TuyenId);
                double giatang = _listGiaTang[ngayle];
                var chuyen = new Chuyen
                {
                    Ten = $"{tuyen?.Ten}",
                    Xe = xe,
                    Tuyen = tuyen,
                    DiemDon = DiemDon,
                    ThoiGianDi = ThoiGianDi,
                    GiaTang = giatang,
                    Trangthai = TrangThaiChuyen.WAITING,
                    Gia = Gia,
                    Ngayle = ngayle

                };

                _context.Add(chuyen);
                SoDoGhe? soDoGhe = await _context.SoDoGhes.Include(sdg => sdg.Tangs)
                            .ThenInclude(t => t.Hangs)
                            .ThenInclude(h => h.Ghes).
                    FirstOrDefaultAsync(s => s.Id == xe!.soDoGhes.FirstOrDefault()!.Id);
                foreach (var Tang in soDoGhe!.Tangs)
                {
                    foreach (var Hang in Tang.Hangs)
                    {
                        foreach (var Ghe in Hang.Ghes)
                        {
                            Ghe ghe = Ghe;
                            Ve ve = new Ve
                            {
                                Chuyen = chuyen,
                                TaiKhoan = null,
                                Ghe = ghe,
                                Tien = chuyen.Gia + Math.Round(chuyen.Gia * giatang),
                                Ten = null,
                                MaVe = null,
                                Sdt = null,
                                CMND = null,
                                DiemDon = chuyen.DiemDon,
                                TrangThai = TrangThaiVe.Empty,
                                NgayTao = DateTime.Now,
                            };

                            _context.Ves.Add(ve);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, string searchString)
        {
            var chuyen = await _context.Chuyens
                .Include(x => x.Xe)
                .Include(t => t.Tuyen)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chuyen == null)
            {
                return View();
            }

            // Tạo chuỗi truy vấn
            var query = _context.Ves.Include(c => c.Chuyen)
                                    .Include(g => g.Ghe)
                                    .Include(t => t.TaiKhoan)
                                    .Where(c => c.Chuyen.Id == id);

            // Kiểm tra xem có chuỗi tìm kiếm không
            if (!string.IsNullOrEmpty(searchString))
            {
                // Thêm điều kiện tìm kiếm vào chuỗi truy vấn
                query = query.Where(v => v.Ghe.Ten.Contains(searchString) || v.Ten.Contains(searchString) || v.CMND.Contains(searchString));
            }

            // Lấy danh sách vé
            ViewBag.listVe = await query.ToListAsync();

            return View(chuyen);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,DiemDon,ThoiGianDi,ThoiGianDen,Trangthai,Gia,KhoiluongHang,GiaHangKhoiDiem")] Chuyen chuyen)
        {
            if (id != chuyen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuyen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuyenExists(chuyen.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chuyen);
        }
        // POST: Chuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuyen = await _context.Chuyens.FindAsync(id);
            if (chuyen != null)
            {
                _context.Chuyens.Remove(chuyen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChuyenExists(int id)
        {
            return _context.Chuyens.Any(e => e.Id == id);
        }
    }
}
