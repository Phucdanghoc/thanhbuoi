using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ThanhBuoi.Data;
using ThanhBuoi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ThanhBuoi.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Chuyen : ControllerBase
    {
        private readonly Dictionary<string, double> _listGiaTang = new Dictionary<string, double>();

        private readonly DataContext _context;
        public Chuyen(DataContext context)
        {
            _listGiaTang.Add("Tết Nguyên Đán", 0.1);
            _listGiaTang.Add("Quốc khánh", 0.05);
            _listGiaTang.Add("30-4, 1-5", 0.03);
            _context = context;
        }

        // GET: api/<ChuyenController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ChuyenController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Chuyen>> GetAsync(int id)
        {
            var chuyen = await _context.Chuyens
                 .Include(x => x.Xe)
                 .Include(t => t.Tuyen)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (chuyen == null)
            {
                return NotFound(); // Return 404 Not Found if Chuyen is not found
            }

            return Ok(chuyen); // Return Chuyen object with 200 OK status
        }
        // POST api/<ChuyenController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<IEnumerable<Chuyen>>> GetChuyens([FromQuery] double weight, [FromQuery] string name)
        {
            var query = _context.Chuyens.Include(c => c.Tuyen).Include(c => c.Xe)
                                        .Where(t => t.ThoiGianDi.Date > DateTime.Now.Date && t.Ten.Contains(name));

            var chuyens = await query.ToListAsync();

            // Serialize the result
            var jsonResult = JsonConvert.SerializeObject(chuyens, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(jsonResult);
        }

        // PUT api/<ChuyenController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, string ngayle, int XeId, int TuyenId, string DiemDon, DateTime ThoiGianDi, double Gia)
        {
            var chuyen = await _context.Chuyens.FindAsync(id);

            if (chuyen == null)
            {
                return NotFound("Không tìm thấy chuyến"); // Trả về 404 Not Found cùng với thông điệp nếu không tìm thấy Chuyến với id được cung cấp
            }

            Xe xe = await _context.Xes.Include(x => x.soDoGhes).FirstOrDefaultAsync(x => x.Id == XeId);
            if (xe == null)
            {
                return BadRequest("Không tìm thấy xe"); // Trả về 400 Bad Request cùng với thông điệp nếu không tìm thấy Xe với Id được cung cấp
            }

            Tuyen tuyen = await _context.Tuyens.FirstOrDefaultAsync(m => m.Id == TuyenId);
            if (tuyen == null)
            {
                return BadRequest("Không tìm thấy tuyến"); // Trả về 400 Bad Request cùng với thông điệp nếu không tìm thấy Tuyến với Id được cung cấp
            }

            double giatang = _listGiaTang[ngayle];
            chuyen.Ngayle = ngayle;
            chuyen.Xe = xe;
            chuyen.Tuyen = tuyen;
            chuyen.GiaTang = giatang;
            chuyen.DiemDon = DiemDon;
            chuyen.ThoiGianDi = ThoiGianDi;
            chuyen.Gia = Gia;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChuyenExists(id))
                {
                    return NotFound("Không tìm thấy chuyến");
                }
                else
                {
                    throw;
                }
            }
        }

        // Check if Chuyen with the provided id exists
        private bool ChuyenExists(int id)
        {
            return _context.Chuyens.Any(e => e.Id == id);
        }
    

    // DELETE api/<ChuyenController>/5
    [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    
}
