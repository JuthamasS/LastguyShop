namespace LastguyShop.Models.HistoryPrice
{
    public class ListHistoryPrice
    {
        public int historyId { get; set; }
        public int productId { get; set; }
        public int price { get; set; }
        public DateOnly createdDate { get; set; }
        public string note { get; set; }
    }
}
