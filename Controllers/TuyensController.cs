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
    public class TuyensController : Controller
    {
        private readonly DataContext _context;

        public TuyensController(DataContext context)
        {
            _context = context;
        }

        // GET: Tuyens
        public async Task<IActionResult> Index(string di, string den)
        {
            ViewBag.listDiaDiem = await _context.Diadiems.Select(d => d.Ten).Distinct().ToListAsync();
            IQueryable<Tuyen> tuyens = _context.Tuyens;
            if (!string.IsNullOrEmpty(di) && !string.IsNullOrEmpty(den))
            {
                tuyens = tuyens.Where(d => d.Diemden.Ten == den && d.DiemDi.Ten == di);
            }

            return View(await tuyens.ToListAsync());
        }


        // GET: Tuyens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyen = await _context.Tuyens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tuyen == null)
            {
                return NotFound();
            }

            return View(tuyen);
        }

        // GET: Tuyens/Create
        public  IActionResult Create()
        {
            return View();
        }

        // POST: Tuyens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Thoigian,Khoangcach")] Tuyen tuyen,int diemdenID, int diemdiID)
        {
            if (ModelState.IsValid)
            {
                Diadiem DiemDi = await _context.Diadiems.FirstOrDefaultAsync(m => m.Id == diemdiID);
                Diadiem  DiemDen = await _context.Diadiems.FirstOrDefaultAsync(m => m.Id == diemdenID);
                tuyen.Ten = $"{DiemDi.Ten} - {DiemDen.Ten} - {GetCurrentTimeIntegerWithSecond()}";
                _context.Add(tuyen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tuyen);
        }

        // GET: Tuyens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyen = await _context.Tuyens.FindAsync(id);
            if (tuyen == null)
            {
                return NotFound();
            }
            return View(tuyen);
        }

        // POST: Tuyens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,Thoigian,Diemden,Khoangcach")] Tuyen tuyen)
        {
            if (id != tuyen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tuyen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuyenExists(tuyen.Id))
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
            return View(tuyen);
        }

        // GET: Tuyens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tuyen = await _context.Tuyens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tuyen == null)
            {
                return NotFound();
            }

            return View(tuyen);
        }

        // POST: Tuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tuyen = await _context.Tuyens.FindAsync(id);
            if (tuyen != null)
            {
                _context.Tuyens.Remove(tuyen);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TuyenExists(int id)
        {
            return _context.Tuyens.Any(e => e.Id == id);
        }

        public int GetCurrentTimeIntegerWithSecond()
        {
            DateTime currentTime = DateTime.Now;
            int timeInteger = currentTime.Hour * 10000 + currentTime.Minute * 100 + currentTime.Second;

            return timeInteger;
        }
    }
}
