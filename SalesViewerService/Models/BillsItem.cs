using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesViewerService.Models
{
    public class BillsItem
    {
        public long Id { get; set; }      
        public decimal NettoPrice { get; set; }
        public decimal BruttoPrice { get; set; }
        public decimal Count { get; set; }
        public MenuItem MenuItem { get; set; }
        public int MenuItemId { get; set; }
        public DateTime? CancellationDate { get; set; }
        public int? CancellingWaiterId { get; set; }

        [ForeignKey("BillId")]
        public Bill Bill { get; set; }
        public long BillId { get; set; }
    }
}
