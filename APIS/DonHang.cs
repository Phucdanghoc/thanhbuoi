using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Models;
using ThanhBuoi.Data;
using Microsoft.AspNetCore.Identity;
using ThanhBuoi.Models.DTO;
using ThanhBuoi.Services;
using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ThanhBuoi.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DonhangController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<TaiKhoan> _userManager;
        Dictionary<string, double> dictGiaHang = new Dictionary<string, double>();
        private IEmailService _emailService ;
        private MomoServices _momoServices ;

        public DonhangController(DataContext context, UserManager<TaiKhoan> userManager, IEmailService emailService, MomoServices momoServices)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _momoServices = momoServices;
            dictGiaHang.Add("Xe tay ga", 0.7);
            dictGiaHang.Add("Xe số", 0.3);
            dictGiaHang.Add("Hàng nhỏ", 0.2);
            dictGiaHang.Add("Hàng đặc biệt lớn", 1);
            _momoServices = momoServices;
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
        public async Task<ActionResult<IEnumerable<DTOHang>>> PostHangGuiBatch(List<DTOHang> hangGuis)
        {
            if (hangGuis == null || hangGuis.Count == 0)
            {
                return BadRequest("No HangGui objects were provided.");
            }
            var chuyen = await _context.Chuyens.FirstOrDefaultAsync(c => c.Id == hangGuis[0].IdChuyen);

            DonHang donHang = new DonHang
            {
                NgayTao = DateTime.Now,
                TaiKhoan = await _userManager.GetUserAsync(HttpContext.User),
                Trangthai = TrangThaiDonHang.Waiting,
                MaDon = $"{chuyen.Id}{int.Parse(DateTime.Now.ToString("MMddHHmmss"))}"
            };
            foreach (var hang in hangGuis)
            {
                hang.NgayTao = DateTime.Now;
                hang.TrangThai = TrangThaiHang.Waiting;
                hang.Chuyen = chuyen;
                hang.TaiKhoan = await _userManager.GetUserAsync(HttpContext.User);
                double basePrice = (double)(hang.Chuyen.Gia * (1 + hang.Chuyen.GiaTang));
                double finalPrice = basePrice + (basePrice * dictGiaHang[hang.LoaiHang]);
                hang.Tien = finalPrice;
                DonHangChiTiet donHangChiTiet = new DonHangChiTiet
                {
                    DonHang = donHang,
                    HangGui = hang,
                    Tien = hang.Tien
                };
                _context.HangGuis.Add(hang);
                _context.DonHangChiTiets.Add(donHangChiTiet);
            }
            donHang.Tien = hangGuis.Sum(hang => hang.Tien);
            _context.DonHangs.Add(donHang);

            await _context.SaveChangesAsync();
            var result = new { donhang = donHang.Id };
            return Ok(result);


        }
        [HttpDelete("{donHangId}")]
        public async Task<IActionResult> Delete(int donHangId)
        {
            try
            {
                if (donHangId <= 0)
                {
                    return BadRequest("Invalid DonHang ID.");
                }
                var donHang = await _context.DonHangs.FindAsync(donHangId);
                if (donHang == null)
                {
                    return NotFound();
                }
                await DeleteRelatedEntities(donHangId);

                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting DonHang with ID {donHangId}: {ex.Message}");
            }
        }

        private async Task DeleteRelatedEntities(int donHangId)
        {
            var chiTietDonHangs = await GetChiTietDonHangsWithHangGui(donHangId);

            if (chiTietDonHangs.Any())
            {
                RemoveChiTietDonHangs(chiTietDonHangs);
                RemoveAssociatedHangGuis(chiTietDonHangs);
            }
        }

        private async Task<List<DonHangChiTiet>> GetChiTietDonHangsWithHangGui(int donHangId)
        {
            return await _context.DonHangChiTiets
                .Include(chi => chi.HangGui)
                .Where(chi => chi.DonHang.Id == donHangId)
                .ToListAsync();
        }

        private void RemoveChiTietDonHangs(List<DonHangChiTiet> chiTietDonHangs)
        {
            _context.DonHangChiTiets.RemoveRange(chiTietDonHangs);
        }

        private void RemoveAssociatedHangGuis(List<DonHangChiTiet> chiTietDonHangs)
        {
            foreach (var chiTiet in chiTietDonHangs)
            {
                if (chiTiet.HangGui != null)
                {
                    _context.HangGuis.Remove(chiTiet.HangGui);
                }
            }
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto CheckoutDto)
        {
            if (CheckoutDto.Id == 0)
            {
                return BadRequest("No HangGui objects were provided.");

            }
            DonHang? donHang = await _context.DonHangs
                                                .Include(dh => dh.DonHangChiTiets).ThenInclude(h => h.HangGui)
                                                .FirstOrDefaultAsync(dh => dh.Id == CheckoutDto.Id);
            if (donHang != null)
            {
                donHang.PhuongThucThanhToan = CheckoutDto.PaymentMethod;
                donHang.Mota = CheckoutDto.Mota;
                donHang.email = CheckoutDto.Email ?? "";
                donHang.Trangthai = TrangThaiDonHang.Cod;
                try
                {
                    await _context.SaveChangesAsync();
                    if (donHang.email != null)
                    {
                        string bodyEmail = _emailService.makeBodyEmailOrder(donHang);
                        await _emailService.SendEmailAsync(donHang.email, "Xác nhận đơn hàng",bodyEmail);
                    }
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return BadRequest(e.Message);
                }
                if (CheckoutDto.PaymentMethod == "momo")
                {
                    MomoPaymentResponseDTO momoPaymentResponseDTO = await _momoServices.Pay(new PaymentDTO
                    {
                        url = $"https://localhost:7273/Donhangs/Details/{CheckoutDto.Id}",
                        cost = donHang.Tien.ToString(),
                    });
                    if (momoPaymentResponseDTO.ResultCode == 0)
                    {
                        donHang.RequestId = momoPaymentResponseDTO.RequestId;
                        donHang.Id_momoRes = momoPaymentResponseDTO.OrderId;
                        _context.DonHangs.Entry(donHang).State = EntityState.Modified;
                        return Ok(new {url = momoPaymentResponseDTO.PayUrl });
                    }
                }
                _context.DonHangs.Entry(donHang).State = EntityState.Modified;
                return Ok(new {url = ""});
            }
            else
            {
                return BadRequest("No HangGui objects were provided.");
            }
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


        private bool HangGuiExists(int id)
        {
            return _context.HangGuis.Any(e => e.Id == id);
        }
       
    }
}
