using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ThanhBuoi.Data;
using ThanhBuoi.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ThanhBuoi.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoanhThu : ControllerBase
    {
        // GET: api/<DoanhThu>
        private readonly DataContext _context;
        public DoanhThu (DataContext dataContext)
        {
            _context = dataContext;
        }
        [HttpGet]
        public IActionResult GetDonHang(DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (!fromDate.HasValue)
            {
                fromDate = DateTime.Today;
            }
            if (!toDate.HasValue)
            {
                toDate = DateTime.Today.AddDays(1).AddTicks(-1); 
            }

            var donhangs = _context.DonHangs
                .Where(d => d.NgayTao >= fromDate && d.NgayTao <= toDate)
                .ToList();
            double totalprice = donhangs.Sum(e => e.Tien);
            return Ok(new { donhang = donhangs,tong = totalprice });
        }
        [HttpGet]
        public IActionResult GetChuyen(DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (!fromDate.HasValue)
            {
                fromDate = DateTime.Today;
            }
            if (!toDate.HasValue)
            {
                toDate = DateTime.Today.AddDays(1).AddTicks(-1);
            }

            var chuyens = _context.Chuyens
                .Where(d => d.ThoiGianDi >= fromDate && d.ThoiGianDi <= toDate)
                .ToList();
            List<DoanhThuVe> doanhThuVes = new List<DoanhThuVe>();

            foreach (var chuyen in chuyens)
            {
                double total = _context.Ves.Where(c => c.Chuyen.Id == chuyen.Id && c.TrangThai == Models.TrangThaiVe.Booked).ToList().Sum(v => v.Tien);
                DoanhThuVe doanhThuVe = new DoanhThuVe
                {
                    Chuyen = chuyen,
                    TotalPrice = total,
                };
                doanhThuVes.Add(doanhThuVe);
            }
            return Ok(doanhThuVes);
        }

        [HttpGet("total-this-month")]
        public IActionResult GetTotalThisMonth()
        {
            var fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var toDate = fromDate.AddMonths(1).AddDays(-1);

            return GetTotalRevenue(fromDate, toDate);
        }
        [HttpGet("total")]
        public IActionResult GetTotalRevenue(DateTime? fromDate = null, DateTime? toDate = null)
        {
            if (!fromDate.HasValue)
            {
                fromDate = DateTime.Today;
                
            }
            if (!toDate.HasValue)
            {
                toDate = DateTime.Today.AddDays(1).AddTicks(-1);
            }

            var totalAll = new List<DoanhTheoNgay>();
            var totalHang = new List<DoanhTheoNgay>();
            var totalVe = new List<DoanhTheoNgay>();

            for (DateTime date = fromDate.Value; date <= toDate.Value; date = date.AddDays(1))
            {
                // Get total from DonHang
                var donhangs = _context.DonHangs
                 .Include(d => d.DonHangChiTiets)
                     .ThenInclude(dhct => dhct.Ve)
                 .Where(d => d.NgayTao.Date == date.Date && d.DonHangChiTiets.Any(dhct => dhct.Ve == null))
                 .ToList();

                double totalDonHang = donhangs.Sum(e => e.Tien);
                
                // Get total from Chuyen
                var donves = _context.DonHangs
                              .Include(d => d.DonHangChiTiets)
                                  .ThenInclude(dhct => dhct.HangGui)
                              .Where(d => d.NgayTao.Date == date.Date && d.DonHangChiTiets.Any(dhct => dhct.HangGui == null))
                              .ToList();
                double totalDonVe = donves.Sum(e => e.Tien);
                var donvehuys = _context.VeHuys.Where(c => c.ngaytao.Date == date.Date).ToList();

                double totalDonVeHuy = Math.Round((donvehuys.Sum(e => e.hoantien) / 0.7)*0.3);
                double combinedTotal = totalDonHang + totalDonVe + totalDonVeHuy;
                var dailyTotal = new DoanhTheoNgay
                {
                    totalHang = totalDonHang,
                    totalVe = totalDonVe,
                    totalHuy = totalDonVeHuy,
                    datetime = date
                };

                totalAll.Add(dailyTotal);
            }
            return Ok(totalAll);
        }

    }
}
