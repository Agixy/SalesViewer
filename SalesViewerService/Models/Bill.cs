using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesViewerService.Models
{
    [Table("DSR_DokumentySprzedazyRachunki")]
    public class Bill
    {
        [Key]
        public long Id { get; set; }
        public int Number { get; set; }      
        public int? TableId { get; set; }     
        public int? DiscountFormId { get; set; }
        public int WaiterId { get; set; }    
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? LastServiceDate { get; set; }
        public decimal? GuestsCount { get; set; }
        public string Description { get; set; }
        public decimal? SettedValueOfBill { get; set; }
    }
}
