using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class DonHangsController : Controller
    {
        private readonly DataContext _context;
        private const int PageSize = 10;

        public DonHangsController(DataContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "ADMIN,SALER")]
        // GET: DonHangs
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

        // GET: DonHangs/Details/5
        [HttpGet]
        [Authorize(Roles = "ADMIN,SALER,USER")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.DonHangs
                                        .Include(dh => dh.DonHangChiTiets)
                                        .ThenInclude(dhct => dhct.HangGui)
                                        .FirstOrDefaultAsync(d => d.Id == id);

            if (donHang == null) return NotFound();

            ViewBag.listChitietDonhang = donHang.DonHangChiTiets.ToList();

            return View(donHang);
        }


        // POST: DonHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,SALER")]

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
        [Authorize(Roles = "ADMIN,SALER")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null) return NotFound();

            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,SALER")]

        public async Task<IActionResult> Edit(int id, [Bind("Id,RequestId,Id_momoRes,Tien,TienPhaiTra,PhuongThucThanhToan,Mota,Trangthai,NgayTao")] DonHang donHang)
        {
            if (id != donHang.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(donHang);
        }

      
        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN,SALER")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.Id == id);
        }
    }
}
