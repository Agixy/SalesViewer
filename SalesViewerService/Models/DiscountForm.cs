using System;
using System.Collections.Generic;
using System.Text;

namespace SalesViewerService.Models
{
    public class DiscountForm
    {
        public int Id { get; set; }
        public byte DiscountTypeId { get; set; }
        public string Name { get; set; }
    }
}
