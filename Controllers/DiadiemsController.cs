using System;
using System.Collections.Generic;
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
    public class DiadiemsController : Controller
    {
        private readonly DataContext _context;

        public DiadiemsController(DataContext context)
        {
            _context = context;
        }

        // GET: Diadiems
        public async Task<IActionResult> Index(string t)
        {
            IQueryable<Diadiem> diadiems = _context.Diadiems;
            ViewBag.listTinhThanh = diadiems.Select(d => d.Ten).Distinct().ToList();
            if (!string.IsNullOrEmpty(t))
            {
                if (t == "all")
                {
                    return View(await diadiems.ToListAsync());
                }
                diadiems = diadiems.Where(d => d.Ten == t);
            }
            return View(await diadiems.ToListAsync());
        }

        // GET: Diadiems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diadiem = await _context.Diadiems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diadiem == null)
            {
                return NotFound();
            }

            return View(diadiem);
        }

        // GET: Diadiems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Diadiems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(string Tinh, string Huyen, string Xa, string diaChi)
        {
            // Check if the Tinh is not null or empty
            if (!string.IsNullOrEmpty(Tinh) || !string.IsNullOrEmpty(Huyen) || !string.IsNullOrEmpty(Xa) || !string.IsNullOrEmpty(diaChi))
            {
                // Check if the Ten already exists in the database
                if (_context.Diadiems.Any(d => d.Diachi == $"{diaChi}-{Xa}-{Huyen}"))
                {
                    // If Ten already exists, set an error message and return to the view
                    TempData["ErrorMessage"] = "Địa điểm đã tồn tại.";
                    return View();
                }

                // If Ten doesn't exist, create a new Diadiem object and add it to the context
                var diadiem = new Diadiem
                {
                    Ten = Tinh,
                    Diachi = $"{diaChi}-{Xa}-{Huyen}"
                };
                _context.Add(diadiem);
                await _context.SaveChangesAsync();
                // Set a success message and redirect to the Index action
                TempData["SuccessMessage"] = "Thêm mới thành công.";
                return RedirectToAction(nameof(Index));
            }

            // If Tinh is null or empty, return to the view with an error message
            TempData["ErrorMessage"] = "Vui lòng nhập Tỉnh.";
            return View();
        }


        // GET: Diadiems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diadiem = await _context.Diadiems.FindAsync(id);
            if (diadiem == null)
            {
                return NotFound();
            }
            return View(diadiem);
        }

        // POST: Diadiems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,Diachi")] Diadiem diadiem)
        {
            if (id != diadiem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diadiem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiadiemExists(diadiem.Id))
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
            return View(diadiem);
        }

        // GET: Diadiems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diadiem = await _context.Diadiems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diadiem == null)
            {
                return NotFound();
            }

            return View(diadiem);
        }

        // POST: Diadiems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diadiem = await _context.Diadiems.FindAsync(id);
            if (diadiem != null)
            {
                _context.Diadiems.Remove(diadiem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiadiemExists(int id)
        {
            return _context.Diadiems.Any(e => e.Id == id);
        }
    }
}
