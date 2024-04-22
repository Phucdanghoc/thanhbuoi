using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThanhBuoi.Models;

namespace ThanhBuoi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<TaiKhoan> _singManager;
        private readonly UserManager<TaiKhoan> _userManager;

        public HomeController(ILogger<HomeController> logger,SignInManager<TaiKhoan> signInManager, UserManager<TaiKhoan> _userManager)

        {
            _logger = logger;
            _singManager = signInManager;
            _userManager = _userManager;


        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
