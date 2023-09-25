using LastguyShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LastguyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("ListProduct");
        }

        public IActionResult ListProduct()
        {
            return View();
        }

        public IActionResult InsertProduct()
        {
            return View();
        }

        public IActionResult ManageProduct()
        {
            return View();
        }
        public IActionResult ManageHistoryPrice()
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