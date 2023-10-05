namespace LastguyShop.Models.Product
{
    public class ManageProduct
    {
        //Product Data
        public ProductModel product { get; set; } = null;

        //Supplier Data
        public SupplierModel supplier { get; set; } = null;
        public List<PricrModel> price { get; set; } = null;
        public List<IFormFile> fileUploads { get; set; }
    }

    public class ProductModel
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int totalAmount { get; set; }
        public int SafetyStockNumber { get; set; }
        public string unit { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int historyPriceId { get; set; }
    }

    public class SupplierModel
    {
        public int supplierId { get; set; }
        public string? supplierName { get; set; }
        public string? address { get; set; }
        public string? phoneNumber { get; set; }
        public string? contactName { get; set; }
        public string? officeHours { get; set; }
        public string? workday { get; set; }
        public string? lineId { get; set; }
    }

    public class PricrModel
    {
        public int historyPriceId { get; set; }
        public string? note { get; set; }
        public DateTime createdDate { get; set; }
    }
}
