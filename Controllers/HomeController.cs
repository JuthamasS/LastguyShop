using LastguyShop.Data.Context;
using LastguyShop.Data.Entities;
using LastguyShop.Models;
using LastguyShop.Models.Product;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LastguyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LastguyShopContext _lastguyShopContext;

        public HomeController(ILogger<HomeController> logger, LastguyShopContext lastguyShopContext)
        {
            _logger = logger;
            _lastguyShopContext = lastguyShopContext;
        }

        public IActionResult Index()
        {
            return View("ListProduct");
        }

        public IActionResult ListProduct()
        {
            var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0).ToList();
            var objectPrice = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0).OrderByDescending(o => o.CreatedDate).ToList();
            var objectSupplier = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0).OrderByDescending(o => o.CreatedDate).ToList();
            var objectProductList = new List<ListProduct>();
            if (objectProduct != null)
            {
                foreach (var item in objectProduct)
                {
                    objectProductList.Add(new ListProduct
                    {
                        productId = item.ProductId,
                        productName = item.Name,
                        price = 0,
                        totalAmount = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                        unit = item.Unit,
                        description = item.Description,
                        note = item.Note,
                        supplierName = ""
                    });
                }
            }
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