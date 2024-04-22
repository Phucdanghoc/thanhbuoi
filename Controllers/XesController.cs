﻿using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class XesController : Controller
    {
        private readonly DataContext _context;

        public XesController(DataContext context)
        {
            _context = context;
        }

        // GET: Xes
        public async Task<IActionResult> Index()
        {
            var xes = await _context.Xes.Include(x => x.LoaiXe).ToListAsync();
            return View(xes);
        }

        // GET: Xes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xe = await _context.Xes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xe == null)
            {
                return NotFound();
            }

            return View(xe);
        }

        // GET: Xes/Create
        public IActionResult Create()
        {
            ViewBag.getLoaiXeList = _context.loaiXes.ToList();
            ViewBag.getHangXe = new List<string> { "Toyota", "Honda", "Ford", "Mercedes-Benz", "Volkswagen", "Hyundai", "Kia", "Nissan", "Chevrolet", "Peugeot" };
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Ten,BienSo,HangXe")] Xe xe,int loaixe_id)
        {
            ViewBag.getLoaiXeList = _context.loaiXes.ToList();
            ViewBag.getHangXe = new List<string> { "Toyota", "Honda", "Ford", "Mercedes-Benz", "Volkswagen", "Hyundai", "Kia", "Nissan", "Chevrolet", "Peugeot" };
            xe.LoaiXe = _context.loaiXes.FirstOrDefault(p => p.Id == loaixe_id);
            xe.MaXe = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            try
            {
                String LoaiGhe = xe.LoaiXe.Ma.Split("-")[1];
                String SoGhe = xe.LoaiXe.Ma.Split("-")[0];
                SoDoGhe sdg = new SoDoGhe
                {
                    LoaiGhe = LoaiGhe,
                    SoGhe = Int32.Parse(SoGhe),
                    Ten = $"{xe.MaXe}-{SoGhe}-{LoaiGhe}",
                    Xe =  xe
                };
                _context.Add(xe);
                _context.SoDoGhes.Add(sdg);
                if (LoaiGhe == "N")
                {
                    Tang tang = new Tang
                    {
                        Ten = $"Tầng 1",
                        STT = 1,
                        SoDoGhe = sdg

                    };
                    _context.Tangs.Add(tang);
                    for (int i = 0; i < 2; i++)
                    {
                        Hang hang = new Hang
                        {
                            Ten = $"H{i+1}",
                            STT = i + 1,
                            Tang = tang
                        };
                        _context.Hangs.Add(hang);
                        for (int j = 0; j < sdg.SoGhe / 2; j++)
                        {
                            Ghe ghe = new Ghe
                            {
                                Ten = $"N{j + 1}T{i + 1}",
                                STT = j + 1,
                                Hang = hang,
                            };
                            _context.Ghes.Add(ghe);
                        }
                    }
                }else
                {
                    for(int Sotang = 0; Sotang < 2; Sotang++) {
                        Tang tang = new Tang
                        {
                            Ten = $"Tầng 1",
                            STT = 1,
                            SoDoGhe = sdg

                        };
                        for (int Sohang = 0; Sohang < 2; Sohang++)
                        {
                            Hang HangGHe = new Hang
                            {
                                Ten = $"H{Sohang + 1}",
                                STT = Sohang + 1,
                                Tang = tang
                            };
                            _context.Hangs.Add(HangGHe);

                            for (int soGhe = 0; soGhe < sdg.SoGhe/4;  soGhe++)
                            {
                                Ghe ghe = new Ghe
                                {
                                    Ten = $"GN{soGhe + 1}T{SoGhe + 1}",
                                    STT = soGhe + 1,
                                    Hang = HangGHe,
                                };
                                _context.Ghes.Add(ghe);
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Tên đã tồn tại.";
                return View();
            }

        }

        // GET: Xes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xe = await _context.Xes.FindAsync(id);
            if (xe == null)
            {
                return NotFound();
            }
            return View(xe);
        }

        // POST: Xes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten,BienSo,MaXe,SoDoGhe,LoaiXe")] Xe xe)
        {
            if (id != xe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XeExists(xe.Id))
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
            return View(xe);
        }

        // GET: Xes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xe = await _context.Xes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (xe == null)
            {
                return NotFound();
            }

            return View(xe);
        }

        // POST: Xes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xe = await _context.Xes.FindAsync(id);
            if (xe != null)
            {
                _context.Xes.Remove(xe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XeExists(int id)
        {
            return _context.Xes.Any(e => e.Id == id);
        }
    }
}
