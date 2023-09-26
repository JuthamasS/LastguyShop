namespace LastguyShop.Models.Product
{
    public class ManageProduct
    {
        //Product Data
        public int productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int totalAmount { get; set; }
        public string unit { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public int historyPriceId { get; set; }

        //Supplier Data
        public int supplierId { get; set; }
        public string? supplierName { get; set; }
        public string? address { get; set; }
        public string? phoneNumber { get; set; }
        public string? contactName { get; set; }
        public string? officeHours { get; set; }
        public string? workday { get; set; }
        public string? lineId { get; set; }
    }
}
