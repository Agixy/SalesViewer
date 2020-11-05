using System;

namespace SalesViewer.Models
{
    public class BillViewModel
    {
        public int Number { get; set; }
        public string TableName { get; set; }
        public string DiscountType { get; set; }
        public string Waiter { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public int? GuestsCount { get; set; }
        public string Description { get; set; }
        public decimal NettoValueOfBill { get; set; }
        public decimal BruttoValueOfBill { get; set; }
        public decimal? SettedValueOfBill { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int? CancellingWaiter{ get; set; }
    }
}
