namespace SalesViewerService.Models
{
    public class BillsItem
    {
        public long Id { get; set; }
        public long BillsId { get; set; }
        public decimal NettoPrice { get; set; }
        public decimal BruttoPrice { get; set; }
        public decimal Count { get; set; }
        public int MenuItemId { get; set; }
    }
}
