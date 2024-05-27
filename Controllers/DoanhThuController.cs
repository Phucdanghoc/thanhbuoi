using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ThanhBuoi.Controllers
{
    public class DoanhThuController : Controller
    {
        // GET: DoanhThu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DoanhThu/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DoanhThu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: DoanhThu/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DoanhThu/Edit/5
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

        // GET: DoanhThu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DoanhThu/Delete/5
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
