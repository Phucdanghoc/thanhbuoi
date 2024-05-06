using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class BookingController : Controller
    {
        private readonly DataContext _context;
        private const int PageSize = 10; // Số lượng chuyến trên mỗi trang

        public BookingController(DataContext context) {
            _context = context;
        }
        // GET: BookingController
        public IActionResult Index(int page = 1, string searchString = null)
        {
            try
            {
                var listChuyen = GetPaginatedChuyens(page, searchString);
                return View(listChuyen);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ, ví dụ: ghi log, hiển thị thông báo lỗi, ...
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách chuyến. Vui lòng thử lại sau.";
                return View();
            }
        }

        private IEnumerable<Chuyen> GetPaginatedChuyens(int page, string searchString = null)
        {
            var query = _context.Chuyens.Include(x => x.Xe).OrderByDescending(c => c.ThoiGianDi).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Ten.Contains(searchString));
            }

            int startIndex = (page - 1) * PageSize;
            var listChuyen = query.Skip(startIndex).Take(PageSize).ToList();

            int totalChuyens = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalChuyens / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listChuyen;
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: BookingController/Create
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int veId,String SDT, String CMND,String Ten,int HanhLi)
        {
            try
            {
                Ve? ve = await _context.Ves
                    .Include(g =>g.Ghe)
                    .Include(c => c.Chuyen)
                    .ThenInclude(x => x.Xe)
                    .FirstOrDefaultAsync(v => v.Id == veId);
                if (ve != null)
                {
                    ve.Ten = Ten;
                    ve.CMND = CMND;
                    ve.Sdt = SDT;
                    ve.MaVe = $"{ve.Chuyen.Xe!.MaXe}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
                    ve.TrangThai = TrangThaiVe.Booked;
                    ve.Ghe.KhoangTrong = false;
                    ve.Hanhli = HanhLi;
                    if (ve.Hanhli > 20)
                    {
                        ve.Tien = ve.Chuyen.Gia + Math.Round((ve.Hanhli - 20) * 0.5);
                    }
                    else
                    {
                        ve.Tien = ve.Chuyen.Gia;
                    }
                    ve.Chuyen.KhoiluongHang += ve.Hanhli;

                }
                _context.Chuyens.Update(ve.Chuyen);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        [Route("Ve/{id}")]
        public async Task<IActionResult> Ve(int id)
        {
            var chuyen = await _context.Chuyens.FindAsync(id);
            if (chuyen == null)
            {
                return NotFound();
            }

            // Lấy danh sách vé cho chuyến này
            var ves = await _context.Ves.Include(c =>  c.Chuyen)
                .Include(g => g.Ghe)
                .Include(t => t.TaiKhoan)
                .FirstOrDefaultAsync(v => v.Id == id);
            return View(ves);
        }
        [HttpPost]
        [Route("Ve/Cancel/{id}")]
        public async Task<ActionResult> Cancel(int id)
        {
            Ve? ve = _context.Ves.Include(g => g.Ghe).FirstOrDefault(v => v.Id == id);
            if(ve != null)
            {
                ve.TrangThai = TrangThaiVe.Cancel;
                ve.Ghe.KhoangTrong = true;
                _context.Ghes.Update(ve.Ghe);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã hủy vé của {ve.Ten}";
                return View();
            }

            return View();
        }


        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
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

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
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
