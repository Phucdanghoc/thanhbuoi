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
    public class GiaSukiensController : Controller
    {
        private readonly DataContext _context;

        public GiaSukiensController(DataContext context)
        {
            _context = context;
        }

        // GET: GiaSukiens
        public async Task<IActionResult> Index()
        {
            return View(await _context.GiaTuyens.ToListAsync());
        }

        // GET: GiaSukiens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giaSukien = await _context.GiaTuyens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giaSukien == null)
            {
                return NotFound();
            }

            return View(giaSukien);
        }

        // GET: GiaSukiens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiaSukiens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ten,Gia_hang,Gia_ve,NgayBatDau,NgayKetThuc")] GiaSukien giaSukien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giaSukien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giaSukien);
        }

        // GET: GiaSukiens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giaSukien = await _context.GiaTuyens.FindAsync(id);
            if (giaSukien == null)
            {
                return NotFound();
            }
            return View(giaSukien);
        }

        // POST: GiaSukiens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,Gia_hang,Gia_ve,NgayBatDau,NgayKetThuc")] GiaSukien giaSukien)
        {
            if (id != giaSukien.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giaSukien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiaSukienExists(giaSukien.Id))
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
            return View(giaSukien);
        }

        // GET: GiaSukiens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giaSukien = await _context.GiaTuyens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giaSukien == null)
            {
                return NotFound();
            }

            return View(giaSukien);
        }

        // POST: GiaSukiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giaSukien = await _context.GiaTuyens.FindAsync(id);
            if (giaSukien != null)
            {
                _context.GiaTuyens.Remove(giaSukien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiaSukienExists(int id)
        {
            return _context.GiaTuyens.Any(e => e.Id == id);
        }
    }
}
