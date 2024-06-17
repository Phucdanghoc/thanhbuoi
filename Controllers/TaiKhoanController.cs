using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Data;
using ThanhBuoi.Models;
using ThanhBuoi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThanhBuoi.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class TaiKhoanController : Controller
    {
        private readonly UserManager<TaiKhoan> _userManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        private const int PageSize = 10; // Number of items per page

        public TaiKhoanController(UserManager<TaiKhoan> userManager, DataContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = dataContext;
            _roleManager = roleManager;
        }

        // GET: TaiKhoanController/Index?page=1&searchString=example
        public async Task<IActionResult> Index(int page = 1, string searchString = null)
        {
            try
            {
                var users = await GetPaginatedUsers(page, searchString);
                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải danh sách người dùng. Vui lòng thử lại sau.";
                return View();
            }
        }

        private async Task<IEnumerable<TaiKhoan>> GetPaginatedUsers(int page, string searchString = null)
        {
            var userRole = await _roleManager.FindByNameAsync("USER");
            var userRoleId = userRole?.Id;

            if (userRoleId == null)
            {
                throw new Exception("Role 'USER' not found.");
            }

            var userRoleIds = await _context.UserRoles
                                            .Where(ur => ur.RoleId == userRoleId)
                                            .Select(ur => ur.UserId)
                                            .ToListAsync();

            var query = _userManager.Users.Where(u => userRoleIds.Contains(u.Id)).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(u => u.Ten.Contains(searchString) || u.UserName.Contains(searchString));
            }

            int totalUsers = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalUsers / PageSize);

            int startIndex = (page - 1) * PageSize;
            var users = await query.Skip(startIndex).Take(PageSize).ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return users;
        }

        // GET: TaiKhoanController/Detail/abc123
        public async Task<IActionResult> Detail(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.ListVe = await _context.Ves
                .Include(c => c.Chuyen)
                .Include(g => g.Ghe)
                .ThenInclude(h => h.Hang)
                .Include(t => t.TaiKhoan)
                .Where(u => u.TaiKhoan!.Id == id)
                .ToListAsync();

            return View(user);
        }

        // GET: TaiKhoanController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoanController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterDTO model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Tài khoản đã tồn tại";
                return View();
            }

            var newUser = new TaiKhoan
            {
                Email = model.Email,
                UserName = model.Email,
                Ten = model.Ten,
                PhoneNumber = model.SDT,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            await _userManager.AddToRoleAsync(newUser, "SALER");

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Tạo mới tài khoản thành công";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TaiKhoanController/Edit/abc123
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: TaiKhoanController/Edit/abc123
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind("Id, Email, Ten")] TaiKhoan user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(id);
                    existingUser.Email = user.Email;
                    existingUser.Ten = user.Ten;
                    await _userManager.UpdateAsync(existingUser);
                    TempData["SuccessMessage"] = "Chỉnh sửa tài khoản thành công";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật tài khoản. Vui lòng thử lại.";
                }

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: TaiKhoanController/Delete/abc123
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: TaiKhoanController/Delete/abc123
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Xóa tài khoản thành công";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa tài khoản. Vui lòng thử lại.";
            return RedirectToAction(nameof(Index));
        }
    }
}
