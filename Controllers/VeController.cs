using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;

namespace ThanhBuoi.Controllers
{
    public class VeController : Controller
    {
        private readonly DataContext _context;
        public VeController(DataContext dataContext) {
            _context = dataContext;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            var ve = await _context.Ves
                                   .Include(c => c.Chuyen)
                                   .ThenInclude(x => x.Xe)
                                   .Include(g => g.Ghe)
                                   .FirstOrDefaultAsync(v => v.Id == id);
            return View(ve);
        }
    }
}
