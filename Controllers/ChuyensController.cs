using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class ChuyensController : Controller
    {
        private readonly DataContext _context;

        public ChuyensController(DataContext context)
        {
            _context = context;
        }

        // GET: Chuyens
        public async Task<IActionResult> Index()
        {
            ViewBag.listXeTrue = _context.Xes.Where(x => x.Trangthai == TrangThaiXe.NoActive).ToList();
            ViewBag.listTuyen = _context.Tuyens.ToList();
            ViewBag.listGiaSuKien = _context.GiaTuyens.ToList();
            return View(await _context.Chuyens.ToListAsync());
        }

        // GET: Chuyens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuyen = await _context.Chuyens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuyen == null)
            {
                return NotFound();
            }

            return View(chuyen);
        }

        // GET: Chuyens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( int XeId, int TuyenId, string DiemDon, DateTime ThoiGianDi, DateTime ThoiGianDen, int GiaSukienID, double Gia)
        {
            // Retrieve necessary entities from the database
            Xe? xe = await _context.Xes.Include(xe => xe.soDoGhes).FirstOrDefaultAsync(m => m.Id == XeId);
            Tuyen? tuyen = await _context.Tuyens.FirstOrDefaultAsync(m => m.Id == TuyenId);
            GiaSukien? gia = await _context.GiaTuyens.FirstOrDefaultAsync(m => m.Id == GiaSukienID);

            var chuyen = new Chuyen
            {
                Ten = $"{tuyen?.Ten}",
                Xe = xe,
                Tuyen = tuyen,
                DiemDon = DiemDon,
                ThoiGianDi = ThoiGianDi,
                ThoiGianDen = ThoiGianDen,
                Trangthai = TrangThaiChuyen.WAITING,
                GiaSukien = gia,
                Gia = Gia,
                KhoiluongHang = 0,
            };

            if (ModelState.IsValid)
            {
                _context.Add(chuyen);
                SoDoGhe? soDoGhe = await _context.SoDoGhes.Include(sdg=>sdg.Tangs)
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
                                Tien = Math.Round(chuyen.Gia * (gia.Gia_ve / 100)),
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
            TempData["ErrorrMessage"] = "Lỗi !";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Route("Detail/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Chuyen? chuyen = await _context.Chuyens.FindAsync(id);
            if (chuyen == null)
            {
                return View();
            }
            ViewBag.listVe = await _context.Ves.Include(c => c.Chuyen)
                .Include(g => g.Ghe)
                .Include(t => t.TaiKhoan)
                .ToListAsync();
            
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

        // GET: Chuyens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chuyen = await _context.Chuyens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuyen == null)
            {
                return NotFound();
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
