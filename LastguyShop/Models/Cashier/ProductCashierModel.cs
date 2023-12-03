namespace LastguyShop.Models.Cashier
{
    public class ProductCashierModel
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int amount { get; set; }
        public int cutAmount { get; set; }
    }
}
