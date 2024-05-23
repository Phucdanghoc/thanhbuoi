using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

namespace ThanhBuoi.APIS
{
 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class BookVe : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<TaiKhoan> _userManager;

        public BookVe(DataContext context, UserManager<TaiKhoan> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET api/<BookVe>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ve>> Get(int id)
        {
            var ve = await _context.Ves
                .Include(c => c.Chuyen) // Include the related 'Chuyen' entity
                .Include(g => g.Ghe)    // Include the related 'Ghe' entity
                .Include(t => t.TaiKhoan) // Include the related 'TaiKhoan' entity
                .FirstOrDefaultAsync(v => v.Id == id); // Find the ticket by id

            if (ve == null)
            {
                return NotFound();
            }

            return Ok(ve); 
        }
        // POST api/<BookVe>
        // POST api/BookVe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<ActionResult> Create(int Id, string SDT, string CMND, string Ten, int HanhLi)
        {
            try
            {
                // Find the ticket by ID, including related entities
                Ve? ve = await _context.Ves
                    .Include(g => g.Ghe)
                    .Include(c => c.Chuyen)
                    .ThenInclude(x => x.Xe)
                    .FirstOrDefaultAsync(v => v.Id == Id);
                // Check if the ticket exists
                if (ve == null)
                {
                    return RedirectToAction("Ve", "Booking", new { id = Id });
                }
                // Update the ticket properties
                ve.Ten = Ten;
                ve.CMND = CMND;
                ve.Sdt = SDT;
                ve.MaVe = $"TB-{ve.Chuyen.ThoiGianDi.Day}-{ve.Chuyen.Xe.MaXe}{Id}";
                ve.TrangThai = TrangThaiVe.Booked;
                ve.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
                ve.Ghe.KhoangTrong = false;
                ve.Hanhli = HanhLi;

                // Calculate the ticket price based on luggage weight
                if (ve.Hanhli > 20)
                {
                    ve.Tien = ve.Chuyen.Gia + Math.Round((ve.Hanhli - 20) * 0.5);
                }
                else
                {
                    ve.Tien = ve.Chuyen.Gia;
                }

                // Update the related entities in the database
                _context.Chuyens.Update(ve.Chuyen);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();

                // Set a success message and redirect to the booking view
                return RedirectToAction("Ve", "Booking", new { id = Id });
            }
            catch (Exception e)
            {
                return RedirectToAction("Ve", "Booking", new { id = Id });
            }
        }


    // PUT api/<BookVe>/5
    [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BookVe>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // POST api/BookVe/Ve/Cancel/{id}
        [HttpPost]
        [Route("Cancel/{id}")]
        public async Task<ActionResult> Cancel(int id)
        {
            Ve? ve = await _context.Ves
                                   .Include(g => g.Ghe)
                                   .FirstOrDefaultAsync(v => v.Id == id);
            if (ve == null)
            {
                return NotFound(new { Message = "Không tìm thấy vé để hủy." });
            }

            try
            {
                ve.TrangThai = TrangThaiVe.Cancel;
                ve.Ten = null;
                ve.CMND = null;
                ve.Sdt = null;
                ve.MaVe = null;
                ve.TaiKhoan = null;
                ve.Hanhli = 0;
                ve.Tien = 0;
                ve.Ghe.KhoangTrong = true;
                _context.Ghes.Update(ve.Ghe);
                _context.Ves.Update(ve);
                await _context.SaveChangesAsync();
                return Ok(new { Message = $"Đã hủy vé thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Đã xảy ra lỗi khi hủy vé: {ex.Message}" });
            }
        }
    }
}
