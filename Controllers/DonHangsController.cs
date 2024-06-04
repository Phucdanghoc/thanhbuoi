using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{

    [Authorize(Roles = "ADMIN")]
    public class DonHangsController : Controller
    {
        private readonly DataContext _context;
        private const int PageSize = 10;

        public DonHangsController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, string searchString = null)
        {
            try
            {
                var listDonHang = GetPaginatedDonHangs(page, searchString);
                return View(listDonHang);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách đơn hàng. Vui lòng thử lại sau.";
                return View();
            }
        }

        private IEnumerable<DonHang> GetPaginatedDonHangs(int page, string searchString = null)
        {
            var query = _context.DonHangs
                                                  .Include(dh => dh.DonHangChiTiets)
                                                  .ThenInclude(dhct => dhct.HangGui)
                                                  .Where(dh => dh.DonHangChiTiets.Any(dhct => dhct.HangGui != null))
                                                  .AsQueryable();



            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d =>d.PhuongThucThanhToan.Contains(searchString) || d.Mota.Contains(searchString) || d.email.Contains(searchString));
            }

            int startIndex = (page - 1) * PageSize;
            var listDonHang = query.OrderByDescending(d => d.NgayTao).Skip(startIndex).Take(PageSize).ToList();
            int totalDonHangs = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalDonHangs / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listDonHang;
        }

        // Other actions like Details, Create, Edit, Delete ...

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.Id == id);
        }

        // GET: DonHangs/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            DonHang donHang = await _context.DonHangs.FirstOrDefaultAsync(d => d.Id == id);
            var listChitietDonhang = await _context.DonHangChiTiets
                                                .Include(d => d.HangGui)
                                                .Where(c => c.DonHang.Id == id)
                                                .ToListAsync();
            ViewBag.listChitietDonhang = listChitietDonhang;
            return View(donHang);
        }

        // GET: DonHangs/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestId,Id_momoRes,Tien,TienPhaiTra,PhuongThucThanhToan,Mota,Trangthai,NgayTao")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestId,Id_momoRes,Tien,TienPhaiTra,PhuongThucThanhToan,Mota,Trangthai,NgayTao")] DonHang donHang)
        {
            if (id != donHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.Id))
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
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
