using LastguyShop.Data.Context;
using LastguyShop.Data.Entities;
using LastguyShop.Models;
using LastguyShop.Models.Product;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Office.Interop.Excel;
//using Spire.Xls;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
//using System.IO.Pipelines;
//using _excel = Microsoft.Office.Interop.Excel;
//using OfficeOpenXml;
using ClosedXML.Excel;
//using DocumentFormat.OpenXml.Drawing.Charts;
using System.Data;
using Irony.Parsing;
using LastguyShop.Models.HistoryPrice;
using System.Globalization;
using PagedList;
using Microsoft.Data.SqlClient;

namespace LastguyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LastguyShopContext _lastguyShopContext;
        private readonly string _documentDirectory = "FileStores";
        CultureInfo th = new CultureInfo("th-TH");

        public HomeController(ILogger<HomeController> logger, LastguyShopContext lastguyShopContext)
        {
            _logger = logger;
            _lastguyShopContext = lastguyShopContext;
        }

        #region list

        public IActionResult Index()
        {
            return RedirectToAction("ListProduct",new { name = "", isNearly = false, isFavorite = false });
        }

        public IActionResult ListProduct(string name, bool isNearly, bool isFavorite,int? page)
        {
            var list = SearchProduct(name, isNearly, isFavorite);

            int pageSize = 3;
            int pageNumber = (page ?? 3);
            var productList = list.ToPagedList(pageNumber, pageSize);

            return View(productList);
        }

        public List<ListProduct> SearchProduct(string name, bool isNearly, bool isFavorite)
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
                    objectProduct = objectProduct.Where(i => i.IsFavorite == 1);
                }

                foreach (var item in objectProduct)
                {
                    if (isNearly)
                    {
                        if (item.TotalAmount <= item.SafetyStockNumber)
                        {
                            objectProductList.Add(new ListProduct
                            {
                                productId = item.ProductId,
                                productName = item.Name,
                                price = item.Price.HasValue ? item.Price.Value : 0,
                                totalAmount = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                                unit = item.Unit,
                                status = (item.TotalAmount - item.SafetyStockNumber) > 0 ? "ปกติ" : "ใกล้หมด",
                                note = item.Note,
                                isFavorite = item.IsFavorite
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
                            note = item.Note,
                            isFavorite = item.IsFavorite
                        });
                    }

                }
            }
            return objectProductList;
        }

        public IActionResult setFavorite(int productId,bool favoriteStatus)
        {
            var productModel = _lastguyShopContext.Products.Where(i => i.IsDelete == 0 && i.ProductId == productId).FirstOrDefault();
            if (productModel != null)
            {
                productModel.IsFavorite = (favoriteStatus ? 0 : 1);
                    
                _lastguyShopContext.Products.Update(productModel);
                _lastguyShopContext.SaveChanges();
            }

            return RedirectToAction("ListProduct", new { name = "", isNearly = false, isFavorite = false });
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
            var productHistoryPricdeModel = new ProductHistoryPrice();
            var fileUploadModel = new FileUpload();
            var productFileModel = new ProductFile();

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

            if (productModel.ProductId != 0 && historyPricdeModel.HistoryPriceId != 0)
            {
                productHistoryPricdeModel.ProductId = productModel.ProductId;
                productHistoryPricdeModel.HistoryPriceId = historyPricdeModel.HistoryPriceId;
                productHistoryPricdeModel.CreatedDate = DateTime.Now;
                productHistoryPricdeModel.IsDelete = 0;

                _lastguyShopContext.ProductHistoryPrices.Add(productHistoryPricdeModel);
                _lastguyShopContext.SaveChanges();
            }

            if (param.fileUpload != null)
            {
                var fileItem = param.fileUpload;
                fileUploadModel.FolderPath = fileItem.FileName;
                fileUploadModel.FileName = fileItem.FileName;
                fileUploadModel.FileNameContent = fileItem.FileName;
                fileUploadModel.Mimetype = fileItem.FileName;
                fileUploadModel.Size = 0;
                fileUploadModel.CreatedDate = DateTime.Now;
                fileUploadModel.IsDelete = 0;
                _lastguyShopContext.FileUploads.Add(fileUploadModel);
                _lastguyShopContext.SaveChanges();

                productFileModel.ProductId = productModel.ProductId;
                productFileModel.FileId = fileUploadModel.FileId;
                productFileModel.CreatedDate = DateTime.Now;
                productFileModel.IsDelete = 0;
                _lastguyShopContext.ProductFiles.Add(productFileModel);
                _lastguyShopContext.SaveChanges();
            }
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
            //var historyPricdeModel = _lastguyShopContext.HistoryPrices.Where(i => i.IsDelete == 0 && i.HistoryPriceId == productModel.HistoryId).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
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

                        HistoryPrice historyPricdeModel = new HistoryPrice();
                        historyPricdeModel.Price = param.product.price;
                        historyPricdeModel.Note = "รายละเอียด : " + param.product.description + " หมายเหตุ : " + param.product.note;
                        historyPricdeModel.CreatedDate = DateTime.Now;
                        historyPricdeModel.ModifiedDate = DateTime.Now;
                        historyPricdeModel.IsDelete = 0;
                        _lastguyShopContext.HistoryPrices.Add(historyPricdeModel);
                        _lastguyShopContext.SaveChanges();

                        productModel.HistoryId = historyPricdeModel.HistoryPriceId;
                    }

                    _lastguyShopContext.Products.Update(productModel);
                    _lastguyShopContext.SaveChanges();
                }

                if (param.fileUpload != null)
                {
                    var checkOldFile = _lastguyShopContext.ProductFiles.Where(i => i.ProductId == param.product.productId).ToList();
                    if (checkOldFile.Count() > 0)
                    {
                        foreach (var item in checkOldFile)
                        {
                            var p_file = _lastguyShopContext.ProductFiles.Where(i => i.FileId == item.FileId).FirstOrDefault();
                            var u_file = _lastguyShopContext.FileUploads.Where(i => i.FileId == item.FileId).FirstOrDefault();

                            u_file.IsDelete = 1;
                            _lastguyShopContext.FileUploads.Update(u_file);
                            _lastguyShopContext.SaveChanges();

                            p_file.IsDelete = 1;
                            _lastguyShopContext.ProductFiles.Update(p_file);
                            _lastguyShopContext.SaveChanges();
                        }
                    }

                    string _dirname = Directory.GetCurrentDirectory();
                    string _filepath = Path.Combine(_dirname, "Storage\\FileUpload");
                    var _guid = Guid.NewGuid().ToString("N");

                    if (!Directory.Exists(_filepath))
                    {
                        Directory.CreateDirectory(_filepath);
                    }

                    var _filePathFull = Path.Combine(_filepath, _guid);
                    var _file = param.fileUpload;
                    using (var _fileStream = new FileStream(_filePathFull, FileMode.Create))
                    {
                        _file.CopyToAsync(_fileStream);
                        _fileStream.Close();
                    };

                    var fileUploadModel = new FileUpload();
                    var productFileModel = new ProductFile();

                    fileUploadModel.FolderPath = _filepath;
                    fileUploadModel.FileName = _file.FileName;
                    fileUploadModel.FileNameContent = _guid;
                    fileUploadModel.Mimetype = param.fileUpload.ContentType;
                    fileUploadModel.Size = Convert.ToInt32(_file.Length);
                    fileUploadModel.CreatedDate = DateTime.Now;
                    fileUploadModel.IsDelete = 0;
                    _lastguyShopContext.FileUploads.Add(fileUploadModel);
                    _lastguyShopContext.SaveChanges();

                    productFileModel.ProductId = productModel.ProductId;
                    productFileModel.FileId = fileUploadModel.FileId;
                    productFileModel.CreatedDate = DateTime.Now;
                    productFileModel.IsDelete = 0;
                    _lastguyShopContext.ProductFiles.Add(productFileModel);
                    _lastguyShopContext.SaveChanges();
                }

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
            if (productId != 0)
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
            }
            return RedirectToAction("ListProduct");
        }

        #endregion

        #region partial price

        public IActionResult ManageHistoryPricePartialView(int productId)
        {
            var productHistoryList = _lastguyShopContext.ProductHistoryPrices.Where(i => i.ProductId == productId).OrderByDescending(o => o.CreatedDate).ToList();

            if (productHistoryList != null)
            {
                List<ManageHistoryPrice> historyModelList = new List<ManageHistoryPrice>();
                foreach (var item in productHistoryList)
                {
                    var his = _lastguyShopContext.HistoryPrices.Where(i => i.HistoryPriceId == item.HistoryPriceId).FirstOrDefault();
                    historyModelList.Add(new ManageHistoryPrice
                    {
                        historyId = item.HistoryPriceId,
                        productId = item.ProductId,
                        price = his.Price.Value,
                        note = his.Note,
                        dateUpdate = item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("d MMM yyyy",th) : "-"
                    });
                }
                
                return PartialView(historyModelList);
            }
            else
            {
                return PartialView();
            }
        }

        public IActionResult ManageHistoryPriceAction()
        {

            return View();
        }

        #endregion

        #region report

        public IActionResult ReportProductAction()
        {
            //DataTable table = DummyDataTableSource();
            DataTable table = new DataTable();

            table.Columns.Add("Number");
            table.Columns.Add("ProductName");
            table.Columns.Add("SaftyNumber");
            table.Columns.Add("TotalAmount");


            DataRow dr1 = table.NewRow();
            dr1["Number"] = "Number" + 1;
            dr1["Number"] = "ProductName" + 1;
            dr1["Number"] = "SaftyNumber" + 1;
            dr1["Number"] = "TotalAmount" + 1;
            table.Rows.Add(dr1);

            DataRow dr2 = table.NewRow();
            dr2["Number"] = "Number" + 2;
            dr2["Number"] = "ProductName" + 2;
            dr2["Number"] = "SaftyNumber" + 2;
            dr2["Number"] = "TotalAmount" + 2;
            table.Rows.Add(dr2);

            using (XLWorkbook workbook = new XLWorkbook())
            {
                table.TableName = "Table 1";
                workbook.Worksheets.Add(table);
                workbook.Style.Font.Bold= true;
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.("content-disposition","attachment;filename=Dummy Excel Export.xlsx");
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(myMemoryStream);
                    //myMemoryStream.WriteTo(Response.output);
                    //Response.End();
                }
            }
            

                //var _excel = _exportExcelService.excelHelper(_unitOfWork, param);

            //    var objectProduct = _lastguyShopContext.Products.Where(i => i.IsDelete == 0).AsEnumerable();
            //if (objectProduct != null)
            //{
            //    foreach (var item in objectProduct)
            //    {
            //        if (item.TotalAmount <= item.SafetyStockNumber)
            //        {

            //        }
            //    }
            //}
            //var res = _reportService.GetReport();
            //if (res.status)
            //{
            //    if (res.data != null)
            //    {
            //        return Ok(res);
            //    }
            //    else
            //    {
            //        return NoContent();
            //    }
            //}
            //else
            //{
            //    return BadRequest(res);
            //}


            //string pathFile = "";
            //int sheet = 1;
            //_Application excel = new _excel._Application();

            //Workbook wb;
            //Worksheet ws;
            //wb.excel.Workbooks.Open(pathFile);
            //ws.wb.Worksheets[sheet];

            //string filePath = "C:\\Users\\monal\\source\\repos\\LGShop\\4\\LastguyShop\\LastguyShop\\Documents\\Excel\\ReportProduct.xlsx";
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //Workbook wb;
            //Worksheet ws;

            ////wb = excel.WorkbookOpen();
            //ws = wb.Worksheets[1];

            //Range cellRange = ws.Range["B1:B2"];
            //cellRange.value = "ชื่อสินค้า";
            //wb.SaveAsImage("C:\\Users\\monal\\source\\repos\\LGShop\\4\\LastguyShop\\LastguyShop\\Documents\\Excel\\ReportProduct.xlsx");
            //wb.Close();

            return RedirectToAction("ListProduct", new { name = "", isNearly = false, isFavorite = false });
        }

        private static DataTable DummyDataTableSource()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Number");
            table.Columns.Add("ProductName");
            table.Columns.Add("SaftyNumber");
            table.Columns.Add("TotalAmount");

            
            DataRow dr1 = table.NewRow();
            dr1["Number"] = "Number" + 1;
            dr1["Number"] = "ProductName" + 1;
            dr1["Number"] = "SaftyNumber" + 1;
            dr1["Number"] = "TotalAmount" + 1;
            table.Rows.Add(dr1);

            DataRow dr2 = table.NewRow();
            dr2["Number"] = "Number" + 2;
            dr2["Number"] = "ProductName" + 2;
            dr2["Number"] = "SaftyNumber" + 2;
            dr2["Number"] = "TotalAmount" + 2;
            table.Rows.Add(dr2);

            return table;
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}