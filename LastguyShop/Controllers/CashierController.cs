using LastguyShop.Data.Context;
using LastguyShop.Models.Cashier;
using LastguyShop.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace LastguyShop.Controllers
{
    public class CashierController : Controller
    {
        private readonly LastguyShopContext _lastguyShopContext;
        public CashierController(LastguyShopContext lastguyShopContext)
        {
            _lastguyShopContext = lastguyShopContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ProductCashierModel GetDataDetail(string barcode)
        {
            var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.Name == barcode).FirstOrDefault();
            var objectPrice = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0 && i.HistoryPriceId == objectProduct.HistoryId).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            var objectSupplier = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0 && i.SupplierId == objectProduct.SupplierId).FirstOrDefault();

            if (objectProduct != null)
            {
                var objectProductFile = _lastguyShopContext.ProductFiles.Where(i => i.IsDelete == 0 && i.ProductId == objectProduct.ProductId).FirstOrDefault();
                ProductCashierModel ch = new ProductCashierModel();
                ch.productId = objectProduct.ProductId;
                ch.productName = objectProduct.Name;
                ch.price = objectProduct.Price.HasValue ? objectProduct.Price.Value : 0;
                ch.amount = objectProduct.Price.HasValue ? objectProduct.Price.Value : 0;
                //ch.cutAmount = objectProduct.cut.HasValue ? objectProduct.Price.Value : 0;
                return ch;
            }
            else
            {
                return null;
            }
        }
    }
}
