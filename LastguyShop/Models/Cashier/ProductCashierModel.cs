namespace LastguyShop.Models.Cashier
{
    public class ProductCashierModel
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public string barcode { get; set; }
        public int price { get; set; }
        public int cash { get; set; }
        public int totalAmount { get; set; }
        public int cutAmount { get; set; }
        public bool isQrCode { get; set; }

        public DateTime? receiptDate { get; set; }
        public int amount { get; set; }
    }
}
