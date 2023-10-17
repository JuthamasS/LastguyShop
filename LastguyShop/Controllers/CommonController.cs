using Microsoft.AspNetCore.Mvc;

namespace LastguyShop.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistrationMember()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            return View();
        }

        public IActionResult FileManagement()
        {
            return View();
        }
    }
}
