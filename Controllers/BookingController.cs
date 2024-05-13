using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        UserManager<TaiKhoan> _userManager;

        public BookingController(DataContext context, UserManager<TaiKhoan> userManager) {
            _context = context;
            _userManager = userManager;

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
        public async Task<ActionResult> Create(int Id, String SDT, String CMND, String Ten,int HanhLi)
        {
            try
            {
                Ve? ve = await _context.Ves
                    .Include(g =>g.Ghe)
                    .Include(c => c.Chuyen)
                    .ThenInclude(x => x.Xe)
                    .FirstOrDefaultAsync(v => v.Id == Id);
                if (ve != null)
                {
                    ve.Ten = Ten;
                    ve.CMND = CMND;
                    ve.Sdt = SDT;
                    ve.MaVe = $"TB-{ve.Chuyen.ThoiGianDi.Day}-{ve.Chuyen.Xe.MaXe}{Id}";
                    ve.TrangThai = TrangThaiVe.Booked;
                    ve.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
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

                }
                _context.Chuyens.Update(ve.Chuyen);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Ve", "Booking", new { id = Id });

            }
        }
        public async Task<IActionResult> Ve(int id)
        {
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
        public async Task<ActionResult> Chuyens(int id) 
        {
            Chuyen? chuyens = await _context.Chuyens.Include(t => t.Tuyen).Include(x => x.Xe).FirstOrDefaultAsync(c => c.Id == id);
            ViewBag.Ves = await _context.Ves.Include(c => c.Chuyen)
                .Include(g => g.Ghe).ToListAsync();
            return  View(chuyens);
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
