using LastguyShop.Data.Context;
using LastguyShop.Data.Entities;
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

        public ProductCashierModel GetDataDetail(string code)
        {
            var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.Barcode == code).FirstOrDefault();
            if (objectProduct != null)
            {
                var objectSupplier = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0 && i.SupplierId == objectProduct.SupplierId).FirstOrDefault();

                if (objectProduct != null)
                {
                    var objectPrice = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0 && i.HistoryPriceId == objectProduct.HistoryId).OrderByDescending(o => o.CreatedDate).FirstOrDefault();

                    var objectProductFile = _lastguyShopContext.ProductFiles.Where(i => i.IsDelete == 0 && i.ProductId == objectProduct.ProductId).FirstOrDefault();
                    ProductCashierModel ch = new ProductCashierModel();
                    ch.productId = objectProduct.ProductId;
                    ch.productName = objectProduct.Name;
                    ch.barcode = objectProduct.Barcode;
                    ch.price = objectProduct.Price.HasValue ? objectProduct.Price.Value : 0;
                    ch.totalAmount = objectProduct.TotalAmount.HasValue ? objectProduct.TotalAmount.Value : 0;
                    ch.cutAmount = objectProduct.CutUnit.HasValue ? objectProduct.CutUnit.Value : 0;
                    ch.productName = objectProduct.Name;
                    return ch;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public bool UpdateStock(List<ProductCashierModel> paramList)
        {
            try
            {
                if (paramList != null)
                {
                    foreach (var item in paramList)
                    {
                        var receiptModel = new Receipt();
                        receiptModel.ReceiptDate = DateTime.Now;
                        receiptModel.ProductId = item.productId;
                        receiptModel.Amount = item.amount;
                        receiptModel.Cash = item.cash;
                        receiptModel.CutUnit = item.cutAmount;
                        //receiptModel.IsQRCodePayment = (byte?)item.isQrCode ? 1 : 0;
                    }

                    var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0).AsEnumerable();

                    if (objectProduct != null)
                    {
                        //var objectProductFile = _lastguyShopContext.ProductFiles.Where(i => i.IsDelete == 0 && i.ProductId == objectProduct.ProductId).FirstOrDefault();
                        //ProductCashierModel ch = new ProductCashierModel();
                        //ch.productId = objectProduct.ProductId;
                        //ch.productName = objectProduct.Name;
                        //ch.price = objectProduct.Price.HasValue ? objectProduct.Price.Value : 0;
                        //ch.amount = objectProduct.TotalAmount.HasValue ? objectProduct.TotalAmount.Value : 0;
                        //ch.cutAmount = objectProduct.CutUnit.HasValue ? objectProduct.CutUnit.Value : 0;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
