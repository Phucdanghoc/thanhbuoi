using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    [Authorize]
    public class DonVeController : Controller
    {
        private readonly DataContext _context;
        private readonly int PageSize = 10;

        public DonVeController(DataContext dbContext)
        {
            _context = dbContext;
        }

        // GET: DonVeController
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
                                    .ThenInclude(dhct => dhct.Ve)
                                        .ThenInclude(v => v.Chuyen)
                                .Where(dh => dh.DonHangChiTiets.Any(dhct => dhct.Ve != null))
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d => d.PhuongThucThanhToan.Contains(searchString) ||
                                         d.Mota.Contains(searchString) ||
                                         d.Email.Contains(searchString));
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

        public async Task<IActionResult> Details(int id)
        {
            var hoadon = await _context.DonHangs
                                .FirstOrDefaultAsync(d => d.Id == id);

            if (hoadon == null)
            {
                return NotFound();
            }

            var ves = await _context.DonHangChiTiets
                                .Include(dhct => dhct.Ve)
                                    .ThenInclude(v => v.Chuyen)
                                .Include(dhct => dhct.Ve)
                                    .ThenInclude(v => v.Ghe)
                                .Where(dhct => dhct.DonHang.Id == hoadon.Id)
                                .ToListAsync();
            ViewBag.ChiTietVe = ves;
            return View(hoadon);
        }
    }
}
