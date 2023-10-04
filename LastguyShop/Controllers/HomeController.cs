using LastguyShop.Data.Context;
using LastguyShop.Data.Entities;
using LastguyShop.Models;
using LastguyShop.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO.Pipelines;

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
        #region list

        public IActionResult Index()
        {
            return RedirectToAction("ListProduct");
        }

        //[HttpPost]
        public IActionResult ListProduct(string name, bool isNearly, bool isFavorite)
        {
            var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0).AsEnumerable();
            var objectPrice = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0).OrderByDescending(o => o.CreatedDate).ToList();
            var objectSupplier = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0).OrderByDescending(o => o.CreatedDate).ToList();
            var objectProductList = new List<ListProduct>();
            if (objectProduct != null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    objectProduct = objectProduct.Where(i => i.Name.Contains(name));
                }

                if (isFavorite)
                {
                    objectProduct = objectProduct.Where(i => i.Name.Contains(name));
                }

                foreach (var item in objectProduct)
                {
                    if (isNearly)
                    {
                        if (item.TotalAmount > item.SafetyStockNumber)
                        {
                            objectProductList.Add(new ListProduct
                            {
                                productId = item.ProductId,
                                productName = item.Name,
                                price = item.Price.HasValue ? item.Price.Value : 0,
                                totalAmount = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                                unit = item.Unit,
                                status = (item.TotalAmount - item.SafetyStockNumber) > 0 ? "ปกติ" : "ใกล้หมด",
                                note = item.Note
                            });
                        }
                    }
                    else
                    {
                        objectProductList.Add(new ListProduct
                        {
                            productId = item.ProductId,
                            productName = item.Name,
                            price = item.Price.HasValue ? item.Price.Value : 0,
                            totalAmount = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                            unit = item.Unit,
                            status = (item.TotalAmount - item.SafetyStockNumber) > 0 ? "ปกติ" : "ใกล้หมด",
                            note = item.Note
                        });
                    }

                }
            }
            return View(objectProductList);
        }

        #endregion

        #region detail

        public IActionResult DetailProduct(int productId)
        {
            var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.ProductId == productId).FirstOrDefault();
            var objectPrice = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0 && i.HistoryPriceId == objectProduct.HistoryId).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            var objectSupplier = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0 && i.SupplierId == objectProduct.SupplierId).FirstOrDefault();

            if (objectProduct != null)
            {
                var productModel = new ProductModel()
                {
                    productId = objectProduct.ProductId,
                    productName = objectProduct.Name,
                    price = objectProduct.Price.HasValue ? objectProduct.Price.Value : 0,
                    totalAmount = objectProduct.TotalAmount.HasValue ? objectProduct.TotalAmount.Value : 0,
                    unit = objectProduct.Unit,
                    description = objectProduct.Description,
                    note = objectProduct.Note,
                    SafetyStockNumber = objectProduct.SafetyStockNumber.HasValue ? objectProduct.SafetyStockNumber.Value : 0
                };

                var supplierModel = new SupplierModel();
                if (objectSupplier != null)
                {
                    supplierModel.supplierId = objectSupplier.SupplierId;
                    supplierModel.supplierName = !string.IsNullOrEmpty(objectSupplier.Name) ? objectSupplier.Name : "-";
                    supplierModel.address = !string.IsNullOrEmpty(objectSupplier.Address) ? objectSupplier.Address : "-";
                    supplierModel.phoneNumber = !string.IsNullOrEmpty(objectSupplier.PhoneNumber) ? objectSupplier.PhoneNumber : "-";
                    supplierModel.contactName = !string.IsNullOrEmpty(objectSupplier.ContactName) ? objectSupplier.ContactName : "-";
                    supplierModel.officeHours = !string.IsNullOrEmpty(objectSupplier.OfficeHours) ? objectSupplier.OfficeHours : "-";
                    supplierModel.workday = !string.IsNullOrEmpty(objectSupplier.Workday) ? objectSupplier.Workday : "-";
                    supplierModel.lineId = !string.IsNullOrEmpty(objectSupplier.LineId) ? objectSupplier.LineId : "-";
                }

                var manageModel = new ManageProduct();
                manageModel.product = productModel;
                manageModel.supplier = supplierModel;

                return View(manageModel);
            }
            else
            {
                return View();
            }
        }

        #endregion

        #region insert

        public IActionResult InsertProduct()
        {
            return View();
        }

        public IActionResult InsertAction(ManageProduct param)
        {
            var supplierModel = new Supplier();
            var productModel = new Product();
            var historyPricdeModel = new HistoryPrice();

            if (param.supplier != null)
            {
                supplierModel.Name = param.supplier.supplierName != null ? param.supplier.supplierName : "-";
                supplierModel.Address = param.supplier.address != null ? param.supplier.address : "-";
                supplierModel.PhoneNumber = param.supplier.phoneNumber != null ? param.supplier.phoneNumber : "-";
                supplierModel.ContactName = param.supplier.contactName != null ? param.supplier.contactName : "-";
                supplierModel.Workday = param.supplier.workday != null ? param.supplier.workday : "-";
                supplierModel.OfficeHours = param.supplier.officeHours != null ? param.supplier.officeHours : "-";
                supplierModel.LineId = param.supplier.lineId != null ? param.supplier.lineId : "-";
                supplierModel.CreatedDate = DateTime.Now;
                supplierModel.ModifiedDate = DateTime.Now;
                supplierModel.IsDelete = 0;

                _lastguyShopContext.Suppliers.Add(supplierModel);
                _lastguyShopContext.SaveChanges();
            }

            if (param.product.price != 0)
            {
                historyPricdeModel.Price = param.product.price;
                historyPricdeModel.Note = "รายละเอียด : " + param.product.description + " หมายเหตุ : " + param.product.note;
                historyPricdeModel.CreatedDate = DateTime.Now;
                historyPricdeModel.ModifiedDate = DateTime.Now;
                historyPricdeModel.IsDelete = 0;

                _lastguyShopContext.HistoryPrices.Add(historyPricdeModel);
                _lastguyShopContext.SaveChanges();
            }

            if (param.product != null)
            {
                productModel.Name = param.product.productName;
                productModel.Price = param.product.price;
                productModel.TotalAmount = param.product.totalAmount;
                productModel.Unit = param.product.unit;
                productModel.SafetyStockNumber = param.product.SafetyStockNumber;
                productModel.Description = param.product.description;
                productModel.Note = param.product.note;
                productModel.SupplierId = supplierModel.SupplierId;
                productModel.HistoryId = historyPricdeModel.HistoryPriceId;
                productModel.CreatedDate = DateTime.Now;
                productModel.ModifiedDate = DateTime.Now;
                productModel.IsDelete = 0;

                _lastguyShopContext.Products.Add(productModel);
                _lastguyShopContext.SaveChanges();
            }

            //var result = _lastguyShopContext.SaveChanges();

            //if (result > 0)
            //{
            //    return RedirectToAction("ListProduct");
            //}
            //else
            //{
            //    return RedirectToAction("ListProduct");
            //}
            return RedirectToAction("ListProduct");
        }

        #endregion

        #region update

        public IActionResult ManageProduct(int productId)
        {
            var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.ProductId == productId).FirstOrDefault();
            var objectPrice = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0 && i.HistoryPriceId == objectProduct.HistoryId).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            var objectSupplier = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0 && i.SupplierId == objectProduct.SupplierId).FirstOrDefault();

            if (objectProduct != null)
            {
                var productModel = new ProductModel()
                {
                    productId = objectProduct.ProductId,
                    productName = objectProduct.Name,
                    price = objectProduct.Price.HasValue ? objectProduct.Price.Value : 0,
                    totalAmount = objectProduct.TotalAmount.HasValue ? objectProduct.TotalAmount.Value : 0,
                    unit = objectProduct.Unit,
                    description = objectProduct.Description,
                    note = objectProduct.Note,
                    SafetyStockNumber = objectProduct.SafetyStockNumber.HasValue ? objectProduct.SafetyStockNumber.Value : 0
                };

                var supplierModel = new SupplierModel();
                if (objectSupplier != null)
                {
                    supplierModel.supplierId = objectSupplier.SupplierId;
                    supplierModel.supplierName = objectSupplier.Name;
                    supplierModel.address = objectSupplier.Address;
                    supplierModel.phoneNumber = objectSupplier.PhoneNumber;
                    supplierModel.contactName = objectSupplier.ContactName;
                    supplierModel.officeHours = objectSupplier.OfficeHours;
                    supplierModel.workday = objectSupplier.Workday;
                    supplierModel.lineId = objectSupplier.LineId;
                }

                var manageModel = new ManageProduct();
                manageModel.product = productModel;
                manageModel.supplier = supplierModel;

                return View(manageModel);
            }
            else
            {
                return View();
            }

        }
        public IActionResult ManageProductAction(ManageProduct param)
        {
            var productModel = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.ProductId == param.product.productId).FirstOrDefault();
            var historyPricdeModel = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0 && i.HistoryPriceId == param.product.historyPriceId).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            var supplierModel = _lastguyShopContext.Suppliers.Where(i => i.IsDelete == 0 && i.SupplierId == param.supplier.supplierId).FirstOrDefault();

            if (supplierModel != null)
            {
                if (param.supplier != null)
                {
                    supplierModel.Name = param.supplier.supplierName != null ? param.supplier.supplierName : "-";
                    supplierModel.Address = param.supplier.address != null ? param.supplier.address : "-";
                    supplierModel.PhoneNumber = param.supplier.phoneNumber != null ? param.supplier.phoneNumber : "-";
                    supplierModel.ContactName = param.supplier.contactName != null ? param.supplier.contactName : "-";
                    supplierModel.Workday = param.supplier.workday != null ? param.supplier.workday : "-";
                    supplierModel.OfficeHours = param.supplier.officeHours != null ? param.supplier.officeHours : "-";
                    supplierModel.LineId = param.supplier.lineId != null ? param.supplier.lineId : "-";
                    supplierModel.ModifiedDate = DateTime.Now;
                    _lastguyShopContext.Suppliers.Update(supplierModel);
                    _lastguyShopContext.SaveChanges();
                }
            }

            if (productModel != null)
            {
                if (param.product != null)
                {
                    productModel.Name = param.product.productName != null ? param.product.productName : "-";
                    productModel.TotalAmount = param.product.totalAmount != null ? param.product.totalAmount : 0;
                    productModel.Unit = param.product.unit != null ? param.product.unit : "-";
                    productModel.SafetyStockNumber = param.product.SafetyStockNumber != null ? param.product.SafetyStockNumber : 0;
                    productModel.Description = param.product.description != null ? param.product.description : "-";
                    productModel.Note = param.product.note != null ? param.product.note : "-";
                    productModel.ModifiedDate = DateTime.Now;

                    if (param.product.price != productModel.Price)
                    {
                        productModel.Price = param.product.price;

                        historyPricdeModel.Price = param.product.price;
                        historyPricdeModel.Note = "รายละเอียด : " + param.product.description + " หมายเหตุ : " + param.product.note;
                        historyPricdeModel.ModifiedDate = DateTime.Now;
                        _lastguyShopContext.HistoryPrices.Add(historyPricdeModel);
                        _lastguyShopContext.SaveChanges();

                        productModel.HistoryId = historyPricdeModel.HistoryPriceId;
                    }

                    _lastguyShopContext.Products.Update(productModel);
                    _lastguyShopContext.SaveChanges();
                }

                //var result = _lastguyShopContext.SaveChanges();

                //if (result > 0)
                //{
                //    return RedirectToAction("ListProduct");
                //}
                //else
                //{
                //    return RedirectToAction("ListProduct");
                //}
                return RedirectToAction("ListProduct");
            }
            else
            {
                return RedirectToAction("ListProduct");
            }
        }

        #endregion

        #region delete

        public IActionResult DeleteProductAction(int productId)
        {
            var productModel = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.ProductId == productId).FirstOrDefault();

            if (productModel != null)
            {
                //_lastguyShopContext.Products.Remove(productModel);
                productModel.IsDelete = 1;
                productModel.ModifiedDate = DateTime.Now;
                _lastguyShopContext.Products.Update(productModel);
                var result = _lastguyShopContext.SaveChanges();

                if (result > 0)
                {
                    return RedirectToAction("ListProduct");
                }
                else
                {
                    return RedirectToAction("ListProduct");
                }
            }
            return RedirectToAction("ListProduct");
        }

        #endregion

        #region partial price

        public IActionResult ManageHistoryPricePatialView()
        {

            return View();
        }

        public IActionResult ManageHistoryPriceAction()
        {

            return View();
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}