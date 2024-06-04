using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class VeHuysController : Controller
    {
        private const int PageSize = 10; // Define page size

        private readonly DataContext _context;

        public VeHuysController(DataContext context)
        {
            _context = context;
        }

        // GET: VeHuys
        public IActionResult Index(int page = 1, string searchString = null)
        {
            try
            {
                var paginatedVeHuys = GetPaginatedVeHuys(page, searchString);
                return View(paginatedVeHuys);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách vé. Vui lòng thử lại sau.";
                return View();
            }
        }

        private IEnumerable<VeHuy> GetPaginatedVeHuys(int page, string searchString)
        {
            var query = _context.VeHuys
                .Include(v => v.chuyen)
                .OrderByDescending(v => v.ngaytao)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(v => v.Name.Contains(searchString));
            }
            int startIndex = (page - 1) * PageSize;
            var listveHuys = query.Skip(startIndex).Take(PageSize).ToList();

            int totalChuyens = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalChuyens / PageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return listveHuys;
        }

        // GET: VeHuys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veHuy = await _context.VeHuys
                .Include(v => v.Ghe)
                .Include(v => v.chuyen)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (veHuy == null)
            {
                return NotFound();
            }

            return View(veHuy);
        }
    }
}
