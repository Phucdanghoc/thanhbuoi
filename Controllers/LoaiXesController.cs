using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class LoaiXesController : Controller
    {
        private readonly DataContext _context;

        public LoaiXesController(DataContext context)
        {
            _context = context;
        }

        // GET: LoaiXes
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoaiXes.ToListAsync());
        }

        // GET: LoaiXes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiXe = await _context.LoaiXes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiXe == null)
            {
                return NotFound();
            }

            return View(loaiXe);
        }

        // GET: LoaiXes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ten, Mota")] LoaiXe loaiXe, int SoGhe, string LoaiGhe)
        {
            if (LoaiGhe == null)
            {
                TempData["ErrorMessage"] = "Loại ghế không được để trống.";
                return RedirectToAction(nameof(Index));
            }

            loaiXe.Ma = $"{SoGhe}-{LoaiGhe}";
            loaiXe.LoaiGheXe = LoaiGhe.Contains("GN") ? LoaiGheXe.GiuongNam : LoaiGheXe.Ngoi;

            if (_context.LoaiXes.Any(x => x.Ten == loaiXe.Ten))
            {
                TempData["ErrorMessage"] = "Tên đã tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            _context.Add(loaiXe);
            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công.";
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LoaiXes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiXe = await _context.LoaiXes.FindAsync(id);
            if (loaiXe == null)
            {
                return NotFound();
            }

            return View(loaiXe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,Mota")] LoaiXe loaiXe)
        {
            if (id != loaiXe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiXe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiXeExists(loaiXe.Id))
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

            return View(loaiXe);
        }

        // GET: LoaiXes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loaiXe = await _context.LoaiXes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiXe == null)
            {
                return NotFound();
            }

            return View(loaiXe);
        }

        // POST: LoaiXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loaiXe = await _context.LoaiXes.FindAsync(id);
            if (loaiXe != null)
            {
                _context.LoaiXes.Remove(loaiXe);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LoaiXeExists(int id)
        {
            return _context.LoaiXes.Any(e => e.Id == id);
        }
    }
}
