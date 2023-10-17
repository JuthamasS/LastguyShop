﻿namespace LastguyShop.Models.Product
{
    public class ListProduct
    {
        public int currentPage { get; set; }
        public int pageCount { get; set; }
        public List<ProductItem> productList { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int totalAmount { get; set; }
        public string unit { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string supplierName { get; set; }
        public int? isFavorite { get; set; }
    }

    public class ProductItem
    {
        public int productId { get; set; }
        public string productName { get; set; }
        public int price { get; set; }
        public int totalAmount { get; set; }
        public string unit { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public string supplierName { get; set; }
        public int? isFavorite { get; set; }

    }
}
