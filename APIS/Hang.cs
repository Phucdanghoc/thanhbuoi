using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Models;
using ThanhBuoi.Data;
using Microsoft.AspNetCore.Identity;
using ThanhBuoi.Models.DTO;

namespace ThanhBuoi.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangGuisController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<TaiKhoan> _userManager;
        Dictionary<string, double> dictGiaHang = new Dictionary<string, double>();

        public HangGuisController(DataContext context, UserManager<TaiKhoan> userManager)
        {
            _context = context;
            _userManager = userManager;
            dictGiaHang.Add("Xe tay ga ", 0.7);
            dictGiaHang.Add("Xe số ", 0.3);
            dictGiaHang.Add("Hàng nhỏ", 0.2);
            dictGiaHang.Add("Hàng đặc biệt lớn", 1);
        }

        // GET: api/HangGuis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HangGui>>> GetHangGuis()
        {
            return await _context.HangGuis.ToListAsync();
        }

        // GET: api/HangGuis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HangGui>> GetHangGui(int id)
        {
            var hangGui = await _context.HangGuis.FindAsync(id);

            if (hangGui == null)
            {
                return NotFound();
            }
            return hangGui;
        }

        // POST: api/HangGuis/Batch
        [HttpPost("Batch")]
        public async Task<ActionResult<IEnumerable<DTOHang>>> PostHangGuiBatch( List<DTOHang> hangGuis)
        {
            if (hangGuis == null || hangGuis.Count == 0)
            {
                return BadRequest("No HangGui objects were provided.");
            }
            DonHang donHang = new DonHang
            {
                NgayTao = DateTime.Now,
                Trangthai = TrangThaiDonHang.Waiting
            };
            foreach (var hang in hangGuis)
            {
                hang.NgayTao = DateTime.Now;
                hang.TrangThai = TrangThaiHang.Waiting;
                hang.Chuyen = await _context.Chuyens.FirstOrDefaultAsync(c => c.Id == hang.IdChuyen);
                hang.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
                hang.Tien = ((double)(hang.Chuyen.Gia * hang.Chuyen.GiaTang)) * dictGiaHang[hang.LoaiHang];
                DonHangChiTiet donHangChiTiet = new DonHangChiTiet
                {
                    DonHang = donHang,
                    HangGui = hang

                };
                _context.HangGuis.Add(hang);
                _context.DonHangChiTiets.Add(donHangChiTiet);
            }
            donHang.Tien = hangGuis.Sum(hang => hang.Tien);
            _context.DonHangs.Add(donHang);

            await _context.SaveChangesAsync();
            var result = new { donhang = donHang.Id};
            return Ok(result);


        }

        // PUT: api/HangGuis/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHangGui(int id, HangGui hangGui)
        {
            if (id != hangGui.Id)
            {
                return BadRequest();
            }

            _context.Entry(hangGui).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HangGuiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/HangGuis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHangGui(int id)
        {
            var hangGui = await _context.HangGuis.FindAsync(id);
            if (hangGui == null)
            {
                return NotFound();
            }

            _context.HangGuis.Remove(hangGui);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HangGuiExists(int id)
        {
            return _context.HangGuis.Any(e => e.Id == id);
        }
    }
}
